using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Components.Miscellaneous
{
    public class UsableComponentExecutor : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        public void Use()
        {
            /*var instantiate = Instantiate(_prefab., _target.position, Quaternion.identity);

            var scale = _target.lossyScale;
            instantiate.transform.localScale = scale;
            instantiate.SetActive(true);*/
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}