using System.IO;
using FrontlineShadow.Bootstrap;
using FrontlineShadow.Config;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrontlineShadow.EditorTools
{
    /// <summary>
    /// Einmaliges Phase-0-Setup per Menüklick — erspart das manuelle Anlegen von
    /// Szenen und das Verdrahten des Bootstraps.
    ///
    /// Menü: <b>Tools ▸ Frontline Shadow ▸ Setup Phase 0</b>.
    /// Legt an: BalanceConfig-Asset, Szenen Bootstrap + Garage, GameBootstrap
    /// (mit zugewiesener BalanceConfig) und die Build-Settings-Reihenfolge.
    /// Mehrfaches Ausführen ist sicher (legt nichts doppelt an).
    /// </summary>
    public static class Phase0Setup
    {
        const string ScenesDir = "Assets/_Project/Scenes";
        const string DataDir = "Assets/_Project/Data/Balance";
        const string BootstrapScenePath = ScenesDir + "/Bootstrap.unity";
        const string GarageScenePath = ScenesDir + "/Garage.unity";
        const string BalanceAssetPath = DataDir + "/BalanceConfig.asset";

        [MenuItem("Tools/Frontline Shadow/Setup Phase 0")]
        public static void Run()
        {
            EnsureFolder(ScenesDir);
            EnsureFolder(DataDir);

            var cfg = EnsureBalanceConfig();
            EnsureGarageScene();
            EnsureBootstrapScene(cfg);
            ConfigureBuildSettings();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Frontline Shadow — Phase 0",
                "Setup fertig:\n" +
                "• BalanceConfig-Asset\n" +
                "• Szenen Bootstrap + Garage\n" +
                "• GameBootstrap verdrahtet\n" +
                "• Build Settings gesetzt\n\n" +
                "Öffne Bootstrap.unity und drücke Play.\n" +
                "Danach: Test Runner ▸ EditMode ▸ Run All.",
                "OK");
        }

        static BalanceConfig EnsureBalanceConfig()
        {
            var existing = AssetDatabase.LoadAssetAtPath<BalanceConfig>(BalanceAssetPath);
            if (existing != null) return existing;

            var cfg = ScriptableObject.CreateInstance<BalanceConfig>();
            AssetDatabase.CreateAsset(cfg, BalanceAssetPath);
            return cfg;
        }

        static void EnsureGarageScene()
        {
            if (File.Exists(GarageScenePath)) return;

            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, GarageScenePath);
        }

        static void EnsureBootstrapScene(BalanceConfig cfg)
        {
            var scene = File.Exists(BootstrapScenePath)
                ? EditorSceneManager.OpenScene(BootstrapScenePath, OpenSceneMode.Single)
                : EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var go = GameObject.Find("Bootstrap") ?? new GameObject("Bootstrap");
            var boot = go.GetComponent<GameBootstrap>() ?? go.AddComponent<GameBootstrap>();

            // Privates [SerializeField] _balanceConfig sauber über SerializedObject setzen.
            var so = new SerializedObject(boot);
            var prop = so.FindProperty("_balanceConfig");
            if (prop != null)
            {
                prop.objectReferenceValue = cfg;
                so.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, BootstrapScenePath);
        }

        static void ConfigureBuildSettings()
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(BootstrapScenePath, true),
                new EditorBuildSettingsScene(GarageScenePath, true),
            };
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
