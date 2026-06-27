using System.Collections.Generic;
using System.IO;
using FrontlineShadow.Bootstrap;
using FrontlineShadow.Stats;
using FrontlineShadow.Tank;
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
    /// Einmaliges Phase-1-Setup per Menüklick (04_ROADMAP.md Phase 1): legt
    /// Platzhalter-Bauteile + Katalog an und baut die Garage-Szene mit dem
    /// Hunter-UI auf. Mehrfaches Ausführen ist sicher — bereits vorhandene
    /// Assets/Inspector-Werte werden nicht überschrieben.
    ///
    /// Menü: <b>Tools ▸ Frontline Shadow ▸ Setup Phase 1</b>.
    /// </summary>
    public static class Phase1Setup
    {
        const string ComponentsDir = "Assets/_Project/Data/Components";
        const string CatalogPath = "Assets/_Project/Data/ComponentCatalog.asset";
        const string PanelSettingsPath = "Assets/_Project/Data/GaragePanelSettings.asset";
        const string GarageScenePath = "Assets/_Project/Scenes/Garage.unity";
        const string BootstrapScenePath = "Assets/_Project/Scenes/Bootstrap.unity";

        [MenuItem("Tools/Frontline Shadow/Setup Phase 1")]
        public static void Run()
        {
            EnsureFolder(ComponentsDir);

            var components = EnsureComponents();
            MigrateStats(components);
            var catalog = EnsureCatalog(components);
            EnsureGarageUi(catalog);
            WireBootstrapCatalog(catalog);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Frontline Shadow — Phase 1",
                "Setup fertig:\n" +
                "• Platzhalter-Bauteile (2 je Slot)\n" +
                "• ComponentCatalog-Asset\n" +
                "• Garage-Szene mit Hunter-UI\n" +
                "• Bootstrap-Katalog verdrahtet\n\n" +
                "Öffne Bootstrap.unity und drücke Play.",
                "OK");
        }

        static List<TankComponentDef> EnsureComponents()
        {
            return new List<TankComponentDef>
            {
                EnsureComponent("tracks.light", ComponentSlot.Tracks, "tracks.light",
                    Mod(StatType.TopSpeed, ModOp.Flat, 65),
                    Mod(StatType.HullTraverse, ModOp.Flat, 42),
                    Mod(StatType.Weight, ModOp.Flat, 120)),
                EnsureComponent("tracks.heavy", ComponentSlot.Tracks, "tracks.heavy",
                    Mod(StatType.TopSpeed, ModOp.Flat, 45),
                    Mod(StatType.HullTraverse, ModOp.Flat, 28),
                    Mod(StatType.Hp, ModOp.Flat, 50),
                    Mod(StatType.Weight, ModOp.Flat, 260)),

                EnsureComponent("core.standard", ComponentSlot.Core, "core.standard",
                    Mod(StatType.Accel, ModOp.Flat, 8),
                    Mod(StatType.Hp, ModOp.Flat, 400),
                    Mod(StatType.Weight, ModOp.Flat, 150)),
                EnsureComponent("core.power", ComponentSlot.Core, "core.power",
                    Mod(StatType.Accel, ModOp.Flat, 11),
                    Mod(StatType.Hp, ModOp.Flat, 420),
                    Mod(StatType.TopSpeed, ModOp.PercentAdd, 0.08f),
                    Mod(StatType.Weight, ModOp.Flat, 260)),

                EnsureComponent("turret.light", ComponentSlot.Turret, "turret.light",
                    Mod(StatType.ArmorFront, ModOp.Flat, 40),
                    Mod(StatType.Hp, ModOp.Flat, 150),
                    Mod(StatType.TurretTraverse, ModOp.Flat, 42),
                    Mod(StatType.Weight, ModOp.Flat, 100)),
                EnsureComponent("turret.heavy", ComponentSlot.Turret, "turret.heavy",
                    Mod(StatType.ArmorFront, ModOp.Flat, 90),
                    Mod(StatType.Hp, ModOp.Flat, 220),
                    Mod(StatType.TurretTraverse, ModOp.Flat, 26),
                    Mod(StatType.Weight, ModOp.Flat, 260)),

                EnsureComponent("gun.standard", ComponentSlot.Gun, "gun.standard",
                    Mod(StatType.AlphaDamage, ModOp.Flat, 240),
                    Mod(StatType.Penetration, ModOp.Flat, 180),
                    Mod(StatType.ReloadTime, ModOp.Flat, 6.5f),
                    Mod(StatType.Dispersion, ModOp.Flat, 0.38f),
                    Mod(StatType.AimTime, ModOp.Flat, 2.3f),
                    Mod(StatType.Weight, ModOp.Flat, 120)),
                EnsureComponent("gun.sniper", ComponentSlot.Gun, "gun.sniper",
                    Mod(StatType.AlphaDamage, ModOp.Flat, 390),
                    Mod(StatType.Penetration, ModOp.Flat, 240),
                    Mod(StatType.Penetration, ModOp.PercentMult, 0.1f),
                    Mod(StatType.ReloadTime, ModOp.Flat, 9.8f),
                    Mod(StatType.Dispersion, ModOp.Flat, 0.30f),
                    Mod(StatType.AimTime, ModOp.Flat, 2.9f),
                    Mod(StatType.Weight, ModOp.Flat, 260)),

                EnsureComponent("comms.standard", ComponentSlot.Comms, "comms.standard",
                    Mod(StatType.ViewRange, ModOp.Flat, 380),
                    Mod(StatType.SpottingSpeed, ModOp.Flat, 1.0f),
                    Mod(StatType.Camo, ModOp.Flat, 0.15f),
                    Mod(StatType.Weight, ModOp.Flat, 40)),
                EnsureComponent("comms.recon", ComponentSlot.Comms, "comms.recon",
                    Mod(StatType.ViewRange, ModOp.Flat, 420),
                    Mod(StatType.SpottingSpeed, ModOp.Flat, 1.3f),
                    Mod(StatType.SpottingPersistence, ModOp.Flat, 2.0f),
                    Mod(StatType.Camo, ModOp.Flat, 0.10f),
                    Mod(StatType.Weight, ModOp.Flat, 100)),
            };
        }

        static StatModifier Mod(StatType type, ModOp op, float value) => new(type, op, value);

        /// <summary>
        /// Ergänzt Stats auf bereits existierenden Assets (die EnsureComponent
        /// nicht überschreibt). Aktuell: TurretTraverse für die Türme nachrüsten,
        /// falls sie vor der Einführung des Stats erstellt wurden.
        /// </summary>
        static void MigrateStats(List<TankComponentDef> components)
        {
            EnsureStat(Find(components, "turret.light"), StatType.TurretTraverse, ModOp.Flat, 42);
            EnsureStat(Find(components, "turret.heavy"), StatType.TurretTraverse, ModOp.Flat, 26);
        }

        static TankComponentDef Find(List<TankComponentDef> components, string id)
            => components.Find(c => c != null && c.Id == id);

        static void EnsureStat(TankComponentDef def, StatType type, ModOp op, float value)
        {
            if (def == null) return;
            if (def.Stats.Exists(m => m.Type == type)) return;

            def.Stats.Add(new StatModifier(type, op, value));
            EditorUtility.SetDirty(def);
        }

        static TankComponentDef EnsureComponent(string id, ComponentSlot slot, string pathId, params StatModifier[] stats)
        {
            var path = $"{ComponentsDir}/{id}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<TankComponentDef>(path);
            if (existing != null) return existing;

            var def = ScriptableObject.CreateInstance<TankComponentDef>();
            def.Id = id;
            def.Slot = slot;
            def.Tier = 1;
            def.PathId = pathId;
            def.Stats = new List<StatModifier>(stats);
            AssetDatabase.CreateAsset(def, path);
            return def;
        }

        static ComponentCatalog EnsureCatalog(List<TankComponentDef> components)
        {
            var catalog = AssetDatabase.LoadAssetAtPath<ComponentCatalog>(CatalogPath);
            if (catalog == null)
            {
                catalog = ScriptableObject.CreateInstance<ComponentCatalog>();
                AssetDatabase.CreateAsset(catalog, CatalogPath);
            }

            foreach (var component in components)
            {
                if (!catalog.Components.Contains(component))
                    catalog.Components.Add(component);
            }

            EditorUtility.SetDirty(catalog);
            return catalog;
        }

        static PanelSettings EnsurePanelSettings()
        {
            var existing = AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            if (existing != null) return existing;

            var settings = ScriptableObject.CreateInstance<PanelSettings>();
            settings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            settings.referenceResolution = new Vector2Int(1920, 1080);
            AssetDatabase.CreateAsset(settings, PanelSettingsPath);
            return settings;
        }

        static void EnsureGarageUi(ComponentCatalog catalog)
        {
            var panelSettings = EnsurePanelSettings();

            var scene = File.Exists(GarageScenePath)
                ? EditorSceneManager.OpenScene(GarageScenePath, OpenSceneMode.Single)
                : EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var uiGo = GameObject.Find("GarageUI") ?? new GameObject("GarageUI");
            var document = uiGo.GetComponent<UIDocument>() ?? uiGo.AddComponent<UIDocument>();
            document.panelSettings = panelSettings;
            if (uiGo.GetComponent<GarageUIController>() == null)
                uiGo.AddComponent<GarageUIController>();

            if (Object.FindAnyObjectByType<EventSystem>() == null)
            {
                var eventSystemGo = new GameObject("EventSystem");
                eventSystemGo.AddComponent<EventSystem>();
                eventSystemGo.AddComponent<InputSystemUIInputModule>();
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, GarageScenePath);
        }

        static void WireBootstrapCatalog(ComponentCatalog catalog)
        {
            var scene = File.Exists(BootstrapScenePath)
                ? EditorSceneManager.OpenScene(BootstrapScenePath, OpenSceneMode.Single)
                : EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var go = GameObject.Find("Bootstrap") ?? new GameObject("Bootstrap");
            var boot = go.GetComponent<GameBootstrap>() ?? go.AddComponent<GameBootstrap>();

            var so = new SerializedObject(boot);
            var prop = so.FindProperty("_componentCatalog");
            if (prop != null && prop.objectReferenceValue == null)
            {
                prop.objectReferenceValue = catalog;
                so.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, BootstrapScenePath);
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
