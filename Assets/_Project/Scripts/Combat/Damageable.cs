using UnityEngine;

namespace FrontlineShadow.Combat
{
    /// <summary>
    /// Treffbares Ziel mit HP und Panzerung (02_GAME_DESIGN.md §7). Phase 3:
    /// ein HP-Pool je Objekt — der pro-Komponente-Schaden der Real-Time-Meta
    /// (§8.2) kommt erst in Phase 5. Bei HP ≤ 0 wird das Objekt zerstört.
    ///
    /// Sichtbares Treffer-Feedback (Farb-Flash) + Console-Logs sind bewusst
    /// minimal — echte VFX/SFX sind Entwickler-Assets (CLAUDE.md Rollen).
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        [SerializeField] float _maxHp = 300f;
        [SerializeField] float _armor = 100f;

        float _hp;
        Renderer _renderer;
        Color _baseColor;
        float _flashTimer;

        public float Hp => _hp;
        public float MaxHp => _maxHp;
        public float Armor => _armor;
        public bool IsDead => _hp <= 0f;

        /// <summary>Letzter tatsächlich verursachter Schaden — praktisch für HUD/Debug.</summary>
        public float LastDamageTaken { get; private set; }

        void Awake()
        {
            _hp = _maxHp;
            _renderer = GetComponentInChildren<Renderer>();
            if (_renderer != null) _baseColor = GetColor(_renderer.material);
        }

        void Update()
        {
            if (_flashTimer <= 0f) return;

            _flashTimer -= Time.deltaTime;
            if (_flashTimer <= 0f && _renderer != null)
                SetColor(_renderer.material, _baseColor);
        }

        /// <summary>Wendet einen Treffer an; gibt den tatsächlich verursachten Schaden zurück.</summary>
        public float TakeHit(DamageInfo hit)
        {
            if (IsDead) return 0f;

            var damage = DamageResolver.Resolve(hit, _armor);
            LastDamageTaken = damage;

            if (damage <= 0f)
            {
                Debug.Log($"[Combat] {name}: Abpraller (Pen {hit.Penetration:0} vs. Panzerung {_armor:0}).");
                Flash(new Color(0.6f, 0.6f, 0.7f));
                return 0f;
            }

            _hp = Mathf.Max(0f, _hp - damage);
            Debug.Log($"[Combat] {name}: {damage:0} Schaden → HP {_hp:0}/{_maxHp:0} (Panzerung {_armor:0}).");
            Flash(new Color(1f, 0.3f, 0.25f));

            if (IsDead) OnDeath();
            return damage;
        }

        void Flash(Color color)
        {
            if (_renderer == null) return;
            SetColor(_renderer.material, color);
            _flashTimer = 0.12f;
        }

        void OnDeath()
        {
            Debug.Log($"[Combat] {name}: zerstört.");
            Destroy(gameObject);
        }

        static Color GetColor(Material material)
            => material.HasProperty("_BaseColor") ? material.GetColor("_BaseColor") : material.color;

        static void SetColor(Material material, Color color)
        {
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            else material.color = color;
        }
    }
}
