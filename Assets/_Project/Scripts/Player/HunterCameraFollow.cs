using UnityEngine;
using UnityEngine.InputSystem;

namespace FrontlineShadow.Player
{
    /// <summary>
    /// WoT-Stil-Orbit-Kamera (04_ROADMAP.md Phase 2, 02_GAME_DESIGN.md §7
    /// "Third-Person"): die Maus dreht die Kamera um den Hunter (Yaw + Pitch),
    /// der Cursor ist gesperrt, die Reticle bleibt zentriert. Der
    /// <see cref="PlayerController"/> richtet den Turm dorthin, wo die Kamera
    /// blickt — die Kamera ist also der Owner des Maus-Inputs.
    /// </summary>
    public class HunterCameraFollow : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] float _distance = 9f;
        [SerializeField] float _focusHeight = 1.5f;
        [SerializeField] float _mouseSensitivity = 0.15f;
        [SerializeField] float _minPitch = -5f;
        [SerializeField] float _maxPitch = 55f;

        float _yaw;
        float _pitch = 15f;

        public void SetTarget(Transform target) => _target = target;

        void Start()
        {
            if (_target != null) _yaw = _target.eulerAngles.y;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void LateUpdate()
        {
            if (_target == null) return;

            var mouse = Mouse.current;
            if (mouse != null)
            {
                _yaw += mouse.delta.x.ReadValue() * _mouseSensitivity;
                _pitch = Mathf.Clamp(_pitch - mouse.delta.y.ReadValue() * _mouseSensitivity, _minPitch, _maxPitch);
            }

            var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            var focus = _target.position + Vector3.up * _focusHeight;

            transform.position = focus - rotation * Vector3.forward * _distance;
            transform.rotation = rotation;
        }
    }
}
