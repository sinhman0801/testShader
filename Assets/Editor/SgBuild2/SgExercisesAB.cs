using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

internal static class SgExercisesAB
{
    static readonly Color Orange = new Color(1f, 0.55f, 0.1f);

    // ---------------------------------------------------------------- A: Dissolve
    // Noise + Alpha Clip + Mask + Emission. Lit, Opaque, Alpha Clipping on.
    public static void ExA()
    {
        var c = new Ctx("A_Dissolve");
        var g = new Sg(true, true, true, ("alphaClip", true));
        var scale = g.Float("Noise Scale", "_NoiseScale", 6f, 1f, 30f);
        var thr = g.Float("Threshold", "_Threshold", 0.45f, 0f, 1f);
        var width = g.Float("Edge Width", "_EdgeWidth", 0.06f, 0f, 0.3f);
        var inten = g.Float("Edge Intensity", "_EdgeIntensity", 3f, 0f, 10f);
        var edgeCol = g.ColorP("Edge Color", "_EdgeColor", Orange, hdr: true);
        var baseCol = g.ColorP("Base Color", "_BaseTint", new Color(0.12f, 0.12f, 0.13f));

        // 01 / surviving area: n -> Alpha, Threshold -> Alpha Clip Threshold
        var n = g.Mk<SaturateNode>("In", g.Mk<GradientNoiseNode>("UV", g.UV2(), "Scale", scale));
        g.ToBlock(n, "Alpha");
        g.ToBlock(thr, "AlphaClipThreshold");

        // 02 / edge band on the visible side of the cut: Threshold <= n < Threshold + EdgeWidth
        var low = g.Mk<StepNode>("Edge", thr, "In", n);
        var high = g.Mk<StepNode>("Edge", g.Mk<AddNode>("A", thr, "B", width), "In", n);
        var edge = g.Mk<SaturateNode>("In", g.Mk<SubtractNode>("A", low, "B", high));

        // 03 / burning edge colour, kept separate from Base Color
        g.ToBlock(g.Mk<MultiplyNode>("A", g.Mk<MultiplyNode>("A", edgeCol, "B", inten), "B", edge), "Emission");
        g.ToBlock(baseCol, "BaseColor");

        var sh = c.Graph(g, "SG_Dissolve");
        Material M(string file, float t, float w) => c.Mat(file, sh, m =>
        {
            m.SetFloat("_Threshold", t); m.SetFloat("_EdgeWidth", w); m.SetFloat("_NoiseScale", 6f);
            m.SetFloat("_EdgeIntensity", 3f); m.SetColor("_EdgeColor", Orange); m.SetColor("_BaseTint", new Color(0.12f, 0.12f, 0.13f));
        });
        var t02 = M("Threshold_0.2", 0.2f, 0.06f);
        var t05 = M("Threshold_0.5", 0.5f, 0.06f);
        var t08 = M("Threshold_0.8", 0.8f, 0.06f);
        var wide = M("Threshold_0.5_EdgeWide0.15", 0.5f, 0.15f);
        var thin = M("Threshold_0.5_EdgeThin0.02", 0.5f, 0.02f);
        c.Scene(sc =>
        {
            sc.Add(PrimitiveType.Sphere, "Sphere_T0.2", new Vector3(-4.4f, 0.5f, 0), t02);
            sc.Add(PrimitiveType.Sphere, "Sphere_T0.5", new Vector3(-2.2f, 0.5f, 0), t05);
            sc.Add(PrimitiveType.Sphere, "Sphere_T0.8", new Vector3(0f, 0.5f, 0), t08);
            sc.Add(PrimitiveType.Capsule, "Capsule_EdgeWide", new Vector3(2.2f, 1f, 0), wide);
            sc.Add(PrimitiveType.Cube, "Cube_EdgeThin", new Vector3(4.4f, 0.5f, 0), thin, euler: new Vector3(0, 25, 0));
            sc.Bloom(1.2f, 0.9f);
        }, new Vector3(0f, 1.8f, -8f), new Vector3(10f, 0f, 0f));
    }

    // ---------------------------------------------------------------- B: Co lay (swaying grass)
    // Position + Time + Sine + Mask + Displacement. Unlit first, per the PDF.
    public static void ExB()
    {
        var c = new Ctx("B_Co_Lay");
        var g = new Sg(false, false, false, ("renderFace", "Both"));
        var amp = g.Float("Amplitude (object units)", "_Amplitude", 0.1f, 0f, 0.5f);
        var spd = g.Float("Speed (rad/s)", "_Speed", 2f, 0f, 10f);
        var freq = g.Float("Frequency", "_Frequency", 3f, 0f, 10f);
        var phase = g.Float("Phase", "_Phase", 0f, 0f, 6.28318f);
        var wind = g.Vec3("Wind Direction (object space)", "_WindDir", 1f, 0f, 0f);
        var rootPow = g.Float("Root Lock Power", "_RootPower", 2f, 1f, 5f);
        var phaseVar = g.Float("Phase Per Position", "_PhaseVar", 3f, 0f, 10f);
        var colLow = g.ColorP("Color Root", "_ColorRoot", new Color(0.08f, 0.3f, 0.1f));
        var colHigh = g.ColorP("Color Tip", "_ColorTip", new Color(0.65f, 0.9f, 0.25f));

        // 01 / root lock: m = Power(Saturate(UV.y), 2)  -> root 0, tip 1
        var uvy = g.O(g.Mk<SplitNode>("In", g.UV2()), "G");
        var mask = g.Mk<PowerNode>("A", g.Mk<SaturateNode>("In", uvy), "B", rootPow);

        // 02 / oscillation: sin(y*Frequency + Time*Speed + Phase (+ per-position variation)) * Amplitude * m
        var pos = g.Pos(CoordinateSpace.Object);
        var p = g.Mk<SplitNode>("In", pos);
        var time = g.N<TimeNode>();
        var posPhase = g.Mk<MultiplyNode>("A", g.Mk<AddNode>("A", g.O(p, "R"), "B", g.O(p, "B")), "B", phaseVar);
        var arg = g.Mk<AddNode>(
            "A", g.Mk<AddNode>("A", g.Mk<MultiplyNode>("A", g.O(p, "G"), "B", freq),
                               "B", g.Mk<MultiplyNode>("A", g.O(time, "Time"), "B", spd)),
            "B", g.Mk<AddNode>("A", phase, "B", posPhase));
        var amount = g.Mk<MultiplyNode>("A", g.Mk<MultiplyNode>("A", g.Mk<SineNode>("In", arg), "B", amp), "B", mask);

        // 03 / displace: Position + normalize(WindDirection) * amount  (must add to the original position)
        var offset = g.Mk<MultiplyNode>("A", g.Mk<NormalizeNode>("In", wind), "B", amount);
        g.ToBlock(g.Mk<AddNode>("A", pos, "B", offset), "Position");
        g.ToBlock(g.Mk<LerpNode>("A", colLow, "B", colHigh, "T", uvy), "BaseColor");

        var sh = c.Graph(g, "SG_Grass");
        var mesh = Tuft(c);
        Material M(string file, float ph) => c.Mat(file, sh, m => m.SetFloat("_Phase", ph));
        var m0 = M("Grass_Phase0", 0f);
        var m1 = M("Grass_Phase1.7", 1.7f);
        c.Scene(sc =>
        {
            var mats = new[] { m0, m1, m0, m1, m0 };
            for (int i = 0; i < 5; i++)
                sc.AddMesh(mesh, "Tuft_" + i, new Vector3(-4f + i * 2f, 0f, 0f), mats[i], new Vector3(0f, i == 4 ? 90f : 0f, 0f));
        }, new Vector3(0f, 1.2f, -5f), new Vector3(10f, 0f, 0f));
    }

    // A tuft of 9 tapered blades; UV.y = 0 at the root, 1 at the tip. 8 rows per blade.
    static Mesh Tuft(Ctx c)
    {
        const int blades = 9, rows = 8;
        var verts = new List<Vector3>();
        var uvs = new List<Vector2>();
        var tris = new List<int>();
        for (int b = 0; b < blades; b++)
        {
            float ang = b * 2.399963f;                       // golden angle spread
            float r = 0.16f * Mathf.Sqrt(b);
            var root = new Vector3(Mathf.Cos(ang) * r, 0f, Mathf.Sin(ang) * r);
            float yaw = ang * 1.7f;
            var dir = new Vector3(Mathf.Cos(yaw), 0f, Mathf.Sin(yaw));
            float h = 0.8f + 0.08f * ((b * 37) % 5);
            int start = verts.Count;
            for (int k = 0; k <= rows; k++)
            {
                float t = k / (float)rows;
                float hw = 0.07f * (1f - t);
                var mid = root + Vector3.up * (t * h);
                verts.Add(mid - dir * hw); uvs.Add(new Vector2(0f, t));
                verts.Add(mid + dir * hw); uvs.Add(new Vector2(1f, t));
            }
            for (int k = 0; k < rows; k++)
            {
                int i = start + k * 2;
                tris.AddRange(new[] { i, i + 2, i + 1, i + 1, i + 2, i + 3 });
            }
        }
        var mesh = new Mesh { name = "GrassTuft" };
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        var bnd = mesh.bounds;
        bnd.Expand(0.4f);                                    // room for the GPU-side sway, avoids culling at the edges
        mesh.bounds = bnd;
        Directory.CreateDirectory(c.root + "/Mesh");
        AssetDatabase.CreateAsset(mesh, c.root + "/Mesh/GrassTuft.asset");
        return mesh;
    }
}

// Runs once when the editor has compiled this folder, then deletes the folder. Status goes to a file.
[InitializeOnLoad]
internal static class SgAutoRun
{
    const string Status = "C:/Users/PC/AppData/Local/Temp/claude/F--Project-test1/e961f0f4-91b3-47ee-bbff-5cae5da4241d/scratchpad/sg2-status.txt";

    static SgAutoRun()
    {
        if (!File.Exists(Status)) EditorApplication.delayCall += Go;
    }

    static void Go()
    {
        if (File.Exists(Status)) return;
        string result = "OK";
        foreach (var (name, fn) in new (string, Action)[] { ("A", SgExercisesAB.ExA), ("B", SgExercisesAB.ExB) })
        {
            try { fn(); Sg.LogSb.AppendLine("[SG-OK] " + name); }
            catch (Exception e) { result = "FAIL"; Sg.LogSb.AppendLine($"[SG-FAIL] {name}: {e}"); }
        }
        AssetDatabase.SaveAssets();
        File.WriteAllText(Status, result + "\n" + Sg.LogSb);
        if (result == "OK")
        {
            AssetDatabase.DeleteAsset("Assets/Editor/SgBuild2");
            if (Directory.GetFileSystemEntries("Assets/Editor").Length == 0)
                AssetDatabase.DeleteAsset("Assets/Editor");
        }
    }
}
