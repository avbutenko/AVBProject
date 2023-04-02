using Assets.PixelCrew.UI.LevelsLoader;
using Assets.PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.UI.Windows.PlayWindow
{
    public class PlayWindow : AnimatedWindow
    {
        public void OnStartNewGame()
        {
            LevelLoader.Instance.LoadLevel("Level1");
        }

        public void OnLoadGame()
        {
            WindowUtils.CreateWindow("UI/LoadGameWindow");
        }

    }
}

