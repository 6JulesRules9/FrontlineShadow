using System.Collections.Generic;
using FrontlineShadow.Stats;
using UnityEngine;

namespace FrontlineShadow.Tank
{
    /// <summary>
    /// Definition eines Bauteils (ein Slot, ein Tier, ein Pfad). Datengetrieben —
    /// neue Bauteile/Balancing entstehen als Assets, nicht als Code
    /// (03_TECH_ARCHITEKTUR.md §4.1).
    /// </summary>
    [CreateAssetMenu(menuName = "Frontline Shadow/Tank Component", fileName = "TankComponentDef")]
    public class TankComponentDef : ScriptableObject
    {
        [Tooltip("Stabile ID für Saves/Presets — bei Umbenennung des Assets unverändert lassen.")]
        public string Id;

        public ComponentSlot Slot;
        public int Tier = 1;

        [Tooltip("Pfad-Identität im künftigen Skill-Tree, z. B. \"gun.sniper\".")]
        public string PathId;

        [Tooltip("Base-Stats (Flat) + Modifier (PercentAdd/PercentMult) dieses Bauteils.")]
        public List<StatModifier> Stats = new();

        public Sprite Icon;
    }
}
