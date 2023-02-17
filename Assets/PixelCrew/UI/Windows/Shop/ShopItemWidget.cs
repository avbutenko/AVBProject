using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.PixelCrew.UI;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Definitions.Repository;
using Assets.PixelCrew.UI.Widjets;
using Assets.PixelCrew.Model.Definitions.Repositories.Items;
using Assets.PixelCrew.Model.Definitions.Localization;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Definitions.Player;
using System.Globalization;

public class ShopItemWidget : MonoBehaviour, IItemRenderer<ItemDef>
{
    [SerializeField] private Text _name;
    [SerializeField] private Image _icon;
    [SerializeField] private ItemWidget _price;
    [SerializeField] private GameObject _selector;
    private GameSession _session;
    private ItemDef _data;
    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        UpdateView();
    }
    public void SetData(ItemDef data, int index)
    {
        _data = data;
        if (_session != null)
            UpdateView();
    }
    private void UpdateView()
    {
        _icon.sprite = _data.Icon;
        _name.text = LocalizationManager.I.Localize(_data.Id);
        _price.SetData(_data.Price);
        _selector.SetActive(_session.ShopModel.InterfaceSelectedShopItem.Value == _data.Id);
    }

    public void OnSelect()
    {
        _session.ShopModel.InterfaceSelectedShopItem.Value = _data.Id;
    }

}
