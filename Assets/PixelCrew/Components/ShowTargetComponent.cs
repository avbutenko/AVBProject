using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.Components
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private ShowTargetController _controller;
        [SerializeField] private float _delay = 1f;
        [SerializeField] private UnityEvent _onDelay;

        private Coroutine _coroutine;

        private void OnValidate()
        {
            if (_controller == null)
            {
                _controller = FindObjectOfType<ShowTargetController>();
            }
        }

        public void Play()
        {
            _controller.SetPosition(_target.position);
            _controller.SetState(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(WaitAndReturn());
        }

        private IEnumerator WaitAndReturn()
        {
            yield return new WaitForSeconds(_delay);

            _onDelay?.Invoke();
            _controller.SetState(false);
        }

    }
}

