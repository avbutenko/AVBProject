﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Utils;
using Assets.PixelCrew.Utils.Disposables;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Model.Definitions.Player;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Data.Properties;


namespace Assets.PixelCrew.Model.Data
{
    public class ShopModel : IDisposable
    {
        public ObservableProperty<string> InterfaceSelectedShopItem = new ObservableProperty<string>();
        public event Action OnChanged;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private readonly PlayerData _data;

        public ShopModel(PlayerData data)
        {
            _data = data;
            _trash.Retain(InterfaceSelectedShopItem.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void Buy(string id)
        {
            var def = DefsFacade.I.Items.Get(id);
            var isEnoughResorces = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResorces)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Inventory.Add(id, 1);
                OnChanged?.Invoke();
            }
        }

        public void Dispose()
        {
            _trash.Dispose();
        }

    }
}

