﻿using Assets.PixelCrew.UI.LevelsLoader;
using Assets.PixelCrew.Utils;
using System;
using UnityEngine;

namespace Assets.PixelCrew.UI
{
    public class MainMenuWindow : AnimatedWindow
    {

        protected Action _closeAction;
        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel("Level2");
            };
            Close();
        }

        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}

