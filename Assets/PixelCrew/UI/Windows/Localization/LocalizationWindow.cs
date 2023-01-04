using Assets.PixelCrew.Model.Definitions.Localization;
using Assets.PixelCrew.UI.Hud.Dialogs;
using Assets.PixelCrew.UI.Widjets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelCrew.UI.Windows.Localization
{
    public class LocalizationWindow : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private LocaleItemWidjet _prefab;

        private string[] _supportedLocales = new[] { "en", "ru", "es" };
        private DataGroup<LocaleInfo, LocaleItemWidjet> _dataGroup;
        protected override void Start()
        {
            base.Start();
            _dataGroup = new DataGroup<LocaleInfo, LocaleItemWidjet>(_prefab, _container);
            _dataGroup.SetData(ComposeData());
        }

        private List<LocaleInfo> ComposeData()
        {
            var data = new List<LocaleInfo>();
            foreach (var locale in _supportedLocales)
            {
                data.Add(new LocaleInfo { LocaleId = locale });
            }

            return data;
        }

        public void OnSelected(string selectedLocale)
        {
            LocalizationManager.I.SetLocale(selectedLocale);
        }

    }
}