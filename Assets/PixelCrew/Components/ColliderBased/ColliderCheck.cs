using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.Components.ColliderBased
{
    public class ColliderCheck : LayerCheck
    {
        private Collider2D _collider;
        public UnityEvent _enterAction;
        public UnityEvent _exitAction;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
            if (_isTouchingLayer)
                _enterAction?.Invoke();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
            if (!_isTouchingLayer)
                _exitAction?.Invoke();
        }

    }
}