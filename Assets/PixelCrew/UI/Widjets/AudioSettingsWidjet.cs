using Assets.PixelCrew.Model.Data.Properties;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.UI.Widjets
{
    public class AudioSettingsWidjet : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _value;

        private FloatPersistentProperty _model;

        private void Start()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void SetModel(FloatPersistentProperty model)
        {
            _model = model;
            model.OnChanged += OnValueChanged;
            OnValueChanged(model.value, model.value);
        }

        private void OnSliderValueChanged(float value)
        {
            _model.value = value;
        }

        private void OnValueChanged(float newValue, float oldValue)
        {
            var textValue = Mathf.Round(newValue * 100);
            _value.text = textValue.ToString();
            _slider.normalizedValue = newValue;
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            _model.OnChanged -= OnValueChanged;
        }

    }
}