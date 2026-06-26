using UnityEngine;

namespace FrontlineShadow.Player
{
    /// <summary>
    /// WoT-Stil-Folgekamera (04_ROADMAP.md Phase 2, 02_GAME_DESIGN.md §7
    /// "Third-Person"): orbiert um den Hunter entlang der Blickrichtung
    /// (<see cref="PlayerController.LookYaw"/>), nicht der Hull-Ausrichtung —
    /// die Reticle (Bildschirmmitte) bleibt so mit der Zielrichtung deckungsgleich.
    /// </summary>
    public class HunterCameraFollow : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] Vector3 _offset = new(0f, 6f, -8f);
        [SerializeField] float _pitchDeg = 15f;
        [SerializeField] float _followSpeed = 8f;

        PlayerController _playerController;

        public void SetTarget(Transform target)
        {
            _target = target;
            _playerController = target != null ? target.GetComponent<PlayerController>() : null;
        }

        void LateUpdate()
        {
            if (_target == null) return;

            var lookYaw = _playerController != null ? _playerController.LookYaw : _target.eulerAngles.y;
            var yawRotation = Quaternion.Euler(0f, lookYaw, 0f);
            var desiredPosition = _target.position + yawRotation * _offset;
            var desiredRotation = Quaternion.Euler(_pitchDeg, lookYaw, 0f);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _followSpeed * Time.deltaTime);
        }
    }
}
