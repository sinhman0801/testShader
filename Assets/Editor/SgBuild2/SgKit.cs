using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Graphing;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

// One-shot tooling: builds .shadergraph files through the Shader Graph API. Deletes itself after running.
internal struct Out
{
    public AbstractMaterialNode n;
    public string s;
}

internal class Sg
{
    const string Asm = ", Unity.RenderPipelines.Universal.Editor";
    const string Ns = "UnityEditor.Rendering.Universal.ShaderGraph.";
    public static readonly StringBuilder LogSb = new StringBuilder();

    public readonly GraphData graph = new GraphData();
    readonly List<AbstractMaterialNode> nodes = new List<AbstractMaterialNode>();
    readonly List<AbstractShaderProperty> props = new List<AbstractShaderProperty>();
    readonly List<(AbstractMaterialNode from, AbstractMaterialNode to)> links = new List<(AbstractMaterialNode, AbstractMaterialNode)>();
    readonly HashSet<AbstractMaterialNode> vertexBlocks = new HashSet<AbstractMaterialNode>();
    readonly Dictionary<string, BlockNode> blocks = new Dictionary<string, BlockNode>();

    public Sg(bool lit, bool alpha = false, bool clip = false, params (string prop, object value)[] settings)
    {
        var tType = Type.GetType(Ns + "UniversalTarget" + Asm, true);
        var target = (Target)Activator.CreateInstance(tType);
        var sub = Type.GetType(Ns + (lit ? "UniversalLitSubTarget" : "UniversalUnlitSubTarget") + Asm, true);
        tType.GetMethod("TrySetActiveSubTarget").Invoke(target, new object[] { sub });
        foreach (var (prop, value) in settings)
        {
            var p = tType.GetProperty(prop) ?? throw new Exception("UniversalTarget has no property " + prop);
            p.SetValue(target, p.PropertyType.IsEnum ? Enum.Parse(p.PropertyType, (string)value) : value);
        }

        var d = new List<BlockFieldDescriptor>
        {
            BlockFields.VertexDescription.Position,
            BlockFields.VertexDescription.Normal,
            BlockFields.VertexDescription.Tangent,
            BlockFields.SurfaceDescription.BaseColor,
        };
        if (lit)
            d.AddRange(new[]
            {
                BlockFields.SurfaceDescription.NormalTS, BlockFields.SurfaceDescription.Metallic,
                BlockFields.SurfaceDescription.Smoothness, BlockFields.SurfaceDescription.Emission,
                BlockFields.SurfaceDescription.Occlusion,
            });
        if (alpha) d.Add(BlockFields.SurfaceDescription.Alpha);
        if (clip) d.Add(BlockFields.SurfaceDescription.AlphaClipThreshold);

        graph.AddContexts();
        graph.InitializeOutputs(new[] { target }, d.ToArray());
        graph.path = "Shader Graphs";
        foreach (var b in graph.GetNodes<BlockNode>())
            blocks[b.descriptor.name] = b;
    }

    // ---------- nodes & links ----------
    public T N<T>() where T : AbstractMaterialNode, new()
    {
        var n = new T();
        graph.AddNode(n);
        nodes.Add(n);
        return n;
    }

    public Out O(AbstractMaterialNode n, string slot) => new Out { n = n, s = slot };

    static string Key(string s)
    {
        var i = s.IndexOf('(');
        if (i >= 0) s = s.Substring(0, i);
        return s.Replace(" ", "").ToLowerInvariant();
    }

    static MaterialSlot Slot(AbstractMaterialNode n, string name, bool input)
    {
        var l = new List<MaterialSlot>();
        n.GetSlots(l);
        var m = l.FirstOrDefault(s => s.isInputSlot == input && (name == null || Key(s.displayName) == Key(name)));
        if (m == null)
            throw new Exception($"{n.GetType().Name}: no {(input ? "input" : "output")} slot '{name}'. Have: " +
                                string.Join(", ", l.Select(s => (s.isInputSlot ? "in:" : "out:") + s.displayName)));
        return m;
    }

    public void Link(AbstractMaterialNode from, string outName, AbstractMaterialNode to, string inName)
    {
        var o = Slot(from, outName, false);
        var i = Slot(to, inName, true);
        graph.Connect(new SlotReference(from, o.id), new SlotReference(to, i.id));
        links.Add((from, to));
    }

    // Create node T and connect inputs: Mk<AddNode>("A", nodeOrOut, "B", nodeOrOut)
    public T Mk<T>(params object[] pairs) where T : AbstractMaterialNode, new()
    {
        var n = N<T>();
        for (int i = 0; i < pairs.Length; i += 2)
        {
            var slot = (string)pairs[i];
            if (pairs[i + 1] is Out o) Link(o.n, o.s, n, slot);
            else Link((AbstractMaterialNode)pairs[i + 1], null, n, slot);
        }
        return n;
    }

    public void ToBlock(object src, string block)
    {
        var o = src is Out ou ? ou : new Out { n = (AbstractMaterialNode)src };
        var b = blocks[block];
        Link(o.n, o.s, b, null);
        if (b.descriptor.shaderStage == UnityEditor.ShaderGraph.ShaderStage.Vertex) vertexBlocks.Add(b);
    }

    // ---------- constants ----------
    static void SetF(AbstractMaterialNode n, string slot, float v) => ((Vector1MaterialSlot)Slot(n, slot, true)).value = v;

    public Vector1Node Fl(float v) { var n = N<Vector1Node>(); SetF(n, "X", v); return n; }
    public Vector2Node V2(float x, float y) { var n = N<Vector2Node>(); SetF(n, "X", x); SetF(n, "Y", y); return n; }

    public ColorNode Col(Color c, bool hdr = false)
    {
        var n = N<ColorNode>();
        n.color = new ColorNode.Color(c, hdr ? ColorMode.HDR : ColorMode.Default);
        return n;
    }

    public TilingAndOffsetNode UV2() => N<TilingAndOffsetNode>(); // UV0 as Vector2 (default tiling 1, offset 0)

    public PositionNode Pos(CoordinateSpace space) => Space(N<PositionNode>(), space);

    static T Space<T>(T n, CoordinateSpace space) where T : GeometryNode
    {
        typeof(GeometryNode).GetField("m_Space", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(n, space);
        return n;
    }

    // ---------- blackboard properties ----------
    PropertyNode Wrap(AbstractShaderProperty p, string display, string refName)
    {
        p.displayName = display;
        p.overrideReferenceName = refName;
        graph.AddGraphInput(p);
        props.Add(p);
        var n = new PropertyNode();
        graph.AddNode(n);
        n.property = p;
        nodes.Add(n);
        return n;
    }

    public PropertyNode Float(string display, string refName, float v, float? min = null, float? max = null)
    {
        var p = new Vector1ShaderProperty { value = v };
        if (min.HasValue)
        {
            p.floatType = FloatType.Slider;
            p.rangeValues = new Vector2(min.Value, max.Value);
        }
        return Wrap(p, display, refName);
    }

    public PropertyNode Vec3(string display, string refName, float x, float y, float z) =>
        Wrap(new Vector3ShaderProperty { value = new Vector4(x, y, z, 0) }, display, refName);

    public PropertyNode ColorP(string display, string refName, Color c, bool hdr = false) =>
        Wrap(new ColorShaderProperty { value = c, colorMode = hdr ? ColorMode.HDR : ColorMode.Default }, display, refName);

    // ---------- layout + save ----------
    void Layout()
    {
        var feedsVertex = new HashSet<AbstractMaterialNode>();
        var stack = new Stack<AbstractMaterialNode>(vertexBlocks);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            foreach (var l in links.Where(l => l.to == n))
                if (feedsVertex.Add(l.from)) stack.Push(l.from);
        }
        var feedsFragment = new HashSet<AbstractMaterialNode>();
        stack = new Stack<AbstractMaterialNode>(blocks.Values.Where(b => !vertexBlocks.Contains(b) && links.Any(l => l.to == b)));
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            foreach (var l in links.Where(l => l.to == n))
                if (feedsFragment.Add(l.from)) stack.Push(l.from);
        }

        var depth = new Dictionary<AbstractMaterialNode, int>();
        int Depth(AbstractMaterialNode n)
        {
            if (depth.TryGetValue(n, out var d)) return d;
            depth[n] = 0;
            d = links.Where(l => l.to == n).Select(l => Depth(l.from) + 1).DefaultIfEmpty(0).Max();
            return depth[n] = d;
        }

        var cols = new Dictionary<(bool v, int x), int>();
        float maxFrag = 0, maxVert = 0;
        foreach (var n in nodes)
        {
            bool v = feedsVertex.Contains(n) && !feedsFragment.Contains(n);
            int x = Depth(n);
            cols.TryGetValue((v, x), out var row);
            cols[(v, x)] = row + 1;
            float px = x * 280f, py = (v ? -520f : 0f) + row * 150f;
            n.drawState = new DrawState { position = new Rect(px, py, 0, 0), expanded = true };
            if (v) maxVert = Mathf.Max(maxVert, px); else maxFrag = Mathf.Max(maxFrag, px);
        }
        graph.vertexContext.position = new Vector2(maxVert + 320f, -520f);
        graph.fragmentContext.position = new Vector2(maxFrag + 320f, 0f);
    }

    public Shader Save(string path)
    {
        Layout();
        graph.AddCategory(CategoryData.DefaultCategory(props.Cast<ShaderInput>().ToList()));
        if (FileUtilities.WriteShaderGraphToDisk(path, graph) == null)
            throw new Exception("write failed: " + path);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        var shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
        if (shader == null) throw new Exception("no shader imported at " + path);
        var msgs = ShaderUtil.GetShaderMessages(shader);
        bool bad = ShaderUtil.ShaderHasError(shader);
        foreach (var m in msgs)
            LogSb.AppendLine($"[SG-MSG] {Path.GetFileName(path)}: {m.severity}: {m.message} {m.messageDetails}");
        LogSb.AppendLine($"[SG] {path} hasError={bad} messages={msgs.Length}");
        return shader;
    }
}
