using Assets.PixelCrew.Model;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.UI.Widjets
{
    public class FlashLightWidjet : MonoBehaviour
    {

        [SerializeField] private Image _coolDownImage;

        private GameSession _session;
        private float _capacity;

        private void Start()
        {
            _session = GameSession.Instance;
            _capacity = _session.Data.Fuel.Value;
        }

        private void Update()
        {
            _coolDownImage.fillAmount = 1 - _session.Data.Fuel.Value / _capacity;
        }
    }
}