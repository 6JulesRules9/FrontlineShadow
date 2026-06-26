namespace FrontlineShadow.Stats
{
    /// <summary>
    /// Wie ein <see cref="StatModifier"/>-Wert in die Aggregation eingeht.
    /// </summary>
    public enum ModOp
    {
        /// <summary>Wird direkt aufsummiert (auch Base-Stats einer Komponente).</summary>
        Flat,

        /// <summary>Prozent-Boni gleicher Quelle addieren sich, bevor sie angewandt werden.</summary>
        PercentAdd,

        /// <summary>Jeder Multiplikator wirkt unabhängig (stackt multiplikativ).</summary>
        PercentMult,
    }
}
