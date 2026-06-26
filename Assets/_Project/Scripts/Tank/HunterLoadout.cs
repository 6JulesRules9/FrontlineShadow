using System;

namespace FrontlineShadow.Tank
{
    /// <summary>
    /// Ein komplettes Hunter-Loadout: je Slot eine Bauteil-Instanz-ID
    /// (03_TECH_ARCHITEKTUR.md §4.3). Ammo/Skill-Bindung kommen in Phase 3.
    /// </summary>
    [Serializable]
    public class HunterLoadout
    {
        public const int SlotCount = 5;

        public string[] ComponentInstanceIds = new string[SlotCount];
        public string PresetName;

        public HunterLoadout Clone()
        {
            return new HunterLoadout
            {
                ComponentInstanceIds = (string[])ComponentInstanceIds.Clone(),
                PresetName = PresetName,
            };
        }
    }
}
