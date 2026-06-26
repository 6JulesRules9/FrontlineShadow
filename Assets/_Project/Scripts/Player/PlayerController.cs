using System.Collections.Generic;
using FrontlineShadow.Combat;
using FrontlineShadow.Core;
using FrontlineShadow.Stats;
using FrontlineShadow.Tank;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FrontlineShadow.Player
{
    /// <summary>
    /// Phase-2-Steuerung (04_ROADMAP.md Phase 2): Hull via WASD, Maus dreht die
    /// Blickrichtung (WoT-Stil — Reticle bleibt zentriert, Turm/Kamera folgen
    /// der Blickrichtung, begrenzt durch <see cref="StatType.TurretTraverse"/>),
    /// Schuss per Linksklick — gegated durch <see cref="StatType.ReloadTime"/>.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Transform _turret;
        [SerializeField] Transform _muzzle;
        [SerializeField] Projectile _projectilePrefab;
        [SerializeField] float _projectileSpeed = 80f;
        [SerializeField] float _mouseSensitivity = 0.2f;

        HunterMotor _motor;
        float _currentSpeed;
        float _turretYaw;
        float _lookYaw;
        float _reloadCooldown;
        float _reloadTime = 1f;

        /// <summary>Welt-Yaw, dem Turm und Kamera nachlaufen (von der Maus gesteuert).</summary>
        public float LookYaw => _lookYaw;

        void Start()
        {
            if (!ServiceLocator.TryGet<LoadoutService>(out var loadout))
            {
                Debug.LogError("[PlayerController] LoadoutService nicht registriert — zuerst die Bootstrap-Szene laden.");
                return;
            }

            var stats = loadout.ComputeFinalStats();
            _motor = new HunterMotor(stats);
            _reloadTime = Mathf.Max(0.1f, stats.GetValueOrDefault(StatType.ReloadTime));
            _lookYaw = transform.eulerAngles.y;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void Update()
        {
            if (_motor == null) return;

            UpdateLookYaw();
            DriveHull();
            AimTurret();
            HandleFiring();
        }

        void UpdateLookYaw()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            _lookYaw += mouse.delta.x.ReadValue() * _mouseSensitivity;
        }

        void DriveHull()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            var throttle = 0f;
            if (keyboard.wKey.isPressed) throttle += 1f;
            if (keyboard.sKey.isPressed) throttle -= 1f;

            var turn = 0f;
            if (keyboard.dKey.isPressed) turn += 1f;
            if (keyboard.aKey.isPressed) turn -= 1f;

            _currentSpeed = _motor.StepSpeed(_currentSpeed, throttle, Time.deltaTime);
            transform.Rotate(Vector3.up, _motor.StepHullYawDelta(turn, Time.deltaTime));
            transform.position += transform.forward * (_currentSpeed * Time.deltaTime);
        }

        void AimTurret()
        {
            if (_turret == null) return;

            var targetYaw = Mathf.DeltaAngle(transform.eulerAngles.y, _lookYaw);
            _turretYaw = _motor.StepTurretYaw(_turretYaw, targetYaw, Time.deltaTime);
            _turret.localEulerAngles = new Vector3(0f, _turretYaw, 0f);
        }

        void HandleFiring()
        {
            _reloadCooldown -= Time.deltaTime;

            var mouse = Mouse.current;
            if (mouse == null || !mouse.leftButton.wasPressedThisFrame) return;
            if (_reloadCooldown > 0f) return;
            if (_projectilePrefab == null || _muzzle == null) return;

            var projectile = Instantiate(_projectilePrefab, _muzzle.position, _muzzle.rotation);
            projectile.Launch(_projectileSpeed);
            _reloadCooldown = _reloadTime;
        }
    }
}
