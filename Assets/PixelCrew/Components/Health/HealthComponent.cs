using Assets.PixelCrew.UI.Widjets;
using Assets.PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] public HealthChangeEvent _onChange;
        private Lock _immune = new Lock();

        public int Health => _health;
        public Lock Immune => _immune;

        public void ModifyHealth(int HealthDelta)
        {
            if (HealthDelta < 0 && _immune.IsLocked) return;
            if (_health <= 0) return;

            _health += HealthDelta;
            _onChange?.Invoke(_health);

            if (HealthDelta < 0)
            {
                _onDamage?.Invoke();
            }

            if (HealthDelta > 0)
            {
                _onHeal?.Invoke();
                Debug.Log($"+{HealthDelta}HP. Total Health is : {_health}HP");
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Update Health Manually")]
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif

        public void SetHealth(int hp)
        {
            _health = hp;
        }

        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}
