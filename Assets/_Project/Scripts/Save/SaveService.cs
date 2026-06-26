using System;
using System.IO;
using FrontlineShadow.Core;
using UnityEngine;

namespace FrontlineShadow.Save
{
    /// <summary>
    /// Lädt/speichert den Spielstand als JSON. Versioniert, mit Migrations-Hook.
    /// Phase 0 nutzt Unity-<see cref="JsonUtility"/> (kein Extra-Package). Sobald
    /// der Save-Graph komplex wird (Dictionaries, Polymorphie für Bauteil-
    /// Instanzen), wechseln wir auf Newtonsoft (com.unity.nuget.newtonsoft-json).
    /// </summary>
    public class SaveService : IService
    {
        const string FileName = "frontline_shadow.save.json";

        readonly string _path;

        public SaveData Data { get; private set; }

        /// <param name="directory">
        /// Zielordner. Standard: <see cref="Application.persistentDataPath"/>.
        /// In Tests kann ein Temp-Ordner übergeben werden.
        /// </param>
        public SaveService(string directory = null)
        {
            var dir = string.IsNullOrEmpty(directory) ? Application.persistentDataPath : directory;
            _path = Path.Combine(dir, FileName);
        }

        /// <summary>Lädt den Spielstand oder erzeugt einen neuen, falls keiner/defekt.</summary>
        public SaveData LoadOrCreate()
        {
            if (File.Exists(_path))
            {
                try
                {
                    var json = File.ReadAllText(_path);
                    Data = Deserialize(json) ?? new SaveData();
                    Migrate(Data);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Save] Laden fehlgeschlagen, starte neuen Spielstand. {e.Message}");
                    Data = new SaveData();
                }
            }
            else
            {
                Data = new SaveData();
            }

            return Data;
        }

        /// <summary>Schreibt den aktuellen Spielstand und aktualisiert den Zeitstempel.</summary>
        public void Save()
        {
            Data ??= new SaveData();
            Data.LastSavedUtc = DateTime.UtcNow.ToString("o");
            File.WriteAllText(_path, Serialize(Data));
        }

        // ---- Reine Funktionen (auch von Tests genutzt) ----

        public static string Serialize(SaveData data) => JsonUtility.ToJson(data, true);

        public static SaveData Deserialize(string json) => JsonUtility.FromJson<SaveData>(json);

        static void Migrate(SaveData data)
        {
            if (data.Version >= SaveData.CurrentVersion) return;

            // Künftige Migrationen Schritt für Schritt hier ergänzen:
            // if (data.Version == 1) { ...; data.Version = 2; }

            data.Version = SaveData.CurrentVersion;
        }
    }
}
