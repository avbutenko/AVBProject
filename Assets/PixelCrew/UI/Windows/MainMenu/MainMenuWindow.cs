using Assets.PixelCrew.Utils;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            _closeAction = () => { SceneManager.LoadScene("Level2"); };
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

