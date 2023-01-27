using System;
using UnityEngine;
using Cinemachine;
using Assets.PixelCrew.Components.Creatures.Hero;

namespace Assets.PixelCrew.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]

    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            vCamera.Follow = GetComponent<MyHero>().transform;
        }
    }
}
