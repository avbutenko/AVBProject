using Assets.PixelCrew.Model;
using Assets.PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.PixelCrew.UI
{
    public class PauseMenuWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();
            if (_playerInput != null)
                _playerInput.enabled = false;
            /*AudioListener.pause = true;*/
        }

        protected override void Start()
        {
            base.Start();
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnShowSettings()
        {
            //WindowUtils.CreateWindow("UI/SettingsWindow");
            //WindowUtils.CreateWindow("UI/ManagePerksWindow");
            //WindowUtils.CreateWindow("UI/PlayerStatsWindow");
            WindowUtils.CreateWindow("UI/ShopWindow");
            Close();
        }

        public void OnShowPerksWindow()
        {

        }

        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
            /*AudioListener.pause = false;*/
            if (_playerInput != null)
            {
                _playerInput.enabled = true;
            }

        }
    }
}

