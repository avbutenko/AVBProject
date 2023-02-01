using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.PixelCrew.UI.Widjets;
using Assets.PixelCrew.Model.Definitions.Repository;
using Assets.PixelCrew.Model;

namespace Assets.PixelCrew.UI
{
    public class PerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isLocked;
        [SerializeField] private GameObject _isUsed;
        [SerializeField] private GameObject _isSelected;

        private GameSession _session;
        private PerkDef _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateView();
        }
        public void SetData(PerkDef data, int index)
        {
            _data = data;

            if (_session != null)
                UpdateView();
        }

        private void UpdateView()
        {
            _icon.sprite = _data.Icon;
            _isUsed.SetActive(_session.Perks.IsUsed(_data.Id));
            _isSelected.SetActive(_session.Perks.InterfaceSelection.Value == _data.Id);
            _isLocked.SetActive(!_session.Perks.IsUnlocked(_data.Id));

        }

        public void OnSelect()
        {
            _session.Perks.InterfaceSelection.Value = _data.Id;
        }
    }

}