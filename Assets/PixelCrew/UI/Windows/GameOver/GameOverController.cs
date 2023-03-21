using Assets.PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.UI.Windows.GameOver
{
    public class GameOverController : MonoBehaviour
    {

        public void Show()
        {
            WindowUtils.CreateWindow("UI/GameOverWindow");
        }
    }
}