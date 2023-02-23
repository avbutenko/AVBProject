﻿using System;
using System.Collections;
using UnityEngine;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Utils.Disposables;
using Assets.PixelCrew.Model.Definitions.Player;
using Assets.PixelCrew.Model.Data.Properties;

namespace Assets.PixelCrew.Effects.CameraRelated
{
    [RequireComponent(typeof(Animator))]
    public class BloodSplashOverlay : MonoBehaviour
    {

        [SerializeField] private Transform _overlay;
        private Animator _animator;
        private Vector3 _overScale;
        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private static readonly int Health = Animator.StringToHash("Health");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _overScale = _overlay.localScale - Vector3.one;
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHpChanged));
        }

        private void OnHpChanged(int newValue, int oldValue)
        {
            var maxHp = _session.StatsModel.GetValue(StatId.Hp);
            var hpNormalized = newValue / maxHp;
            _animator.SetFloat(Health, hpNormalized);

            var overlayModifier = Mathf.Max(hpNormalized - 0.3f, 0f);
            _overlay.localScale = Vector3.one + _overScale * overlayModifier;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}

