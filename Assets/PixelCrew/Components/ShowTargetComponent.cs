using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelCrew.Components
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private ShowTargetController _controller;
        [SerializeField] private float _delay = 1f;
        private void OnValidate()
        {
            if (_controller == null)
                _controller = FindObjectOfType<ShowTargetController>();
        }

        public void ShowTarget()
        {
            _controller.SetPosition(_target.position);
            _controller.SetState(true);
            Invoke(nameof(MoveBack), _delay);
        }

        private void MoveBack()
        {
            _controller.SetState(false);
        }

    }
}

