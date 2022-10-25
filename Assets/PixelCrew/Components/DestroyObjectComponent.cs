using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AVBProject.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _ObjectToDestroy;
        public void DestroyObject()
        {
            Destroy(_ObjectToDestroy);
        }
    }
}
