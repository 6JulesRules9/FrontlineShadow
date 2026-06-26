using System;

namespace FrontlineShadow.Save
{
    /// <summary>
    /// Wurzel des Spielstands. <see cref="Version"/> erlaubt schrittweise
    /// Migrationen, wenn das Schema in späteren Phasen wächst (Loadouts, Presets,
    /// Inventar, Reparatur-Timer ...). In Phase 0 bewusst minimal.
    ///
    /// Real-Time-Hinweis: Zeiten werden als UTC-ISO-8601-Strings gespeichert,
    /// damit Material-Cap & Reparatur das Schließen der App überstehen.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        /// <summary>Aktuelle Schema-Version. Bei Änderungen hochzählen + Migration ergänzen.</summary>
        public const int CurrentVersion = 1;

        public int Version = CurrentVersion;

        /// <summary>UTC-Zeitstempel der letzten Speicherung (ISO 8601, "o").</summary>
        public string LastSavedUtc = DateTime.UtcNow.ToString("o");
    }
}
