﻿using Assets.PixelCrew.Model;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.UI.Widjets;
using Assets.PixelCrew.Utils;
using System;
using System.Collections;
using UnityEngine;
using Assets.PixelCrew.Utils.Disposables;

namespace Assets.PixelCrew.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidjet _healthBar;
        [SerializeField] private CurrentPerkWidget _currentPerk;

        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHealthChanged));
            _trash.Retain(_session.Perks.Subscribe(OnPerkChanged));

            OnPerkChanged();
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
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        public void OnSettings()
        {
            WindowUtils.CreateWindow("UI/PauseWindow");
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }


    }
}