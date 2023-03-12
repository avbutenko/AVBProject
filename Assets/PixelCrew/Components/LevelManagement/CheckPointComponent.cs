using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.PixelCrew.Components.GoBased;
using Assets.PixelCrew.Model;

namespace Assets.PixelCrew.Components.LevelManagement
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnChecked;

        public string ID => _id;
        private GameSession _session;
        [SerializeField] private SpawnComponent _heroSpawner;

        private void Start()
        {

            _session = GameSession.Instance;

            if (_session.IsChecked(_id))
                _setChecked?.Invoke();
            else
                _setUnChecked?.Invoke();
        }

        public void Check()
        {
            _session.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnHero()
        {
            _heroSpawner.Spawn();
        }
    }
}
