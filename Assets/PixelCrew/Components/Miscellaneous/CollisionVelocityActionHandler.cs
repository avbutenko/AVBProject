using Assets.PixelCrew.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.Components.Audio
{
    public class CollisionVelocityActionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private float _xVelocity;
        [SerializeField] private float _yVelocity;
        [SerializeField] private UnityEvent _action;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_layer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.x >= _xVelocity || contact.relativeVelocity.y >= _yVelocity)
                {
                    _action?.Invoke();
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_layer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.x >= _xVelocity || contact.relativeVelocity.y >= _yVelocity)
                {
                    _action?.Invoke();
                }
            }
        }
    }
}