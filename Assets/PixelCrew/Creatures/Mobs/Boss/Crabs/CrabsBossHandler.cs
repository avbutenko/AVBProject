using Assets.PixelCrew.Components.Health;
using Assets.PixelCrew.Effects;
using Assets.PixelCrew.Utils.Disposables;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Mobs.Boss.Crabs
{
    public class CrabsBossHandler : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyWave[] _waves;
        [SerializeField] private SetPostEffectProfile _defEffect;
        [SerializeField] private SetPostEffectProfile _targetEffect;
        [SerializeField] private HealthComponent _hp;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private static readonly int SpawnKey = Animator.StringToHash("spawn");


        private void Awake()
        {
            _trash.Retain(_hp._onChange.Subscribe(OnHealthChanged));
            OnHealthChanged(_hp.Health);
        }

        private void OnHealthChanged(int health)
        {
            if (health > 21 && health < 26)
            {
                SetWave(0);
            }
            else if (health > 11 && health < 20)
            {
                SetWave(1);
            }
            else if (health > 0 && health < 10)
            {
                SetWave(2);
            }
        }

        private void SetWave(int counter)
        {
            if (_waves[counter].IsSpawned == false)
            {
                _animator.SetTrigger(SpawnKey);
                _waves[counter].Spawn();
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

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}