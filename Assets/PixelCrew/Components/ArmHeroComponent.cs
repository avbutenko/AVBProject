using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AVBProject.Creatures;
using Assets.PixelCrew.Components.Creatures;

namespace Assets.PixelCrew.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        private MyHero _hero;
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<MyHero>();
            hero?.ArmHero();
        }

        

    }
}