using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.PixelCrew.UI.Widjets;
using Assets.PixelCrew.Utils.Disposables;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Definitions.Repository;
using Assets.PixelCrew.Model.Definitions.Localization;

namespace Assets.PixelCrew.UI
{
    public class ManagePerksWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private Text _info;
        [SerializeField] private Transform _perksContainer;
        [SerializeField] private PredefinedDataGroup<PerkDef, PerkWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;

        protected override void Start()
        {
            base.Start();
            _dataGroup = new PredefinedDataGroup<PerkDef, PerkWidget>(_perksContainer);
            _session = GameSession.Instance;

            _trash.Retain(_session.Perks.Subscribe(OnPerksChanged));
            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(_useButton.onClick.Subscribe(OnUse));
            OnPerksChanged();
        }
        public void OnPerksChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Perks.All);

            var selected = _session.Perks.InterfaceSelection.Value;

            _useButton.gameObject.SetActive(_session.Perks.IsUnlocked(selected));
            _useButton.interactable = _session.Perks.Used != selected;

            _buyButton.gameObject.SetActive(!_session.Perks.IsUnlocked(selected));
            _buyButton.interactable = _session.Perks.CanBuy(selected);

            var def = DefsFacade.I.Perks.Get(selected);
            _price.SetData(def.Price);

            _info.text = LocalizationManager.I.Localize(def.Info);

        }
        private void OnBuy()
        {
            var selected = _session.Perks.InterfaceSelection.Value;
            _session.Perks.Unlock(selected);
        }

        private void OnUse()
        {
            var selected = _session.Perks.InterfaceSelection.Value;
            _session.Perks.SelectPerk(selected);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }


    }
}