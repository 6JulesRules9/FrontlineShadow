using FrontlineShadow.Combat;
using NUnit.Framework;

namespace FrontlineShadow.Tests
{
    public class DamageResolverTests
    {
        [Test]
        public void Resolve_PenetrationAboveArmor_DealsFullAlpha()
        {
            var damage = DamageResolver.Resolve(new DamageInfo(300f, 200f), armor: 100f);

            Assert.AreEqual(300f, damage, 0.001f);
        }

        [Test]
        public void Resolve_PenetrationEqualsArmor_DealsFullAlpha()
        {
            var damage = DamageResolver.Resolve(new DamageInfo(300f, 100f), armor: 100f);

            Assert.AreEqual(300f, damage, 0.001f);
        }

        [Test]
        public void Resolve_PenetrationFarBelowArmor_Ricochets()
        {
            // ratio 40/100 = 0.4 < RicochetThreshold (0.5)
            var damage = DamageResolver.Resolve(new DamageInfo(300f, 40f), armor: 100f);

            Assert.AreEqual(0f, damage, 0.001f);
        }

        [Test]
        public void Resolve_PenetrationAtRicochetThreshold_DealsZero()
        {
            // ratio 50/100 = 0.5 == RicochetThreshold
            var damage = DamageResolver.Resolve(new DamageInfo(300f, 50f), armor: 100f);

            Assert.AreEqual(0f, damage, 0.001f);
        }

        [Test]
        public void Resolve_PenetrationBetweenThresholdAndArmor_DealsPartialDamage()
        {
            // ratio 75/100 = 0.75 → t = InverseLerp(0.5, 1, 0.75) = 0.5 → 150
            var damage = DamageResolver.Resolve(new DamageInfo(300f, 75f), armor: 100f);

            Assert.AreEqual(150f, damage, 0.001f);
        }

        [Test]
        public void Resolve_ZeroArmor_DealsFullAlpha()
        {
            var damage = DamageResolver.Resolve(new DamageInfo(300f, 10f), armor: 0f);

            Assert.AreEqual(300f, damage, 0.001f);
        }

        [Test]
        public void Resolve_ZeroAlpha_DealsZero()
        {
            var damage = DamageResolver.Resolve(new DamageInfo(0f, 500f), armor: 100f);

            Assert.AreEqual(0f, damage, 0.001f);
        }
    }
}
