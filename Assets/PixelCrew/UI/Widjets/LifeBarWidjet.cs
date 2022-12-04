using Assets.PixelCrew.Components.Health;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.UI.Widjets
{
    public class LifeBarWidjet : MonoBehaviour
    {

        [SerializeField] private ProgressBarWidjet _lifeBar;
        [SerializeField] private HealthComponent _hp;

        private int _maxHp;

        private void Start()
        {
            if (_hp == null)
                _hp = GetComponentInParent<HealthComponent>();

            _maxHp = _hp.Health;

            _hp._onChange.AddListener(OnHpChanged);
            _hp._onDie.AddListener(OnDie);
        }

        private void OnDie()
        {
            Destroy(gameObject);
        }

        private void OnHpChanged(int hp)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            var progress = (float)hp / _maxHp;
            _lifeBar.SetProgress(progress);
        }

        private void OnDestroy()
        {
            _hp._onChange.RemoveListener(OnHpChanged);
            _hp._onDie.RemoveListener(OnDie);
        }

    }
}