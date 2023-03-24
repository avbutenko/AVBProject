using Assets.PixelCrew.Components.Creatures.Hero;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Definitions.Repositories.Items;
using Assets.PixelCrew.Utils.Disposables;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.UI.Hud.QuickInventory
{
    public class InventoryItemWidjet : MonoBehaviour
    {

        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _value;

        private MyHero _hero;
        private int _index;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            var session = GameSession.Instance;
            _hero = FindObjectOfType<MyHero>();
            var index = session.QuickInventory.SelectedIndex;
            _trash.Retain(index.SubscribeAndInvoke(OnIndexChanged));
        }

        private void OnIndexChanged(int newValue, int oldValue)
        {
            _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;

            var def = DefsFacade.I.Items.Get(item.Id);
            _icon.sprite = def.Icon;
            _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
        }

        public void OnClick()
        {
            if (_selection.activeSelf)
            {

                _hero?.UseInventory();
            }
            else
            {
                GameSession.Instance.QuickInventory.SetIndex(_index);
            }

        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}