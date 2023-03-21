using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Components.Miscellaneous
{
    public class CameraFinder : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        void Start()
        {
            _canvas.worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }

    }
}