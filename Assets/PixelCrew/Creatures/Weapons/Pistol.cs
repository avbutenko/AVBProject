using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Assets.PixelCrew.Utils;
using AVBProject.Creatures.Weapons;
using System;

namespace Assets.PixelCrew.Creatures.Weapons
{
    public class Pistol : MonoBehaviour
    {
        [Space]
        [Header("Settings")]
        [SerializeField] private Transform _ownerTransform;

        [Space]
        [Header("Bullet Settings")]
        [SerializeField] private DirectionalProjectile _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPosition;

        [Space]
        [Header("Projection Settings")]
        [SerializeField] private GameObject _pointPrefab;
        [SerializeField] private int _pointNum;
        [SerializeField] private float spaceBetweenPoints;

        private Vector3 _mousePosition;
        private Vector3 _pistolDirection;
        private GameObject[] _points;

        private void Start()
        {
            _points = new GameObject[_pointNum];
            for (int i = 0; i < _pointNum; i++)
            {
                _points[i] = Instantiate(_pointPrefab, _bulletSpawnPosition.position, Quaternion.identity);
            }
        }

        void Update()
        {
            HandleAiming();
            DrawProjection();
            HandleShooting();
        }

        private void DrawProjection()
        {
            for (int i = 0; i < _pointNum; i++)
            {
                _points[i].transform.position = GetPointPosition(i * spaceBetweenPoints);
            }
        }

        private Vector2 GetPointPosition(float time)
        {
            Vector2 delta = _bulletPrefab.Speed * time * _pistolDirection;
            Vector2 position = (Vector2)_bulletSpawnPosition.position + delta;
            return position;
        }

        private void HandleAiming()
        {
            _pistolDirection = (GetAimPosition() - transform.position).normalized;
            float angle = Mathf.Atan2(_pistolDirection.y, _pistolDirection.x) * Mathf.Rad2Deg;
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
                var instance = SpawnUtils.Spawn(_bulletPrefab.gameObject, _bulletSpawnPosition.position);
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(_pistolDirection);
            }
        }
    }
}