using System.Collections.Generic;
using FrontlineShadow.Stats;
using UnityEngine;

namespace FrontlineShadow.Player
{
    /// <summary>
    /// Reine Bewegungs-Mathe für den Hunter, abgeleitet aus Final-Stats
    /// (04_ROADMAP.md Phase 2: "Bewegung aus Mobilitäts-Stats"). Bewusst ohne
    /// MonoBehaviour-Abhängigkeit gehalten (Simulation ⟂ Präsentation,
    /// 03_TECH_ARCHITEKTUR.md) — <see cref="PlayerController"/> ruft das hier auf.
    /// </summary>
    public class HunterMotor
    {
        readonly float _maxSpeed;
        readonly float _accel;
        readonly float _hullTurnRateDegPerSec;
        readonly float _turretTurnRateDegPerSec;

        public float MaxSpeed => _maxSpeed;
        public float HullTurnRateDegPerSec => _hullTurnRateDegPerSec;
        public float TurretTurnRateDegPerSec => _turretTurnRateDegPerSec;

        public HunterMotor(Dictionary<StatType, float> stats)
        {
            _maxSpeed = stats.GetValueOrDefault(StatType.TopSpeed);
            _accel = stats.GetValueOrDefault(StatType.Accel);
            _hullTurnRateDegPerSec = stats.GetValueOrDefault(StatType.HullTraverse);
            _turretTurnRateDegPerSec = stats.GetValueOrDefault(StatType.TurretTraverse);
        }

        /// <summary>Bewegt die aktuelle Geschwindigkeit Richtung Ziel (throttle * MaxSpeed), begrenzt durch Accel.</summary>
        public float StepSpeed(float currentSpeed, float throttle, float deltaTime)
        {
            var target = _maxSpeed * Mathf.Clamp(throttle, -1f, 1f);
            var maxDelta = _accel * deltaTime;
            return Mathf.MoveTowards(currentSpeed, target, maxDelta);
        }

        /// <summary>Gibt die Hull-Drehung (Grad) für diesen Frame zurück.</summary>
        public float StepHullYawDelta(float turnInput, float deltaTime)
            => Mathf.Clamp(turnInput, -1f, 1f) * _hullTurnRateDegPerSec * deltaTime;

        /// <summary>Bewegt den aktuellen Turm-Yaw (Grad) Richtung Ziel-Yaw, begrenzt durch TurretTraverse.</summary>
        public float StepTurretYaw(float currentYawDeg, float targetYawDeg, float deltaTime)
        {
            var maxDelta = _turretTurnRateDegPerSec * deltaTime;
            return Mathf.MoveTowardsAngle(currentYawDeg, targetYawDeg, maxDelta);
        }
    }
}
