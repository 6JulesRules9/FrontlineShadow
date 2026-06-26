using System.Collections.Generic;
using FrontlineShadow.Config;
using FrontlineShadow.Stats;
using FrontlineShadow.Tank;
using NUnit.Framework;
using UnityEngine;

namespace FrontlineShadow.Tests
{
    public class StatAggregatorTests
    {
        static TankComponentDef MakeComponent(params StatModifier[] mods)
        {
            var def = ScriptableObject.CreateInstance<TankComponentDef>();
            def.Stats = new List<StatModifier>(mods);
            return def;
        }

        [Test]
        public void Compute_SumsFlatModifiers()
        {
            var a = MakeComponent(new StatModifier(StatType.Hp, ModOp.Flat, 100));
            var b = MakeComponent(new StatModifier(StatType.Hp, ModOp.Flat, 50));

            var result = StatAggregator.Compute(new[] { a, b }, null);

            Assert.AreEqual(150f, result[StatType.Hp], 0.001f);
        }

        [Test]
        public void Compute_AppliesPercentAdd_Additively()
        {
            var a = MakeComponent(
                new StatModifier(StatType.TopSpeed, ModOp.Flat, 100),
                new StatModifier(StatType.TopSpeed, ModOp.PercentAdd, 0.1f));
            var b = MakeComponent(new StatModifier(StatType.TopSpeed, ModOp.PercentAdd, 0.2f));

            var result = StatAggregator.Compute(new[] { a, b }, null);

            Assert.AreEqual(130f, result[StatType.TopSpeed], 0.001f);
        }

        [Test]
        public void Compute_AppliesPercentMult_Multiplicatively()
        {
            var a = MakeComponent(
                new StatModifier(StatType.Penetration, ModOp.Flat, 100),
                new StatModifier(StatType.Penetration, ModOp.PercentMult, 0.1f));
            var b = MakeComponent(new StatModifier(StatType.Penetration, ModOp.PercentMult, 0.2f));

            var result = StatAggregator.Compute(new[] { a, b }, null);

            Assert.AreEqual(132f, result[StatType.Penetration], 0.001f);
        }

        [Test]
        public void Compute_NullBalance_SkipsWeightSpeedCoupling()
        {
            var a = MakeComponent(
                new StatModifier(StatType.TopSpeed, ModOp.Flat, 60),
                new StatModifier(StatType.Weight, ModOp.Flat, 5000));

            var result = StatAggregator.Compute(new[] { a }, null);

            Assert.AreEqual(60f, result[StatType.TopSpeed], 0.001f);
        }

        [Test]
        public void Compute_WeightBelowReference_NoSpeedPenalty()
        {
            var balance = ScriptableObject.CreateInstance<BalanceConfig>();
            balance.ReferenceWeight = 1000f;
            balance.WeightSpeedCoupling = 0.0005f;

            var a = MakeComponent(
                new StatModifier(StatType.TopSpeed, ModOp.Flat, 60),
                new StatModifier(StatType.Weight, ModOp.Flat, 500));

            var result = StatAggregator.Compute(new[] { a }, balance);

            Assert.AreEqual(60f, result[StatType.TopSpeed], 0.001f);
        }

        [Test]
        public void Compute_WeightAboveReference_ReducesTopSpeed()
        {
            var balance = ScriptableObject.CreateInstance<BalanceConfig>();
            balance.ReferenceWeight = 1000f;
            balance.WeightSpeedCoupling = 0.0005f;

            var a = MakeComponent(
                new StatModifier(StatType.TopSpeed, ModOp.Flat, 60),
                new StatModifier(StatType.Weight, ModOp.Flat, 1200));

            var result = StatAggregator.Compute(new[] { a }, balance);

            Assert.AreEqual(54f, result[StatType.TopSpeed], 0.001f);
        }

        [Test]
        public void Compute_NullComponentInList_IsIgnored()
        {
            var a = MakeComponent(new StatModifier(StatType.Hp, ModOp.Flat, 100));

            var result = StatAggregator.Compute(new TankComponentDef[] { a, null }, null);

            Assert.AreEqual(100f, result[StatType.Hp], 0.001f);
        }
    }
}
