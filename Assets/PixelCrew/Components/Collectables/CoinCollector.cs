using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AVBProject.Creatures;
using Assets.PixelCrew.Components.Creatures.Hero;

namespace Assets.PixelCrew.Components.Collectables
{
    public class CoinCollector : MonoBehaviour
    {
        [SerializeField] private int _amount;

        public void Collect(GameObject target)
        {
            var heroComponent = target.GetComponent<MyHero>();
            heroComponent.CollectCoin(_amount);
        }
    }
}
