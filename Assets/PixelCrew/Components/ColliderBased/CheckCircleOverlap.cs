﻿using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;
using Assets.PixelCrew.Utils;

namespace Assets.PixelCrew.Components.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {

        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;

        private readonly Collider2D[] _interactionResult = new Collider2D[10];
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TransparentRed;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
#endif
        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
               transform.position,
               _radius,
               _interactionResult,
               _mask);

            for (var i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTags)
                {
                    _onOverlap?.Invoke(overlapResult.gameObject);
                }

            }
        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {
        }


    }
}