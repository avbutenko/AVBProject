using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AVBProject.Components
{
    public class StickyComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_tag))
            {
                collision.transform.SetParent(transform); ;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(_tag))
            {
                collision.transform.SetParent(null);
            }

        }
    }
}