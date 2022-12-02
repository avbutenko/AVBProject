using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.UI.Widjets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelCrew.UI
{
    public class SettingsWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidjet _music;
        [SerializeField] private AudioSettingsWidjet _sfx;
        protected override void Start()
        {
            base.Start();
            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);
        }
    }
}
