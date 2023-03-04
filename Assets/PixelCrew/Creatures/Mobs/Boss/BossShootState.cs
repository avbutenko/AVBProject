using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelCrew.Components.GoBased;

namespace Assets.PixelCrew.Creatures.Mobs.Boss
{
    public class BossShootState : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.LaunchProjectiles();
        }
    }
}


