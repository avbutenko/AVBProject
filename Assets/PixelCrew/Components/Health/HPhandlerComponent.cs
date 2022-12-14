using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelCrew.Components.Health
{
    public class HPhandlerComponent : MonoBehaviour
    {
        [SerializeField] private int _deltaValue;

        public void ApplyChange(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            healthComponent?.ModifyHealth(_deltaValue);

        }
    }
}
