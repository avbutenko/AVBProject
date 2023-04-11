using UnityEngine;
using Assets.PixelCrew.Utils;
using AVBProject.Creatures.Weapons;
using Assets.PixelCrew.Model;
using System.Collections;

namespace Assets.PixelCrew.Creatures.Weapons
{
    public class Pistol : MonoBehaviour
    {
        [Space]
        [Header("Settings")]
        [SerializeField] private Transform _ownerTransform;
        [SerializeField] private CoolDown _fireCoolDown;
        [SerializeField] private GameObject _crossHairObject;
        [SerializeField] private float _crossHairSpeed;
        [SerializeField] private float _aimBackTime = 2.5f;

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
        private Vector3 _aimPosition;
        private float _angle;

        private void Start()
        {
            _session = GameSession.Instance;
            CreateDefaultNumOfProjectionPoints();
        }

        private void OnEnable()
        {

            SetDefaults();
            _crossHairObject.SetActive(true);
        }

        private void Update()
        {
            HandleAiming();
        }

        private void SetDefaults()
        {
            _crossHairObject.transform.localPosition = new Vector3(_crossHairSpeed, 0, 0);
            _aimPosition = _crossHairObject.transform.position;
        }

        public void AimByMouse(Vector2 position)
        {
            _aimPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y));
            _aimPosition.z = 0f;
            _crossHairObject.transform.position = _aimPosition;
        }

        public void AimByStick(Vector2 position)
        {
            if (_ownerTransform.localScale.x < 0)
                position.x *= -1;

            if (position.magnitude > 0f)
            {
                _crossHairObject.transform.localPosition = position * _crossHairSpeed;
                _aimPosition = _crossHairObject.transform.position;
            }
            else
            {
                StartCoroutine(BacktoDefaults());
            }

        }

        private IEnumerator BacktoDefaults()
        {
            yield return new WaitForSeconds(_aimBackTime);
            SetDefaults();
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

        private void DrawProjection()
        {
            float maxDistance = Vector3.Distance(_bulletSpawnPosition.position, _aimPosition);

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
                _points[i].SetActive(false);
            }
        }

        private Vector2 GetPointPosition(float time)
        {
            Vector2 delta = _bulletPrefab.Speed * time * _pistolDirection;
            Vector2 position = (Vector2)_bulletSpawnPosition.position + delta;
            return position;
        }

        private void DisableAllPoints()
        {
            if (_points == null)
                return;

            for (int i = 0; i < _points.Length; i++)
            {
                if (_points[i] == null) break;
                _points[i].SetActive(false);
            }
        }

        private void HandleAiming()
        {
            _aimPosition = _crossHairObject.transform.position;
            UpdatePistoleAngle();
            UpdatePistolScale();
            DrawProjection();
        }

        private void UpdatePistoleAngle()
        {
            _pistolDirection = (_aimPosition - transform.position).normalized;
            _angle = Mathf.Atan2(_pistolDirection.y, _pistolDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, _angle);
        }
        private void UpdatePistolScale()
        {
            Vector3 pistolLocaleScale = Vector3.one;
            if (_angle > 90 || _angle < -90)
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

        private void OnDisable()
        {
            _crossHairObject.SetActive(false);
            SetDefaults();
            DisableAllPoints();
            StopAllCoroutines();
        }
    }
}