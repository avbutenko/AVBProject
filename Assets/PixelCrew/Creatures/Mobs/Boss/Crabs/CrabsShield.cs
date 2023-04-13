using Assets.PixelCrew.Components.Creatures.Hero.Features;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Mobs.Boss.Crabs
{
    public class CrabsShield : HeroShield
    {
        public override void Use()
        {
            _health.Immune.Retain(this);
            gameObject.SetActive(true);
        }

        protected override void Update()
        {
            //do nothing
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}