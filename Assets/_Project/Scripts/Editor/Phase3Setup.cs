using System.IO;
using FrontlineShadow.Combat;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrontlineShadow.EditorTools
{
    /// <summary>
    /// Phase-3-Setup-Häppchen "Schadensmodell" (04_ROADMAP.md Phase 3): legt ein
    /// Ziel-Dummy-Prefab (<see cref="Damageable"/>) an und stellt ein paar Dummies
    /// mit unterschiedlicher Panzerung in die Battle-Szene — zum Testen von
    /// Durchschlag vs. Panzerung. Mehrfaches Ausführen ist sicher.
    ///
    /// Menü: <b>Tools ▸ Frontline Shadow ▸ Setup Phase 3 (Schadensmodell)</b>.
    /// </summary>
    public static class Phase3Setup
    {
        const string PrefabsDir = "Assets/_Project/Prefabs";
        const string TargetPrefabPath = "Assets/_Project/Prefabs/TargetDummy.prefab";
        const string BattleScenePath = "Assets/_Project/Scenes/Battle.unity";
        const string TargetsRootName = "Targets";

        // Position, Panzerung — gestaffelt, damit der Durchschlags-Trade-off sichtbar wird.
        static readonly (Vector3 pos, float armor)[] Dummies =
        {
            (new Vector3(-6f, 0.5f, 14f), 60f),
            (new Vector3(0f, 0.5f, 16f), 140f),
            (new Vector3(6f, 0.5f, 14f), 240f),
        };

        [MenuItem("Tools/Frontline Shadow/Setup Phase 3 (Schadensmodell)")]
        public static void Run()
        {
            EnsureFolder(PrefabsDir);

            var prefab = EnsureTargetPrefab();
            PopulateBattleScene(prefab);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Frontline Shadow — Phase 3 (Schadensmodell)",
                "Setup fertig:\n" +
                "• TargetDummy-Prefab (Damageable)\n" +
                "• 3 Ziele mit gestaffelter Panzerung (60 / 140 / 240) in der Battle-Szene\n\n" +
                "Play, ins Gefecht, draufhalten: schwächere Panzerung nimmt vollen\n" +
                "Schaden, stärkere prallt ab (abhängig vom Penetration-Stat).",
                "OK");
        }

        static GameObject EnsureTargetPrefab()
        {
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(TargetPrefabPath);
            if (existing != null) return existing;

            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "TargetDummy";
            go.transform.localScale = new Vector3(1.5f, 1f, 1.5f);
            go.AddComponent<Damageable>();

            var prefab = PrefabUtility.SaveAsPrefabAsset(go, TargetPrefabPath);
            Object.DestroyImmediate(go);
            return prefab;
        }

        static void PopulateBattleScene(GameObject prefab)
        {
            if (!File.Exists(BattleScenePath))
            {
                Debug.LogWarning("[Phase3] Battle-Szene fehlt — zuerst Setup Phase 2 ausführen.");
                return;
            }

            var scene = EditorSceneManager.OpenScene(BattleScenePath, OpenSceneMode.Single);

            var root = GameObject.Find(TargetsRootName) ?? new GameObject(TargetsRootName);

            for (var i = 0; i < Dummies.Length; i++)
            {
                var name = $"TargetDummy_{i}";
                if (FindChild(root.transform, name) != null) continue;

                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.name = name;
                instance.transform.SetParent(root.transform);
                instance.transform.position = Dummies[i].pos;

                var so = new SerializedObject(instance.GetComponent<Damageable>());
                so.FindProperty("_armor").floatValue = Dummies[i].armor;
                so.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, BattleScenePath);
        }

        static Transform FindChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
                if (child.name == name) return child;
            return null;
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
