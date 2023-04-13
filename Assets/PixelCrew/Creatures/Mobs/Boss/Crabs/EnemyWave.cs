using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.Creatures.Mobs.Boss.Crabs
{
    public class EnemyWave : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onSpawn;
        [SerializeField] private UnityEvent _allDead;
        public bool IsSpawned;
        private int _enemyNum;

        private void Update()
        {
            _enemyNum = gameObject.transform.childCount;
            if (_enemyNum == 0)
            {
                _allDead?.Invoke();
                Destroy(this);
            }
        }

        public void Spawn()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
            IsSpawned = true;
            _onSpawn?.Invoke();
        }

    }
}