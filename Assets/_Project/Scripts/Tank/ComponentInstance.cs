using System;

namespace FrontlineShadow.Tank
{
    /// <summary>
    /// Eine konkrete, besessene Ausprägung eines <see cref="TankComponentDef"/>.
    /// Trägt Zustand, der am Bauteil selbst klebt statt am Loadout — Schaden
    /// (Phase 5) und gebundene Skills (Phase 3) bleiben so auf der Instanz, wenn
    /// man andere Slots tauscht (03_TECH_ARCHITEKTUR.md §4.3).
    /// </summary>
    [Serializable]
    public class ComponentInstance
    {
        public string Id;
        public string DefId;

        /// <summary>0–100. Bis Phase 5 (Repair) immer 0.</summary>
        public float DamagePercent;

        /// <summary>Id der gebundenen Skill-Instanz. Bis Phase 3 immer null.</summary>
        public string BoundSkillId;

        public ComponentInstance() { }

        public ComponentInstance(string id, string defId)
        {
            Id = id;
            DefId = defId;
        }
    }
}
