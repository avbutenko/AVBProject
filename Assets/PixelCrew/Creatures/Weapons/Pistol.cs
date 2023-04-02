using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Weapons
{
    public class Pistol : MonoBehaviour
    {

        void Update()
        {
            Vector2 pistolPosition = transform.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - pistolPosition;
            transform.right = direction;
        }
    }
}