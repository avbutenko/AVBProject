using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Utils
{
    public static class WindowUtils
    {
        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            /*var canvas = Object.FindObjectOfType<Canvas>(); // этот метод берёт первый попавшийся канвас и в него пихает меню паузы
            Object.Instantiate(window, canvas.transform);*/

            var canvases = Object.FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (canvas.CompareTag("HudCanvas"))
                {
                    Object.Instantiate(window, canvas.transform);
                    break;
                }
            }

        }

    }
}