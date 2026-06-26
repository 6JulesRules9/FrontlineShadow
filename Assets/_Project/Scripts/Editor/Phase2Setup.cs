using System.IO;
using FrontlineShadow.Combat;
using FrontlineShadow.Player;
using FrontlineShadow.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace FrontlineShadow.EditorTools
{
    /// <summary>
    /// Einmaliges Phase-2-Setup per Menüklick (04_ROADMAP.md Phase 2): legt
    /// Platzhalter-Hunter-Rig + Projektil-Prefab an und baut die Battle-Szene mit
    /// "Zurück zur Garage"-UI. Mehrfaches Ausführen ist sicher.
    ///
    /// Menü: <b>Tools ▸ Frontline Shadow ▸ Setup Phase 2</b>.
    /// </summary>
    public static class Phase2Setup
    {
        const string PrefabsDir = "Assets/_Project/Prefabs";
        const string HunterRigPrefabPath = "Assets/_Project/Prefabs/HunterRig.prefab";
        const string ProjectilePrefabPath = "Assets/_Project/Prefabs/Projectile.prefab";
        const string BattlePanelSettingsPath = "Assets/_Project/Data/BattlePanelSettings.asset";
        const string BattleScenePath = "Assets/_Project/Scenes/Battle.unity";

        [MenuItem("Tools/Frontline Shadow/Setup Phase 2")]
        public static void Run()
        {
            EnsureFolder(PrefabsDir);

            var projectile = EnsureProjectilePrefab();
            var hunterRig = EnsureHunterRigPrefab(projectile);
            EnsureBattleScene(hunterRig);
            EnsureBattleSceneInBuildSettings();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Frontline Shadow — Phase 2",
                "Setup fertig:\n" +
                "• Platzhalter-Hunter-Rig-Prefab (Hull/Turm/Mündung)\n" +
                "• Projektil-Prefab\n" +
                "• Battle-Szene mit Hunter-Rig + Zurück-zur-Garage-UI\n" +
                "• Battle.unity in Build Settings\n\n" +
                "Öffne Bootstrap.unity, drücke Play, dann \"Zum Gefecht\".",
                "OK");
        }

        static Projectile EnsureProjectilePrefab()
        {
            var existing = AssetDatabase.LoadAssetAtPath<Projectile>(ProjectilePrefabPath);
            if (existing != null) return existing;

            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Projectile";
            go.transform.localScale = Vector3.one * 0.3f;
            Object.DestroyImmediate(go.GetComponent<Collider>());
            go.AddComponent<Projectile>();

            var prefab = PrefabUtility.SaveAsPrefabAsset(go, ProjectilePrefabPath);
            Object.DestroyImmediate(go);
            return prefab.GetComponent<Projectile>();
        }

        static GameObject EnsureHunterRigPrefab(Projectile projectilePrefab)
        {
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(HunterRigPrefabPath);
            if (existing != null) return existing;

            var hull = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hull.name = "HunterRig";
            hull.transform.localScale = new Vector3(2f, 0.6f, 3f);

            var turret = GameObject.CreatePrimitive(PrimitiveType.Cube);
            turret.name = "Turret";
            turret.transform.SetParent(hull.transform);
            turret.transform.localScale = new Vector3(0.6f, 0.4f, 0.8f);
            turret.transform.localPosition = new Vector3(0f, 0.65f, 0f);

            var muzzle = new GameObject("Muzzle");
            muzzle.transform.SetParent(turret.transform);
            muzzle.transform.localPosition = new Vector3(0f, 0f, 1.2f);

            var controller = hull.AddComponent<PlayerController>();
            var so = new SerializedObject(controller);
            so.FindProperty("_turret").objectReferenceValue = turret.transform;
            so.FindProperty("_muzzle").objectReferenceValue = muzzle.transform;
            so.FindProperty("_projectilePrefab").objectReferenceValue = projectilePrefab;
            so.ApplyModifiedPropertiesWithoutUndo();

            var prefab = PrefabUtility.SaveAsPrefabAsset(hull, HunterRigPrefabPath);
            Object.DestroyImmediate(hull);
            return prefab;
        }

        static PanelSettings EnsureBattlePanelSettings()
        {
            var existing = AssetDatabase.LoadAssetAtPath<PanelSettings>(BattlePanelSettingsPath);
            if (existing != null) return existing;

            var settings = ScriptableObject.CreateInstance<PanelSettings>();
            settings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            settings.referenceResolution = new Vector2Int(1920, 1080);
            AssetDatabase.CreateAsset(settings, BattlePanelSettingsPath);
            return settings;
        }

        static void EnsureBattleScene(GameObject hunterRigPrefab)
        {
            var panelSettings = EnsureBattlePanelSettings();

            var scene = File.Exists(BattleScenePath)
                ? EditorSceneManager.OpenScene(BattleScenePath, OpenSceneMode.Single)
                : EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            if (GameObject.Find("Ground") == null)
            {
                var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
                ground.name = "Ground";
                ground.transform.localScale = Vector3.one * 10f;
            }

            var rigInstance = GameObject.Find("HunterRig");
            if (rigInstance == null)
                rigInstance = (GameObject)PrefabUtility.InstantiatePrefab(hunterRigPrefab);

            var mainCamera = Camera.main;
            if (mainCamera != null && mainCamera.GetComponent<HunterCameraFollow>() == null)
                mainCamera.gameObject.AddComponent<HunterCameraFollow>().SetTarget(rigInstance.transform);

            var battleUiGo = GameObject.Find("BattleUI") ?? new GameObject("BattleUI");
            var document = battleUiGo.GetComponent<UIDocument>() ?? battleUiGo.AddComponent<UIDocument>();
            document.panelSettings = panelSettings;
            if (battleUiGo.GetComponent<BattleUIController>() == null)
                battleUiGo.AddComponent<BattleUIController>();

            if (Object.FindAnyObjectByType<EventSystem>() == null)
            {
                var eventSystemGo = new GameObject("EventSystem");
                eventSystemGo.AddComponent<EventSystem>();
                eventSystemGo.AddComponent<InputSystemUIInputModule>();
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, BattleScenePath);
        }

        static void EnsureBattleSceneInBuildSettings()
        {
            var scenes = EditorBuildSettings.scenes;
            foreach (var s in scenes)
            {
                if (s.path == BattleScenePath) return;
            }

            var updated = new EditorBuildSettingsScene[scenes.Length + 1];
            scenes.CopyTo(updated, 0);
            updated[scenes.Length] = new EditorBuildSettingsScene(BattleScenePath, true);
            EditorBuildSettings.scenes = updated;
        }

        static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;

            var parent = Path.GetDirectoryName(path).Replace('\\', '/');
            var leaf = Path.GetFileName(path);
            if (!AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, leaf);
        }
    }
}
