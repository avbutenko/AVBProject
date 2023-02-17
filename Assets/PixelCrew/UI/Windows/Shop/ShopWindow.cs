using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.PixelCrew.UI.Widjets;
using Assets.PixelCrew.Model.Definitions.Repositories.Items;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Utils.Disposables;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Model.Data.Properties;
using Assets.PixelCrew.UI;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Definitions.Player;


namespace Assets.PixelCrew.UI
{
    public class ShopWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private ItemWidget _wallet;
        [SerializeField] private Text _info;
        [SerializeField] private Transform _shopContainer;
        [SerializeField] private ShopItemWidget _prefab;

        private DataGroup<ItemDef, ShopItemWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public ObservableProperty<string> InterfaceSelectedShopItem = new ObservableProperty<string>();
        private GameSession _session;
        private ItemDef[] _sellableItems;
        protected override void Start()
        {
            base.Start();
            _dataGroup = new DataGroup<ItemDef, ShopItemWidget>(_prefab, _shopContainer);
            _sellableItems = DefsFacade.I.Items.GetAllByTags(ItemTag.Sellable);
            _session = FindObjectOfType<GameSession>();
            _session.ShopModel.InterfaceSelectedShopItem.Value = _sellableItems[0].Id;

            _trash.Retain(_session.ShopModel.Subscribe(OnShopItemChanged));
            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));

            OnShopItemChanged();
        }

        private void OnShopItemChanged()
        {
            _dataGroup.SetData(_sellableItems);
        }

        private void OnBuy()
        {

        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

    }
}

