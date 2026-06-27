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
    /// Phase-2-Steuerung (04_ROADMAP.md Phase 2): Hull via WASD. Der Turm zielt
    /// dorthin, wo die <see cref="HunterCameraFollow"/>-Kamera blickt (WoT-Stil —
    /// die Maus dreht die Kamera, die Reticle bleibt zentriert), begrenzt durch
    /// <see cref="StatType.TurretTraverse"/>. Schuss per Linksklick — gegated
    /// durch <see cref="StatType.ReloadTime"/>.
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
        DamageInfo _shotDamage;

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
            _shotDamage = new DamageInfo(
                stats.GetValueOrDefault(StatType.AlphaDamage),
                stats.GetValueOrDefault(StatType.Penetration));

            var turretRate = stats.GetValueOrDefault(StatType.TurretTraverse);
            Debug.Log($"[Player] Stats geladen — TopSpeed {stats.GetValueOrDefault(StatType.TopSpeed):0}, " +
                      $"TurretTraverse {turretRate:0}°/s, Alpha {_shotDamage.AlphaDamage:0}, " +
                      $"Pen {_shotDamage.Penetration:0}, Reload {_reloadTime:0.0}s.");
            if (turretRate <= 0f)
                Debug.LogWarning("[Player] TurretTraverse = 0 → Turm dreht nicht. Bitte " +
                                 "'Tools ▸ Frontline Shadow ▸ Setup Phase 1' erneut ausführen (rüstet den Stat nach).");
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

            var camera = Camera.main;
            var lookYaw = camera != null ? camera.transform.eulerAngles.y : transform.eulerAngles.y;

            var targetYaw = Mathf.DeltaAngle(transform.eulerAngles.y, lookYaw);
            _turretYaw = _motor.StepTurretYaw(_turretYaw, targetYaw, Time.deltaTime);
            _turret.localEulerAngles = new Vector3(0f, _turretYaw, 0f);
        }

        void HandleFiring()
        {
            _reloadCooldown -= Time.deltaTime;

            var mouse = Mouse.current;
            if (mouse == null || !mouse.leftButton.wasPressedThisFrame) return;

            if (_reloadCooldown > 0f)
            {
                Debug.Log($"[Player] Lädt nach … noch {_reloadCooldown:0.0}s (ReloadTime {_reloadTime:0.0}s).");
                return;
            }

            if (_projectilePrefab == null || _muzzle == null)
            {
                Debug.LogWarning("[Player] Schuss blockiert: Projektil-Prefab oder Muzzle nicht zugewiesen. " +
                                 "HunterRig-Prefab via 'Setup Phase 2' neu erzeugen (ggf. altes Prefab löschen).");
                return;
            }

            var projectile = Instantiate(_projectilePrefab, _muzzle.position, _muzzle.rotation);
            projectile.Launch(_projectileSpeed, _shotDamage);
            _reloadCooldown = _reloadTime;
            Debug.Log($"[Player] Schuss! Alpha {_shotDamage.AlphaDamage:0}, Pen {_shotDamage.Penetration:0}.");
        }
    }
}
