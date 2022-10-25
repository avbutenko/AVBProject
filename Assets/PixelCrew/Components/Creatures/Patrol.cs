using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Components.Creatures
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}