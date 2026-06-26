using System;

namespace FrontlineShadow.Stats
{
    /// <summary>
    /// Ein einzelner Beitrag zu einem Stat — entweder Base-Wert (<see cref="ModOp.Flat"/>)
    /// oder Modifier (<see cref="ModOp.PercentAdd"/>/<see cref="ModOp.PercentMult"/>).
    /// Prozentwerte sind Fraktionen (0.15 = +15%).
    /// </summary>
    [Serializable]
    public struct StatModifier
    {
        public StatType Type;
        public ModOp Op;
        public float Value;

        public StatModifier(StatType type, ModOp op, float value)
        {
            Type = type;
            Op = op;
            Value = value;
        }
    }
}
