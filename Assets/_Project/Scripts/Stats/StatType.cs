namespace FrontlineShadow.Stats
{
    /// <summary>
    /// Stat-Kanäle, die Komponenten beeinflussen können (siehe
    /// 03_TECH_ARCHITEKTUR.md §4.2 und 02_GAME_DESIGN.md §2.1). Erste Iteration —
    /// wächst mit Kampf-/Vision-Systemen (Phase 2/3).
    /// </summary>
    public enum StatType
    {
        TopSpeed,
        Accel,
        HullTraverse,
        Hp,
        ArmorFront,
        Weight,
        AlphaDamage,
        Penetration,
        ReloadTime,
        Dispersion,
        AimTime,
        ViewRange,
        SpottingSpeed,
        SpottingPersistence,
        Camo,
    }
}
