using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.UI.Widjets;
using Assets.PixelCrew.Utils;
using System;
using System.Collections;
using UnityEngine;
using Assets.PixelCrew.Utils.Disposables;
using Assets.PixelCrew.Model.Definitions.Player;
using UnityEngine.InputSystem;

namespace Assets.PixelCrew.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidjet _healthBar;
        [SerializeField] public GameObject _crossHairObject;

        private CurrentPerkWidget _currentPerk;
        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private Vector3 _mousePosition;
        private void Start()
        {
            _currentPerk = FindObjectOfType<CurrentPerkWidget>();
            _session = GameSession.Instance;
            _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHealthChanged));
            _trash.Retain(_session.Perks.Subscribe(OnPerkChanged));

            OnPerkChanged();
        }

        private void Update()
        {
            _crossHairObject.transform.position = Mouse.current.position.ReadValue();
            //_mousePosition.z = 0f;
        }

        private void OnPerkChanged()
        {
            var usedPerkId = _session.Perks.Used;
            var hasPerk = !string.IsNullOrEmpty(usedPerkId);
            if (hasPerk)
            {
                var perkDef = DefsFacade.I.Perks.Get(usedPerkId);
                _currentPerk.Set(perkDef);
            }

            _currentPerk.gameObject.SetActive(hasPerk);
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        public void OnSettings()
        {
            WindowUtils.CreateWindow("UI/PauseWindow");
        }

        public void OnStudy()
        {
            WindowUtils.CreateWindow("UI/StudyWindow");
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public void OnStats()
        {
            WindowUtils.CreateWindow("UI/PlayerStatsWindow");
        }

        public void OnPerks()
        {
            WindowUtils.CreateWindow("UI/ManagePerksWindow");
        }

    }
}