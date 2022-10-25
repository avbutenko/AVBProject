using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AVBProject.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;

        public void ModifyHealth(int HealthDelta)
        {
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

        internal void SetHealth(int hp)
        {
            _health = hp;
        }
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {

    }
}
