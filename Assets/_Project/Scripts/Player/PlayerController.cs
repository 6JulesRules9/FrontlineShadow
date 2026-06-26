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
    /// Phase-2-Steuerung (04_ROADMAP.md Phase 2): Hull via WASD, Turm folgt der
    /// Maus (Raycast auf Boden-Ebene), Schuss per Linksklick — gegated durch den
    /// <see cref="StatType.ReloadTime"/>-Stat. Bewusst arcade-nah (siehe
    /// 02_GAME_DESIGN.md §12 "Steuerungs-Feeling" — explizit nicht blockierend,
    /// Entscheidung fällt beim Balancing).
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Transform _turret;
        [SerializeField] Transform _muzzle;
        [SerializeField] Projectile _projectilePrefab;
        [SerializeField] float _projectileSpeed = 80f;

        HunterMotor _motor;
        float _currentSpeed;
        float _turretYaw;
        float _reloadCooldown;
        float _reloadTime = 1f;

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
        }

        void Update()
        {
            if (_motor == null) return;

            DriveHull();
            AimTurret();
            HandleFiring();
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

            var targetYaw = _turretYaw;
            var mouse = Mouse.current;
            var camera = Camera.main;
            if (mouse != null && camera != null)
            {
                var ray = camera.ScreenPointToRay(mouse.position.ReadValue());
                var groundPlane = new Plane(Vector3.up, _turret.position);
                if (groundPlane.Raycast(ray, out var distance))
                {
                    var hitPoint = ray.GetPoint(distance);
                    var direction = hitPoint - _turret.position;
                    direction.y = 0f;
                    if (direction.sqrMagnitude > 0.001f)
                    {
                        var worldYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                        targetYaw = worldYaw - transform.eulerAngles.y;
                    }
                }
            }

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
