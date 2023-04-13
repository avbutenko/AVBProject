using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Utils;
using Assets.PixelCrew.Components.Health;

namespace Assets.PixelCrew.Components.Creatures.Hero.Features
{
    public class HeroShield : MonoBehaviour
    {

        [SerializeField] protected HealthComponent _health;
        [SerializeField] private CoolDown _coolDown;

        public virtual void Use()
        {
            _health.Immune.Retain(this);
            _coolDown.Reset();
            gameObject.SetActive(true);
        }

        protected virtual void Update()
        {
            if (_coolDown.IsReady)
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _health.Immune.Release(this);
        }
    }
}
