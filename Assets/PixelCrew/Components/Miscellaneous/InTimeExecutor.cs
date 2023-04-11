using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.Components.Miscellaneous
{
    public class InTimeExecutor : MonoBehaviour
    {
        [SerializeField] private float _timer;
        [SerializeField] private bool _executeOnAwake;
        [SerializeField] private UnityEvent _action;

        private void Awake()
        {
            if (_executeOnAwake)
                Execute();
        }

        public void Execute()
        {
            StartCoroutine(PerformExecution());
        }
        private IEnumerator PerformExecution()
        {
            yield return new WaitForSeconds(_timer);
            _action?.Invoke();
        }

    }
}
