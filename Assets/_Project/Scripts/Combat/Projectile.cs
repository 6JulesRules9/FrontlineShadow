using UnityEngine;

namespace FrontlineShadow.Combat
{
    /// <summary>
    /// Phase-2-Projektil (04_ROADMAP.md Phase 2): fliegt geradeaus, verschwindet
    /// nach Ablauf der Lebenszeit. Keine Schadens-/Treffer-Auflösung — das
    /// Schadensmodell kommt erst in Phase 3.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float _speed = 80f;
        [SerializeField] float _lifetimeSeconds = 4f;

        float _age;

        public void Launch(float speed)
        {
            _speed = speed;
        }

        void Update()
        {
            transform.position += transform.forward * (_speed * Time.deltaTime);

            _age += Time.deltaTime;
            if (_age >= _lifetimeSeconds)
                Destroy(gameObject);
        }
    }
}
