using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Model;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.PixelCrew.Components.Creatures.Hero.Features
{
    public class HeroFlashlight : MonoBehaviour
    {
        [SerializeField] private float _consumePerSecond;
        [SerializeField] private Light2D _light;
        private GameSession _session;
        private float _defaultIntensity;
        private void Start()
        {
            _session = GameSession.Instance;
            _defaultIntensity = _light.intensity;
        }

        private void Update()
        {
            var consumed = Time.deltaTime * _consumePerSecond;
            var currentValue = _session.Data.Fuel.Value;
            var nextValue = currentValue - consumed;
            nextValue = Mathf.Max(nextValue, 0);
            _session.Data.Fuel.Value = nextValue;

            var progress = Mathf.Clamp(nextValue / 10, 0, 1);
            _light.intensity = _defaultIntensity * progress;

        }
    }
}

