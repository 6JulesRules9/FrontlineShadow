using UnityEngine;

namespace FrontlineShadow.Combat
{
    /// <summary>
    /// Treffbares Ziel mit HP und Panzerung (02_GAME_DESIGN.md §7). Phase 3:
    /// ein HP-Pool je Objekt — der pro-Komponente-Schaden der Real-Time-Meta
    /// (§8.2) kommt erst in Phase 5. Bei HP ≤ 0 wird das Objekt zerstört.
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        [SerializeField] float _maxHp = 300f;
        [SerializeField] float _armor = 100f;

        float _hp;

        public float Hp => _hp;
        public float MaxHp => _maxHp;
        public float Armor => _armor;
        public bool IsDead => _hp <= 0f;

        /// <summary>Letzter tatsächlich verursachter Schaden — praktisch für HUD/Debug.</summary>
        public float LastDamageTaken { get; private set; }

        void Awake()
        {
            _hp = _maxHp;
        }

        /// <summary>Wendet einen Treffer an; gibt den tatsächlich verursachten Schaden zurück.</summary>
        public float TakeHit(DamageInfo hit)
        {
            if (IsDead) return 0f;

            var damage = DamageResolver.Resolve(hit, _armor);
            LastDamageTaken = damage;

            if (damage <= 0f) return 0f;

            _hp = Mathf.Max(0f, _hp - damage);
            if (IsDead) OnDeath();

            return damage;
        }

        void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
