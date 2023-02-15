using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Utils.Disposables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryConroller : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidjet _prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private Image _backGround;
        private List<InventoryItemWidjet> _createdItems = new List<InventoryItemWidjet>();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _backGround = GetComponentInChildren<Image>();
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            Rebuild();
        }

        private void Rebuild()
        {
            var inventory = _session.QuickInventory.Inventory;

            //create required items
            for (var i = _createdItems.Count; i < inventory.Length; i++)
            {
                var item = Instantiate(_prefab, _container);
                _createdItems.Add(item);
            }

            //Update data and activate
            for (var i = 0; i < inventory.Length; i++)
            {
                _createdItems[i].SetData(inventory[i], i);
                _createdItems[i].gameObject.SetActive(true);
            }

            //hide unused items
            for (var i = inventory.Length; i < _createdItems.Count; i++)
            {
                _createdItems[i].gameObject.SetActive(false);
            }

            //hide background cover of quick inventory
            if (inventory.Length == 0)
            {
                _backGround.gameObject.SetActive(false);
            }
            else
            {
                _backGround.gameObject.SetActive(true);
            }

        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}

