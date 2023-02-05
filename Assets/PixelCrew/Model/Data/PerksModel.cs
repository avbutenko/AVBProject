﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Utils.Disposables;
using System;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Data.Properties;

namespace Assets.PixelCrew.Model.Data
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        public string Used => _data.Perks.Used.Value;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public event Action OnChanged;

        public bool IsDoubleJumpSupported => _data.Perks.Used.Value == "double-jump";
        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;

            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResorces = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResorces)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);
                OnChanged?.Invoke();
            }

        }

        public void UsePerk(string id)
        {
            _data.Perks.Used.Value = id;
        }
        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);

        }

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}