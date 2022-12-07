using Assets.PixelCrew.Components.Creatures.Hero;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Definitions.Repositories.Items;
using Assets.PixelCrew.Utils;
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
            var hero = go.GetInterface<ICanAddInInventory>();
            hero?.AddInInventory(_id, _value);
        }
    }
}