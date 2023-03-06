using Assets.PixelCrew.Components.GoBased;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.PixelCrew.Creatures.Mobs.Boss
{
    [RequireComponent(typeof(SpawnComponent))]
    public class BombController : MonoBehaviour
    {
        [Space]
        [Header("Bombing Waves Settings")]
        [SerializeField] private BombWave[] _waves;



        [Space]
        [Header("Random Spawner Settings")]
        [SerializeField] private SpawnComponent _spawner;
        [SerializeField] private int _fromX;
        [SerializeField] private int _toX;
        [SerializeField] private int _fromY;
        [SerializeField] private int _toY;

        private Coroutine _coroutine;

        [ContextMenu("StartBombing")]
        public void StartBombing()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(SpawnWave());
        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(_fromX, _toX), Random.Range(_fromY, _toY), 0);
        }

        private IEnumerator SpawnWave()
        {

            foreach (var wave in _waves)
            {
                for (int i = 0; i < wave.BombNum; i++)
                {
                    _spawner.SetTargetPosition(GetRandomPosition());
                    _spawner.Spawn();
                }
                yield return new WaitForSeconds(wave.Delay);
            }

            _coroutine = null;
        }

        [Serializable]
        public class BombWave
        {
            [SerializeField] private int _bombNum;
            [SerializeField] private float _delay;
            public int BombNum => _bombNum;
            public float Delay => _delay;

        }

    }
}