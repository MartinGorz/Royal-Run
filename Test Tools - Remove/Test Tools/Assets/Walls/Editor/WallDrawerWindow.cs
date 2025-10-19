using UnityEditor;
using UnityEngine;

public class WallDrawerWindow : EditorWindow
{
    // ... متغیرهای UI و وضعیت (بدون تغییر) ...
    private GameObject wallPrefab;
    private Vector3 wallScale = Vector3.one;
    private float planeHeight = 0f;

    private bool isToolActive = false;
    private Vector3 lastWallPosition = Vector3.zero;

    // متغیر جدید برای نگهداری آبجکت والد (گروه) در طول عملیات درگ
    private GameObject currentWallGroup = null;

    private Plane placementPlane = new Plane(Vector3.up, 0f);

    // ------------------- بخش 1: منو و رابط کاربری (UI) -------------------

    [MenuItem("Tools/Wall Drawer")]
    private static void ShowWindow()
    {
        WallDrawerWindow window = GetWindow<WallDrawerWindow>();
        window.titleContent = new GUIContent("Wall Drawer (Grouping)");
        window.minSize = new Vector2(300, 250);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Wall Settings", EditorStyles.boldLabel);
        wallPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Wall Prefab", wallPrefab, typeof(GameObject), false
        );

        wallScale = EditorGUILayout.Vector3Field("Wall Scale", wallScale);

        EditorGUILayout.Space();

        planeHeight = EditorGUILayout.FloatField("Placement Height (Y)", planeHeight);
        placementPlane = new Plane(Vector3.up, planeHeight);

        EditorGUILayout.Space();

        string buttonText = isToolActive ? "Deactivate Tool" : "Activate Tool (Scene View)";
        GUI.backgroundColor = isToolActive ? Color.red : Color.green;

        if (GUILayout.Button(buttonText))
        {
            ToggleToolActivation();
        }
        GUI.backgroundColor = Color.white;

        if (isToolActive)
        {
            EditorGUILayout.HelpBox("Tool is active. New walls will be grouped together upon mouse release.", MessageType.Info);
        }
    }

    private void ToggleToolActivation()
    {
        if (!isToolActive)
        {
            if (wallPrefab == null)
            {
                Debug.LogError("Wall Prefab is not set. Please assign a Prefab.");
                return;
            }
            SceneView.duringSceneGui += DrawWallOnSceneGUI;
            isToolActive = true;
            Debug.Log("Wall Drawer Tool Activated.");
        }
        else
        {
            SceneView.duringSceneGui -= DrawWallOnSceneGUI;
            isToolActive = false;
            // مطمئن می‌شویم که گروه‌بندی در صورت غیرفعال شدن ناگهانی هم پایان یابد
            currentWallGroup = null;
            Debug.Log("Wall Drawer Tool Deactivated.");
            Repaint();
        }
    }

    // ------------------- بخش 4: منطق گروه‌بندی و دیوارکشی در Scene View -------------------

    private void DrawWallOnSceneGUI(SceneView sceneView)
    {
        Event currentEvent = Event.current;

        // لازم است این رویداد را به عنوان کنترل پیش‌فرض ثبت کنیم.
        HandleUtility.AddDefaultControl(0);

        // **مرحله A: مدیریت رها کردن موس (MouseUp) - پایان گروه‌بندی**
        if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
        {
            currentWallGroup = null;
            lastWallPosition = Vector3.zero;
            currentEvent.Use();
            return; // از ادامه اجرا جلوگیری می‌کنیم
        }

        // **مرحله B: مدیریت فشردن موس (MouseDown) - شروع گروه‌بندی**
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
        {
            // ایجاد یک آبجکت خالی به عنوان والد جدید برای این عملیات درگ
            currentWallGroup = new GameObject("Wall Group - " + System.DateTime.Now.ToString("HH:mm:ss"));
            // ثبت گروه برای قابلیت Undo
            Undo.RegisterCreatedObjectUndo(currentWallGroup, "Start Wall Group");
            // مصرف رویداد
            currentEvent.Use();
        }

        // **مرحله C: مدیریت فشردن و کشیدن موس (MouseDown/MouseDrag) - ایجاد دیوارها**
        if (currentEvent.button == 0 && (currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag))
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            float distance;

            if (placementPlane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                float wallDistance = Vector3.Distance(hitPoint, lastWallPosition);

                if (currentEvent.type == EventType.MouseDown || wallDistance >= wallScale.x)
                {
                    // === عملیات اصلی ساخت و ثبت دیوار ===
                    GameObject newWall = (GameObject)PrefabUtility.InstantiatePrefab(wallPrefab);

                    newWall.transform.position = hitPoint;
                    newWall.transform.localScale = wallScale;

                    // تنظیم Parent: دیوار را فرزند گروه فعلی می‌کنیم
                    if (currentWallGroup != null)
                    {
                        newWall.transform.SetParent(currentWallGroup.transform);
                    }

                    // ثبت عملیات ساخت دیوار
                    Undo.RegisterCreatedObjectUndo(newWall, "Draw Wall Segment");

                    lastWallPosition = hitPoint;
                    EditorUtility.SetDirty(newWall);
                }
            }

            // مصرف رویداد (حتی اگر ساخت دیوار موفق نباشد)
            currentEvent.Use();
        }

        sceneView.Repaint();
    }
}





























//using System;
//using UnityEditor;
//using UnityEngine;

//public class WallDrawerWindow : EditorWindow
//{
//    [SerializeField] GameObject wallPrefab; 
//    [SerializeField] Vector3 wallScale = Vector3.one;
//    private bool isToolActive = false;


//    [MenuItem("Tools/Wall Drawer")]
//        private static void ShowWindow()
//    {
//        WallDrawerWindow window = GetWindow<WallDrawerWindow>();

//        window.titleContent = new GUIContent("Wall Drawer");
//    }

//    private void OnGUI()
//    {
//        //For the title
//        EditorGUILayout.LabelField("Wall Settings", EditorStyles.boldLabel);
//        wallPrefab = (GameObject)EditorGUILayout.ObjectField("Wall Prefab", wallPrefab, typeof(GameObject), false);
//        EditorGUILayout.Space();
//        wallScale = EditorGUILayout.Vector3Field("Wall Scale", wallScale);
//        EditorGUILayout.Space();

//        if (GUILayout.Button("Activate Wall Drawing Tool"))
//        {
//            ToggleToolActivation();
//        }
//    }

//    private void ToggleToolActivation()
//    {
//        if (!isToolActive)
//        {
//            SceneView.duringSceneGui += DrawWallOnSceneGUI;
//            isToolActive = true;

//        }
//        else
//        {
//            SceneView.duringSceneGui -= DrawWallOnSceneGUI;
//            isToolActive = false;
//        }
//    }

//    private void DrawWallOnSceneGUI(SceneView sceneView)
//    {

//    }
//}
