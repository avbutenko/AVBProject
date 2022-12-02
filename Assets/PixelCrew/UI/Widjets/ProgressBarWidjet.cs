using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.UI.Widjets
{
    public class ProgressBarWidjet : MonoBehaviour
    {
        [SerializeField] private Image _bar;

        public void SetProgress(float progress)
        {
            _bar.fillAmount = progress;
        }

    }
}