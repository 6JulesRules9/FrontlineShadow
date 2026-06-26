using UnityEngine;

namespace FrontlineShadow.Config
{
    /// <summary>
    /// Zentrale, tunebare Balance-Werte als ScriptableObject — Balancing ohne
    /// Code-Change (Prinzip "Daten vor Code"). In Phase 0 ein Skelett; die Felder
    /// wachsen mit den Systemen (Stats, Crafting, Real-Time-Meta ...).
    ///
    /// Anlegen via: Assets ▸ Create ▸ Frontline Shadow ▸ Balance Config.
    /// </summary>
    [CreateAssetMenu(menuName = "Frontline Shadow/Balance Config", fileName = "BalanceConfig")]
    public class BalanceConfig : ScriptableObject
    {
        [Header("Hunter")]
        [Tooltip("Anzahl Komponenten-Slots: Ketten, Core, Turm, Geschütz, Comms.")]
        public int ComponentSlots = 5;

        [Tooltip("Anzahl Preset-Slots für schnelle Loadout-Wechsel vor dem Gefecht.")]
        public int PresetSlots = 3;

        [Header("Real-Time Meta")]
        [Tooltip("Maximal pro Tag sammelbare Material-Einheiten (Soft-Cap).")]
        public int DailyMaterialCap = 1000;

        [Tooltip("Reparaturzeit in Sekunden bei 100 % Gesamtbeschädigung des Hunters.")]
        public float FullRepairSeconds = 900f; // 15 Min Startwert (später skalierend)

        [Tooltip("Deckel der Reparaturzeit in Sekunden (z. B. 24 h = 86400).")]
        public float MaxRepairSeconds = 86400f;

        void OnValidate()
        {
            ComponentSlots = Mathf.Max(1, ComponentSlots);
            PresetSlots = Mathf.Max(1, PresetSlots);
            DailyMaterialCap = Mathf.Max(0, DailyMaterialCap);
            FullRepairSeconds = Mathf.Max(0f, FullRepairSeconds);
            MaxRepairSeconds = Mathf.Max(FullRepairSeconds, MaxRepairSeconds);
        }
    }
}
