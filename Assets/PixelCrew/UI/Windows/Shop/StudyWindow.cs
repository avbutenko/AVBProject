using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Utils;

namespace Assets.PixelCrew.UI
{
    public class StudyWindow : PauseMenuWindow
    {
        public void OnShowPerksWindow()
        {
            WindowUtils.CreateWindow("UI/ManagePerksWindow");
            Close();
        }

        public void OnShowStatsWindow()
        {
            WindowUtils.CreateWindow("UI/PlayerStatsWindow");
            Close();
        }

        public void OnShowShopWindow()
        {
            WindowUtils.CreateWindow("UI/ShopWindow");
            Close();
        }

    }
}

