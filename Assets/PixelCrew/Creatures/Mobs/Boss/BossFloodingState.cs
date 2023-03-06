using Assets.PixelCrew.Creatures.Mobs.Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BossFloodingState : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var controller = animator.GetComponent<FloodController>();
        controller.StartFlooding();
    }


}
