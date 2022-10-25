using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AVBProject.Creatures;
using Assets.PixelCrew.Components.Creatures.Hero;

namespace Assets.PixelCrew.Components.Collectables
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