using Assets.PixelCrew.Components.Creatures.Hero;
using Assets.PixelCrew.Model.Definitions;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _value;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<MyHero>();
            if (hero != null)
                hero.AddInInventory(_id, _value);
        }
    }
}