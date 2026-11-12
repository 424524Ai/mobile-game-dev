// Put this in Assets/Editor/ (create the folder). Import the student_*.obj files and student_palette.png first.
// Then run: Tools > Lowpoly > Build Student Character
// It builds a prefab (StudentCharacter) with Run, Jump and Roll animation clips and an Animator Controller.
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

public static class StudentCharacterBuilder
{
    struct Pose
    {
        public float t, hipsY, hipsX, spine, head, sh, el, th, kn;
        public Pose(float t, float hipsY, float hipsX, float spine, float head, float sh, float el, float th, float kn)
        { this.t = t; this.hipsY = hipsY; this.hipsX = hipsX; this.spine = spine; this.head = head; this.sh = sh; this.el = el; this.th = th; this.kn = kn; }
    }

    [MenuItem("Tools/Lowpoly/Build Student Character")]
    static void Build()
    {
        string hipsPath = FindObj("student_hips");
        string texGuid = AssetDatabase.FindAssets("student_palette t:Texture2D").FirstOrDefault();
        if (hipsPath == null || texGuid == null)
        {
            EditorUtility.DisplayDialog("Student character", "Import the student_*.obj files and student_palette.png into the project first.", "OK");
            return;
        }
        string dir = Path.GetDirectoryName(hipsPath).Replace('\\', '/');
        string outDir = dir + "/Generated";
        if (!AssetDatabase.IsValidFolder(outDir)) AssetDatabase.CreateFolder(dir, "Generated");

        // palette texture + material (works with Built-in, URP and HDRP)
        string texPath = AssetDatabase.GUIDToAssetPath(texGuid);
        var ti = (TextureImporter)AssetImporter.GetAtPath(texPath);
        ti.filterMode = FilterMode.Point; ti.mipmapEnabled = false; ti.wrapMode = TextureWrapMode.Clamp;
        ti.textureCompression = TextureImporterCompression.Uncompressed; ti.SaveAndReimport();
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
        var rp = GraphicsSettings.currentRenderPipeline;
        Shader sh = (rp != null && rp.defaultShader != null) ? rp.defaultShader : Shader.Find("Standard");
        string matPath = outDir + "/StudentPalette.mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (mat == null) { mat = new Material(sh); AssetDatabase.CreateAsset(mat, matPath); } else mat.shader = sh;
        foreach (string p in new[] { "_BaseMap", "_BaseColorMap", "_MainTex" }) if (mat.HasProperty(p)) mat.SetTexture(p, tex);
        foreach (string p in new[] { "_BaseColor", "_Color" }) if (mat.HasProperty(p)) mat.SetColor(p, Color.white);
        foreach (string p in new[] { "_Smoothness", "_Glossiness" }) if (mat.HasProperty(p)) mat.SetFloat(p, 0f);
        EditorUtility.SetDirty(mat);

        // hierarchy (joint offsets match make_student.py)
        var root = new GameObject("StudentCharacter");
        var hips = Joint(root.transform, "Hips", new Vector3(0, 0.88f, 0), "student_hips", mat);
        var spine = Joint(hips, "Spine", new Vector3(0, 0.07f, 0), "student_torso", mat);
        Joint(spine, "Head", new Vector3(0, 0.43f, 0), "student_head", mat);
        foreach (string s in new[] { "L", "R" })
        {
            float sx = s == "L" ? -1f : 1f;
            var shoulder = Joint(spine, "Shoulder" + s, new Vector3(sx * 0.27f, 0.37f, 0), "student_arm_upper", mat);
            Joint(shoulder, "Elbow" + s, new Vector3(0, -0.28f, 0), "student_arm_lower", mat);
            var thigh = Joint(hips, "Thigh" + s, new Vector3(sx * 0.11f, -0.04f, 0), "student_leg_upper", mat);
            Joint(thigh, "Knee" + s, new Vector3(0, -0.42f, 0), "student_leg_lower", mat);
        }

        // clips
        var run = RunClip(); var jump = JumpClip(); var roll = RollClip();
        Save(run, outDir + "/Run.anim"); Save(jump, outDir + "/Jump.anim"); Save(roll, outDir + "/Roll.anim");
        var runS = AnimationUtility.GetAnimationClipSettings(run); runS.loopTime = true; AnimationUtility.SetAnimationClipSettings(run, runS);

        // controller: Run (default) -> Jump / Roll via triggers, back to Run when finished
        string cp = outDir + "/StudentAnimator.controller";
        AssetDatabase.DeleteAsset(cp);
        var ctrl = AnimatorController.CreateAnimatorControllerAtPath(cp);
        ctrl.AddParameter("Jump", AnimatorControllerParameterType.Trigger);
        ctrl.AddParameter("Roll", AnimatorControllerParameterType.Trigger);
        var sm = ctrl.layers[0].stateMachine;
        var sRun = sm.AddState("Run"); sRun.motion = run;
        var sJump = sm.AddState("Jump"); sJump.motion = jump;
        var sRoll = sm.AddState("Roll"); sRoll.motion = roll;
        sm.defaultState = sRun;
        var tr = sRun.AddTransition(sJump); tr.AddCondition(AnimatorConditionMode.If, 0, "Jump"); tr.hasExitTime = false; tr.duration = 0.05f;
        tr = sRun.AddTransition(sRoll); tr.AddCondition(AnimatorConditionMode.If, 0, "Roll"); tr.hasExitTime = false; tr.duration = 0.05f;
        tr = sJump.AddTransition(sRun); tr.hasExitTime = true; tr.exitTime = 0.95f; tr.duration = 0.1f;
        tr = sRoll.AddTransition(sRun); tr.hasExitTime = true; tr.exitTime = 0.95f; tr.duration = 0.1f;

        root.AddComponent<Animator>().runtimeAnimatorController = ctrl;
        string prefabPath = outDir + "/StudentCharacter.prefab";
        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        UnityEngine.Object.DestroyImmediate(root);
        AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        Debug.Log("Student character built: " + prefabPath + " (animator triggers: Jump, Roll)");
    }

    // ---------- helpers ----------
    static string FindObj(string name)
    {
        return AssetDatabase.FindAssets(name + " t:Model").Select(AssetDatabase.GUIDToAssetPath)
            .FirstOrDefault(p => p.EndsWith(".obj") && Path.GetFileNameWithoutExtension(p) == name);
    }
    static Transform Joint(Transform parent, string name, Vector3 pos, string meshName, Material mat)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        var mesh = AssetDatabase.LoadAllAssetsAtPath(FindObj(meshName)).OfType<Mesh>().FirstOrDefault();
        go.AddComponent<MeshFilter>().sharedMesh = mesh;
        go.AddComponent<MeshRenderer>().sharedMaterial = mat;
        return go.transform;
    }
    static void Save(AnimationClip clip, string path) { AssetDatabase.DeleteAsset(path); AssetDatabase.CreateAsset(clip, path); }

    static void Curve3(AnimationClip c, string path, float dur, int n, Func<float, Vector3> f, bool position)
    {
        var cx = new AnimationCurve(); var cy = new AnimationCurve(); var cz = new AnimationCurve();
        for (int i = 0; i <= n; i++)
        {
            float p = (float)i / n; Vector3 v = f(p);
            cx.AddKey(p * dur, v.x); cy.AddKey(p * dur, v.y); cz.AddKey(p * dur, v.z);
        }
        foreach (var cv in new[] { cx, cy, cz }) for (int i = 0; i < cv.length; i++) cv.SmoothTangents(i, 0f);
        string prop = position ? "localPosition" : "localEulerAnglesRaw";
        c.SetCurve(path, typeof(Transform), prop + ".x", cx);
        c.SetCurve(path, typeof(Transform), prop + ".y", cy);
        c.SetCurve(path, typeof(Transform), prop + ".z", cz);
    }

    static AnimationClip RunClip()
    {
        var c = new AnimationClip { name = "Run", frameRate = 30 };
        const float D = 0.6f; const int N = 24; const float TAU = Mathf.PI * 2f;
        Func<float, float> S = p => Mathf.Sin(p * TAU);
        Func<float, float> Knee = p => 15f + 60f * (0.5f + 0.5f * Mathf.Cos(p * TAU));
        Curve3(c, "Hips", D, N, p => new Vector3(0, 0.85f + 0.03f * Mathf.Cos(p * 2f * TAU), 0), true);
        Curve3(c, "Hips", D, N, p => new Vector3(0, -8f * S(p), 0), false);
        Curve3(c, "Hips/Spine", D, N, p => new Vector3(12f, 8f * S(p), 0), false);
        Curve3(c, "Hips/Spine/Head", D, N, p => new Vector3(-10f, 0, 0), false);
        Curve3(c, "Hips/Spine/ShoulderL", D, N, p => new Vector3(40f * S(p), 0, 0), false);
        Curve3(c, "Hips/Spine/ShoulderR", D, N, p => new Vector3(-40f * S(p), 0, 0), false);
        Curve3(c, "Hips/Spine/ShoulderL/ElbowL", D, N, p => new Vector3(-70f, 0, 0), false);
        Curve3(c, "Hips/Spine/ShoulderR/ElbowR", D, N, p => new Vector3(-70f, 0, 0), false);
        Curve3(c, "Hips/ThighL", D, N, p => new Vector3(-45f * S(p), 0, 0), false);
        Curve3(c, "Hips/ThighR", D, N, p => new Vector3(45f * S(p), 0, 0), false);
        Curve3(c, "Hips/ThighL/KneeL", D, N, p => new Vector3(Knee(p), 0, 0), false);
        Curve3(c, "Hips/ThighR/KneeR", D, N, p => new Vector3(Knee(p + 0.5f), 0, 0), false);
        return c;
    }

    static AnimationClip JumpClip()
    {
        var k = new[]
        {   //            t     hipsY hipsX spine head   sh     el    th    kn
            new Pose(0.00f, 0.84f, 0, 12, -10,    0,  -70,  -10,  20),   // running pose
            new Pose(0.12f, 0.62f, 0, 30, -20,   40,  -40,  -80, 110),   // crouch
            new Pose(0.24f, 0.95f, 0, -5,   0, -150,  -20,   10,   5),   // takeoff, arms up
            new Pose(0.45f, 0.95f, 0, 10,  -5, -120,  -60,  -70,  80),   // tuck in the air
            new Pose(0.68f, 0.95f, 0, 15, -10,  -60,  -50,  -50,  40),   // reach for landing
            new Pose(0.78f, 0.68f, 0, 28, -15,   30,  -60,  -75, 100),   // land and absorb
            new Pose(0.90f, 0.84f, 0, 12, -10,    0,  -70,  -10,  20),   // back to run
        };
        return Sym("Jump", k);
    }

    static AnimationClip RollClip()
    {
        var k = new[]
        {   //            t     hipsY hipsX spine head   sh     el    th    kn
            new Pose(0.00f, 0.84f,   0, 12, -10,    0,  -70,  -10,  20),
            new Pose(0.12f, 0.62f,  20, 40,  30,  -80, -100,  -80, 120),  // duck
            new Pose(0.20f, 0.55f,  60, 55,  40,  -70, -110, -100, 130),  // tucked
            new Pose(0.45f, 0.55f, 200, 55,  40,  -70, -110, -100, 130),  // rolling
            new Pose(0.62f, 0.55f, 330, 55,  40,  -70, -110, -100, 130),
            new Pose(0.72f, 0.70f, 360, 30,   0,  -20,  -80,  -50,  70),  // stand up
            new Pose(0.80f, 0.84f, 360, 12, -10,    0,  -70,  -10,  20),
        };
        return Sym("Roll", k);
    }

    static AnimationClip Sym(string name, Pose[] ps)
    {
        var c = new AnimationClip { name = name, frameRate = 30 };
        Action<string, string, Func<Pose, float>> add = (path, prop, f) =>
        {
            var cv = new AnimationCurve();
            foreach (var p in ps) cv.AddKey(p.t, f(p));
            for (int i = 0; i < cv.length; i++) cv.SmoothTangents(i, 0f);
            c.SetCurve(path, typeof(Transform), prop, cv);
        };
        add("Hips", "localPosition.y", p => p.hipsY);
        add("Hips", "localEulerAnglesRaw.x", p => p.hipsX);
        add("Hips/Spine", "localEulerAnglesRaw.x", p => p.spine);
        add("Hips/Spine/Head", "localEulerAnglesRaw.x", p => p.head);
        foreach (string s in new[] { "L", "R" })
        {
            add("Hips/Spine/Shoulder" + s, "localEulerAnglesRaw.x", p => p.sh);
            add("Hips/Spine/Shoulder" + s + "/Elbow" + s, "localEulerAnglesRaw.x", p => p.el);
            add("Hips/Thigh" + s, "localEulerAnglesRaw.x", p => p.th);
            add("Hips/Thigh" + s + "/Knee" + s, "localEulerAnglesRaw.x", p => p.kn);
        }
        return c;
    }
}
