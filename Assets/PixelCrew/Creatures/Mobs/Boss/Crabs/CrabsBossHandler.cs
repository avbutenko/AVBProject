using Assets.PixelCrew.Components.Health;
using Assets.PixelCrew.Effects;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Mobs.Boss.Crabs
{
    public class CrabsBossHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] _waves;
        [SerializeField] private GameObject _shield;
        [SerializeField] private SetPostEffectProfile _defEffect;
        [SerializeField] private SetPostEffectProfile _targetEffect;
        [SerializeField] private HealthComponent _hp;

        private int _waveCounter;

        private void Update()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                _shield.SetActive(false);
            }
            else
            {
                _shield.SetActive(true);
            }
        }

        public void SetPostEffect(bool v)
        {
            switch (v)
            {
                case false:
                    _defEffect.Set();
                    break;
                case true:
                    _targetEffect.Set();
                    break;
            }
        }

        public void SpawnWave()
        {
            if (_waveCounter > _waves.Length) return;
            _waves[_waveCounter].SetActive(true);
            _waveCounter++;
        }
    }
}