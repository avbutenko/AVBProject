using Assets.PixelCrew.UI.LevelsLoader;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.UI.Windows.LoadGameWindow
{
    public class LoadGameWindow : AnimatedWindow
    {
        public void LoadLevel(string levelName)
        {
            LevelLoader.Instance.LoadLevel(levelName);
        }
    }
}