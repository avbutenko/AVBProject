using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Utils;
using Assets.PixelCrew.Utils.ObjectPool;

namespace Assets.PixelCrew.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _usePool;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            SpawnInstance();
        }

        public GameObject SpawnInstance()
        {
            var targetPosition = _target.position;

            var instance = _usePool
                ? Pool.Instance.Get(_prefab, targetPosition)
                : SpawnUtils.Spawn(_prefab, targetPosition);

            var scale = _target.lossyScale;
            instance.transform.localScale = scale;
            instance.SetActive(true);

            return instance;
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }

        public void SetTargetPosition(Vector3 position)
        {
            _target.position = position;
        }
    }
}
