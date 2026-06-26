using System.Collections.Generic;
using FrontlineShadow.Player;
using FrontlineShadow.Stats;
using NUnit.Framework;

namespace FrontlineShadow.Tests
{
    public class HunterMotorTests
    {
        static HunterMotor MakeMotor(float topSpeed = 50f, float accel = 10f, float hullTraverse = 30f, float turretTraverse = 60f)
        {
            return new HunterMotor(new Dictionary<StatType, float>
            {
                { StatType.TopSpeed, topSpeed },
                { StatType.Accel, accel },
                { StatType.HullTraverse, hullTraverse },
                { StatType.TurretTraverse, turretTraverse },
            });
        }

        [Test]
        public void StepSpeed_AcceleratesTowardsMaxSpeed_LimitedByAccel()
        {
            var motor = MakeMotor(topSpeed: 50f, accel: 10f);

            var speed = motor.StepSpeed(0f, throttle: 1f, deltaTime: 1f);

            Assert.AreEqual(10f, speed, 0.001f);
        }

        [Test]
        public void StepSpeed_ReachesMaxSpeed_WhenAccelExceedsGap()
        {
            var motor = MakeMotor(topSpeed: 50f, accel: 100f);

            var speed = motor.StepSpeed(0f, throttle: 1f, deltaTime: 1f);

            Assert.AreEqual(50f, speed, 0.001f);
        }

        [Test]
        public void StepSpeed_DecelerateTowardsZero_WhenNoThrottle()
        {
            var motor = MakeMotor(topSpeed: 50f, accel: 10f);

            var speed = motor.StepSpeed(20f, throttle: 0f, deltaTime: 1f);

            Assert.AreEqual(10f, speed, 0.001f);
        }

        [Test]
        public void StepSpeed_ReverseThrottle_TargetsNegativeMaxSpeed()
        {
            var motor = MakeMotor(topSpeed: 50f, accel: 100f);

            var speed = motor.StepSpeed(0f, throttle: -1f, deltaTime: 1f);

            Assert.AreEqual(-50f, speed, 0.001f);
        }

        [Test]
        public void StepHullYawDelta_ScalesWithTurnInputAndDeltaTime()
        {
            var motor = MakeMotor(hullTraverse: 30f);

            var delta = motor.StepHullYawDelta(turnInput: 1f, deltaTime: 0.5f);

            Assert.AreEqual(15f, delta, 0.001f);
        }

        [Test]
        public void StepHullYawDelta_ClampsTurnInput()
        {
            var motor = MakeMotor(hullTraverse: 30f);

            var delta = motor.StepHullYawDelta(turnInput: 5f, deltaTime: 1f);

            Assert.AreEqual(30f, delta, 0.001f);
        }

        [Test]
        public void StepTurretYaw_MovesTowardsTarget_LimitedByTurretTraverse()
        {
            var motor = MakeMotor(turretTraverse: 60f);

            var yaw = motor.StepTurretYaw(currentYawDeg: 0f, targetYawDeg: 90f, deltaTime: 1f);

            Assert.AreEqual(60f, yaw, 0.001f);
        }

        [Test]
        public void StepTurretYaw_ReachesTarget_WhenWithinRange()
        {
            var motor = MakeMotor(turretTraverse: 60f);

            var yaw = motor.StepTurretYaw(currentYawDeg: 0f, targetYawDeg: 10f, deltaTime: 1f);

            Assert.AreEqual(10f, yaw, 0.001f);
        }

        [Test]
        public void StepTurretYaw_TakesShortestPathAcross360DegreeBoundary()
        {
            var motor = MakeMotor(turretTraverse: 10f);

            var yaw = motor.StepTurretYaw(currentYawDeg: 350f, targetYawDeg: 20f, deltaTime: 1f);

            Assert.AreEqual(360f, yaw, 0.001f);
        }

        [Test]
        public void MissingStats_DefaultToZero()
        {
            var motor = new HunterMotor(new Dictionary<StatType, float>());

            Assert.AreEqual(0f, motor.MaxSpeed);
            Assert.AreEqual(0f, motor.HullTurnRateDegPerSec);
            Assert.AreEqual(0f, motor.TurretTurnRateDegPerSec);
        }
    }
}
