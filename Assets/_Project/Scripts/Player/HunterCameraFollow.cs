using UnityEngine;

namespace FrontlineShadow.Player
{
    /// <summary>
    /// Einfache Third-Person-Folgekamera für die Battle-Szene (04_ROADMAP.md
    /// Phase 2, 02_GAME_DESIGN.md §7 "Third-Person"). Kein Smoothing-Anspruch —
    /// nur genug, um den Hunter im Bild zu halten.
    /// </summary>
    public class HunterCameraFollow : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] Vector3 _offset = new(0f, 6f, -8f);
        [SerializeField] float _followSpeed = 8f;

        public void SetTarget(Transform target) => _target = target;

        void LateUpdate()
        {
            if (_target == null) return;

            var desiredPosition = _target.position + _target.TransformDirection(_offset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);
            transform.LookAt(_target.position + Vector3.up * 1.5f);
        }
    }
}
