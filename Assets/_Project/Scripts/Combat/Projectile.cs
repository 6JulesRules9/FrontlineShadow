using UnityEngine;

namespace FrontlineShadow.Combat
{
    /// <summary>
    /// Projektil (04_ROADMAP.md Phase 2 → 3): fliegt geradeaus, verschwindet nach
    /// Ablauf der Lebenszeit. Treffererkennung per Segment-Raycast über die in
    /// diesem Frame zurückgelegte Strecke (kein Physics-Rigidbody nötig). Bei
    /// einem <see cref="Damageable"/> wird der Schaden über
    /// <see cref="DamageResolver"/> aufgelöst.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float _speed = 80f;
        [SerializeField] float _lifetimeSeconds = 4f;
        [SerializeField] LayerMask _hitMask = ~0;

        float _age;
        DamageInfo _damage;

        void Awake()
        {
            EnsureTracer();
        }

        /// <summary>Gibt dem Projektil eine sichtbare Leuchtspur — auch wenn das Prefab keine hat.</summary>
        void EnsureTracer()
        {
            var trail = GetComponent<TrailRenderer>();
            if (trail == null) trail = gameObject.AddComponent<TrailRenderer>();

            trail.time = 0.35f;
            trail.startWidth = 0.25f;
            trail.endWidth = 0.02f;
            trail.numCapVertices = 4;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            trail.startColor = new Color(1f, 0.85f, 0.3f, 1f);
            trail.endColor = new Color(1f, 0.45f, 0f, 0f);
        }

        public void Launch(float speed)
        {
            _speed = speed;
        }

        public void Launch(float speed, DamageInfo damage)
        {
            _speed = speed;
            _damage = damage;
        }

        void Update()
        {
            var step = _speed * Time.deltaTime;

            if (Physics.Raycast(transform.position, transform.forward, out var hit, step, _hitMask, QueryTriggerInteraction.Ignore))
            {
                OnHit(hit.collider);
                return;
            }

            transform.position += transform.forward * step;

            _age += Time.deltaTime;
            if (_age >= _lifetimeSeconds)
                Destroy(gameObject);
        }

        void OnHit(Collider collider)
        {
            var target = collider.GetComponentInParent<Damageable>();
            if (target != null)
                target.TakeHit(_damage);

            Destroy(gameObject);
        }
    }
}
