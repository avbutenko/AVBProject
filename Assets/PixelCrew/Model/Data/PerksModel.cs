using System.Collections;
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
        public PerksModel(PlayerData data)
        {
            _data = data;
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResorces = _data.Inventory.isEnough(def.Price);

            if (isEnoughResorces)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);
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

        public void Dispose()
        {

        }
    }
}