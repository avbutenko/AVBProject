using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Assets.PixelCrew.Creatures.Weapons
{
    public class Pistol : MonoBehaviour
    {
        [SerializeField] private Transform _ownerTransform;
        [SerializeField] private UnityEvent _onShoot;

        private Vector3 _mousePosition;

        void Update()
        {
            HandleAiming();
            HandleShooting();
        }

        private void HandleAiming()
        {
            Vector3 pistolDirection = (GetAimPosition() - transform.position).normalized;
            float angle = Mathf.Atan2(pistolDirection.y, pistolDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);

            Vector3 pistolLocaleScale = Vector3.one;
            if (angle > 90 || angle < -90)
            {
                pistolLocaleScale.y = -1f;
            }
            else
            {
                pistolLocaleScale.y = +1f;
            }

            if (_ownerTransform.localScale.x < 0)
            {
                pistolLocaleScale.x = -1f;
            }

            transform.localScale = pistolLocaleScale;
        }

        private Vector3 GetAimPosition()
        {
            return GetMousePosition();
        }

        private Vector3 GetMousePosition()
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _mousePosition.z = 0f;
            return _mousePosition;
        }

        private void HandleShooting()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _onShoot?.Invoke();
            }
        }
    }
}