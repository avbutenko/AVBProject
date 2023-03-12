using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.PixelCrew.Model.Definitions.Repository;
using Assets.PixelCrew.Model;

namespace Assets.PixelCrew.UI.Widjets
{
    public class CurrentPerkWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _coolDownImage;

        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
        }
        public void Set(PerkDef perkDef)
        {
            _icon.sprite = perkDef.Icon;
        }

        private void Update()
        {
            var coolDown = _session.Perks.CoolDown;
            _coolDownImage.fillAmount = coolDown.TimeLeft / coolDown.Value;
        }
    }
}
