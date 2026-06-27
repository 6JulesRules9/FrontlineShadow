namespace FrontlineShadow.Stats
{
    /// <summary>
    /// Stat-Kanäle, die Komponenten beeinflussen können (siehe
    /// 03_TECH_ARCHITEKTUR.md §4.2 und 02_GAME_DESIGN.md §2.1). Erste Iteration —
    /// wächst mit Kampf-/Vision-Systemen (Phase 2/3).
    /// </summary>
    // WICHTIG: Reihenfolge = serialisierter Integer-Index in den .asset-Dateien.
    // Neue Werte NUR HINTEN anhängen — Einfügen in der Mitte verschiebt alle
    // folgenden Indizes und korrumpiert bestehende ScriptableObject-Daten.
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
        TurretTraverse,
    }
}
