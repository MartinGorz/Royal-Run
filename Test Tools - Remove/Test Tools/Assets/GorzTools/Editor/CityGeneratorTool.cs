using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor.AI;  // for NavMeshBuilder
#endif

public class CityGeneratorTool : EditorWindow
{
    private CityGenSettings _settings;
    private Transform _parent;
    private CityGenerator _runtimeGen;

    [MenuItem("Gorz Tools/City Generator")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<CityGeneratorTool>("City Generator");
        wnd.minSize = new Vector2(360, 320);
    }

    private void OnEnable()
    {
        // Ensure a hidden runtime generator exists in scene
        var host = GameObject.Find("__GorzCityGenHost__");
        if (!host)
        {
            host = new GameObject("__GorzCityGenHost__");
            host.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInBuild;
            _runtimeGen = host.AddComponent<CityGenerator>();
        }
        else
        {
            _runtimeGen = host.GetComponent<CityGenerator>();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        _settings = (CityGenSettings)EditorGUILayout.ObjectField("Settings Asset", _settings, typeof(CityGenSettings), false);

        if (_settings == null)
        {
            EditorGUILayout.HelpBox("Create a CityGenSettings asset (Right Click → Create → Gorz → City Gen Settings).", MessageType.Info);
        }
        else
        {
            using (new EditorGUI.IndentLevelScope())
            {
                _settings.rows = EditorGUILayout.IntSlider("Rows", _settings.rows, 1, 50);
                _settings.cols = EditorGUILayout.IntSlider("Cols", _settings.cols, 1, 50);
                _settings.blockSize = EditorGUILayout.Slider("Block Size", _settings.blockSize, 5f, 200f);
                _settings.streetWidth = EditorGUILayout.Slider("Street Width", _settings.streetWidth, 0f, 20f);
                _settings.buildingsPerBlock = EditorGUILayout.IntSlider("Buildings / Block", _settings.buildingsPerBlock, 1, 32);
                _settings.buildingPrefab = (GameObject)EditorGUILayout.ObjectField("Building Prefab", _settings.buildingPrefab, typeof(GameObject), false);
                _settings.randomizeSeed = EditorGUILayout.Toggle("Randomize Seed", _settings.randomizeSeed);
                _settings.seed = EditorGUILayout.IntField("Seed", _settings.seed);
                _settings.autoBuildNavMesh = EditorGUILayout.Toggle("Auto-Build NavMesh", _settings.autoBuildNavMesh);

                if (GUI.changed)
                    EditorUtility.SetDirty(_settings);
            }
        }

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
        _parent = (Transform)EditorGUILayout.ObjectField("Parent (optional)", _parent, typeof(Transform), true);

        EditorGUILayout.Space(10);
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Generate", GUILayout.Height(32)))
                Generate();

            if (GUILayout.Button("Clear", GUILayout.Height(32)))
                Clear();
        }

        if (GUILayout.Button("Create Empty Parent", GUILayout.Height(24)))
        {
            var parentGO = new GameObject("CityRoot");
            Undo.RegisterCreatedObjectUndo(parentGO, "Create CityRoot");
            _parent = parentGO.transform;
        }
    }

    private void Generate()
    {
        if (_settings == null)
        {
            ShowNotification(new GUIContent("Assign Settings first."));
            return;
        }

        if (_runtimeGen == null)
        {
            Debug.LogError("Missing runtime generator host.");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(Selection.activeGameObject, "Generate City");
        _runtimeGen.Generate(_settings, _parent);

#if UNITY_EDITOR
        if (_settings.autoBuildNavMesh)
        {
            // Requires Navigation package in Project (Window → AI → Navigation (Components))
            try { NavMeshBuilder.BuildNavMesh(); }
            catch { Debug.LogWarning("NavMeshBuilder not available. Add Navigation components to project."); }
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
#endif
    }

    private void Clear()
    {
        if (_runtimeGen == null) return;
        _runtimeGen.Clear();
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
