using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Definitions.Repository;

namespace Assets.PixelCrew.UI
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        public virtual void SetData(ItemWithCount price)
        {
            var def = DefsFacade.I.Items.Get(price.ItemId);
            _icon.sprite = def.Icon;
            _value.text = price.Count.ToString();
        }
    }
}
