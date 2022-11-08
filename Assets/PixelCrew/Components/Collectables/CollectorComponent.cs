using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelCrew.Components.Collectables
{
    public class CollectorComponent : MonoBehaviour, ICanAddInInventory
    {
        [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();


        public void AddInInventory(string id, int value)
        {
            _items.Add(new InventoryItemData(id) { Value = value });
        }

        public void DropInInventory()
        {
            var session = FindObjectOfType<GameSession>();
            foreach (var item in _items)
            {
                session.Data.Inventory.Add(item.Id, item.Value);
            }

            _items.Clear();
        }
    }
}