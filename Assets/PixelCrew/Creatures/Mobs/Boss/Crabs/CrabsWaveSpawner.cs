using Assets.PixelCrew.Effects;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Mobs.Boss.Crabs
{
    public class CrabsWaveSpawner : StateMachineBehaviour
    {
        private CrabsBossHandler _handler;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _handler = animator.GetComponent<CrabsBossHandler>();
            _handler.SpawnWave();
            _handler.SetPostEffect(true);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _handler.SetPostEffect(false);
        }

    }
}