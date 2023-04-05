using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Assets.PixelCrew.Utils;
using AVBProject.Creatures.Weapons;
using System;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.UI.Hud;

namespace Assets.PixelCrew.Creatures.Weapons
{
    public class Pistol : MonoBehaviour
    {
        [Space]
        [Header("Settings")]
        [SerializeField] private Transform _ownerTransform;
        [SerializeField] private CoolDown _fireCoolDown;

        [Space]
        [Header("Bullet Settings")]
        [SerializeField] private DirectionalProjectile _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPosition;

        [Space]
        [Header("Projection Settings")]
        [SerializeField] private GameObject _pointPrefab;
        [SerializeField] private int _pointNum;
        [SerializeField] private float _spaceBetweenPoints;

        private const string BulletId = "PearlProjectile";
        private int BulletCount => _session.Data.Inventory.Count(BulletId);


        private Vector3 _pistolDirection;
        private GameObject[] _points;
        private GameSession _session;
        private GameObject _crossHairObject;

        private void Start()
        {
            _session = GameSession.Instance;
            _crossHairObject = FindObjectOfType<HudController>()._crossHairObject;
        }

        private void OnEnable()
        {
            CreateDefaultNumOfProjectionPoints();
        }

        void Update()
        {
            HandleAiming();
            DrawProjection();
        }

        private void DrawProjection()
        {
            float maxDistance = Vector3.Distance(_bulletSpawnPosition.position, GetAimPosition());

            for (int i = 0; i < _points.Length; i++)
            {
                _points[i].transform.position = GetPointPosition(i * _spaceBetweenPoints);
                float distance = Vector3.Distance(_bulletSpawnPosition.position, _points[i].transform.position);
                if (distance < maxDistance)
                {

                    _points[i].SetActive(true);
                }
                else
                {
                    _points[i].SetActive(false);
                }
            }
        }

        private void CreateDefaultNumOfProjectionPoints()
        {
            _points = new GameObject[_pointNum];
            for (int i = 0; i < _pointNum; i++)
            {
                _points[i] = Instantiate(_pointPrefab, _bulletSpawnPosition.position, Quaternion.identity);
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
            var aimPosition = Camera.main.ScreenToWorldPoint(_crossHairObject.transform.position);
            aimPosition.z = 0f;
            return aimPosition;
        }

        public void Shoot()
        {
            if (_fireCoolDown.IsReady && BulletCount > 0)
            {
                var instance = SpawnUtils.Spawn(_bulletPrefab.gameObject, _bulletSpawnPosition.position);
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(_pistolDirection);
                _fireCoolDown.Reset();
                _session.Data.Inventory.Remove(BulletId, 1);
            }
        }

        private void OnDisable()
        {

            for (int i = 0; i < _points.Length; i++)
            {
                Destroy(_points[i]);
            }
        }
    }
}