using Assets.PixelCrew.Components.GoBased;
using Assets.PixelCrew.Creatures.Mobs.Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBombState : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var bomber = animator.GetComponent<BombController>();
        bomber.StartBombing();
    }


}
