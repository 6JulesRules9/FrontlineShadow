namespace FrontlineShadow.Combat
{
    /// <summary>
    /// Schadens-Nutzlast eines Treffers (02_GAME_DESIGN.md §7 "Schadensmodell:
    /// Durchschlag vs. Panzerung, Alpha-Damage"). Trägt die kampfrelevanten
    /// Geschütz-Werte vom Schuss bis zur Auflösung am Ziel.
    /// </summary>
    public readonly struct DamageInfo
    {
        /// <summary>Voller Schaden bei Durchschlag (Geschütz-Stat AlphaDamage).</summary>
        public readonly float AlphaDamage;

        /// <summary>Durchschlagskraft, die gegen die Panzerung des Ziels geprüft wird.</summary>
        public readonly float Penetration;

        public DamageInfo(float alphaDamage, float penetration)
        {
            AlphaDamage = alphaDamage;
            Penetration = penetration;
        }
    }
}
