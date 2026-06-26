using System;
using System.Collections.Generic;
using FrontlineShadow.Config;
using FrontlineShadow.Stats;
using UnityEngine;

namespace FrontlineShadow.Tank
{
    /// <summary>
    /// Berechnet Final-Stats aus den Modifiern aller equippten Bauteile.
    /// Pro Stat: <c>FlatSum × (1 + Σ PercentAdd) × Π(1 + PercentMult)</c>
    /// (02_GAME_DESIGN.md §2.1, [VORSCHLAG]). Danach koppelt <see cref="StatType.Weight"/>
    /// den <see cref="StatType.TopSpeed"/> über <see cref="BalanceConfig.WeightSpeedCoupling"/>.
    /// </summary>
    public static class StatAggregator
    {
        public static Dictionary<StatType, float> Compute(IEnumerable<TankComponentDef> equipped, BalanceConfig balance)
        {
            var flat = new Dictionary<StatType, float>();
            var percentAdd = new Dictionary<StatType, float>();
            var percentMult = new Dictionary<StatType, float>();

            foreach (var component in equipped)
            {
                if (component == null) continue;

                foreach (var mod in component.Stats)
                {
                    switch (mod.Op)
                    {
                        case ModOp.Flat:
                            flat[mod.Type] = flat.GetValueOrDefault(mod.Type) + mod.Value;
                            break;
                        case ModOp.PercentAdd:
                            percentAdd[mod.Type] = percentAdd.GetValueOrDefault(mod.Type) + mod.Value;
                            break;
                        case ModOp.PercentMult:
                            percentMult[mod.Type] = percentMult.GetValueOrDefault(mod.Type, 1f) * (1f + mod.Value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(mod.Op), mod.Op, "Unbekannter ModOp.");
                    }
                }
            }

            var result = new Dictionary<StatType, float>();
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                var baseValue = flat.GetValueOrDefault(type);
                var addFactor = 1f + percentAdd.GetValueOrDefault(type);
                var multFactor = percentMult.GetValueOrDefault(type, 1f);
                result[type] = baseValue * addFactor * multFactor;
            }

            if (balance != null)
            {
                var weight = result.GetValueOrDefault(StatType.Weight);
                var excess = Mathf.Max(0f, weight - balance.ReferenceWeight);
                var speedMultiplier = Mathf.Max(0f, 1f - balance.WeightSpeedCoupling * excess);
                result[StatType.TopSpeed] *= speedMultiplier;
            }

            return result;
        }
    }
}
