using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// One exercise folder: material helpers + scene builder + screenshot.
internal class Ctx
{
    public const string ShotDir = "C:/Users/PC/AppData/Local/Temp/claude/F--Project-test1/e961f0f4-91b3-47ee-bbff-5cae5da4241d/scratchpad/shots2";
    public readonly string name;
    public readonly string root;

    public Ctx(string name)
    {
        this.name = name;
        root = "Assets/Exercises/" + name;
        foreach (var d in new[] { "Shaders", "Materials", "Textures" })
            Directory.CreateDirectory($"{root}/{d}");
        AssetDatabase.Refresh();
    }

    public Shader Graph(Sg g, string file) => g.Save($"{root}/Shaders/{file}.shadergraph");

    public Material Mat(string file, Shader s, Action<Material> setup = null)
    {
        var m = new Material(s);
        setup?.Invoke(m);
        AssetDatabase.CreateAsset(m, $"{root}/Materials/{file}.mat");
        return m;
    }

    public Material Lit(string file, Color c, float smooth = 0.2f) =>
        Mat(file, Shader.Find("Universal Render Pipeline/Lit"), m =>
        {
            m.SetColor("_BaseColor", c);
            m.SetFloat("_Smoothness", smooth);
        });

    public class Scn
    {
        public readonly Ctx c;
        public Scn(Ctx c) { this.c = c; }

        public GameObject Add(PrimitiveType t, string n, Vector3 pos, Material m, Vector3? scale = null, Vector3? euler = null)
        {
            var go = GameObject.CreatePrimitive(t);
            go.name = n;
            go.transform.position = pos;
            go.transform.localScale = scale ?? Vector3.one;
            go.transform.eulerAngles = euler ?? Vector3.zero;
            if (m != null) go.GetComponent<Renderer>().sharedMaterial = m;
            return go;
        }

        public GameObject AddMesh(Mesh mesh, string n, Vector3 pos, Material m, Vector3? euler = null)
        {
            var go = new GameObject(n, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.position = pos;
            go.transform.eulerAngles = euler ?? Vector3.zero;
            go.GetComponent<MeshFilter>().sharedMesh = mesh;
            go.GetComponent<MeshRenderer>().sharedMaterial = m;
            return go;
        }

        public void Cam(Vector3 pos, Vector3 euler)
        {
            var cam = Camera.main;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            cam.transform.SetPositionAndRotation(pos, Quaternion.Euler(euler));
        }

        public void Bloom(float intensity, float threshold)
        {
            Camera.main.gameObject.AddComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
            var vp = ScriptableObject.CreateInstance<VolumeProfile>();
            AssetDatabase.CreateAsset(vp, $"{c.root}/BloomProfile.asset");
            var b = vp.Add<UnityEngine.Rendering.Universal.Bloom>(true);
            b.intensity.Override(intensity);
            b.threshold.Override(threshold);
            AssetDatabase.AddObjectToAsset(b, vp);
            EditorUtility.SetDirty(vp);
            var go = new GameObject("Global Volume");
            var v = go.AddComponent<Volume>();
            v.isGlobal = true;
            v.sharedProfile = vp;
        }
    }

    public void Scene(Action<Scn> build, Vector3 camPos, Vector3 camEuler)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        var s = new Scn(this);
        s.Cam(camPos, camEuler);
        s.Add(PrimitiveType.Plane, "Ground", Vector3.zero, Lit("Ground", new Color(0.35f, 0.35f, 0.37f), 0.1f), new Vector3(2f, 1f, 2f));
        build(s);
        EditorSceneManager.SaveScene(scene, $"{root}/{name}.unity");
        Shoot();
    }

    void Shoot()
    {
        var cam = Camera.main;
        var rt = new RenderTexture(768, 432, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        cam.targetTexture = rt;
        cam.Render();
        var prev = RenderTexture.active;
        RenderTexture.active = rt;
        var tex = new Texture2D(768, 432, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, 768, 432), 0, 0);
        tex.Apply();
        RenderTexture.active = prev;
        cam.targetTexture = null;
        rt.Release();
        Directory.CreateDirectory(ShotDir);
        File.WriteAllBytes($"{ShotDir}/{name}.png", tex.EncodeToPNG());
    }
}
