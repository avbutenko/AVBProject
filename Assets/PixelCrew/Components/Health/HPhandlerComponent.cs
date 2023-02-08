using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelCrew.Components.Health
{
    public class HPhandlerComponent : MonoBehaviour
    {
        [SerializeField] private int _deltaValue;

        private int _originalDeltaValue;

        private void Start()
        {
            _originalDeltaValue = _deltaValue;
        }
        public int GetOriginalDeltaValue()
        {
            return _originalDeltaValue;
        }

        public void SetDelta(int delta)
        {
            _deltaValue = delta;
        }
        public void ApplyChange(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            healthComponent?.ModifyHealth(_deltaValue);

        }
    }
}
