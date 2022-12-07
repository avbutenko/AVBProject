using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.PixelCrew.Utils
{
    [Serializable]
    public class CoolDown
    {
        [SerializeField] private float _value;

        private float _timesUp;

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public void Reset()
        {
            _timesUp = Time.time + _value;
        }

        public float TimeLeft => Mathf.Max(_timesUp - Time.deltaTime, 0);

        public bool IsReady => _timesUp <= Time.time;
    }
}

