using Assets.PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AVBProject.Components
{

    public class EnterTriggerComponent : MonoBehaviour
    {

        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0; //Everything
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (!other.gameObject.IsInLayer(_layer)) return;
            if (!string.IsNullOrEmpty(_tag) && !other.CompareTag(_tag)) return;

            _action?.Invoke(other.gameObject);
        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {
        }
    }
}