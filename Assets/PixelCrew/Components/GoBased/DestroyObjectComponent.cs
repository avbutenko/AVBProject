using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Model;

namespace Assets.PixelCrew.Components.GoBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private RestoreStateComponent _state;
        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
            if (_state != null)
                GameSession.Instance.StoreState(_state.Id);
        }

    }
}
