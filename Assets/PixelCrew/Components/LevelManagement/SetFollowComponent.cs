using System;
using UnityEngine;
using Cinemachine;
using Assets.PixelCrew.Components.Creatures.Hero;
using System.Collections;

namespace Assets.PixelCrew.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]

    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            vCamera.Follow = FindObjectOfType<MyHero>().transform;
        }
    }
}
