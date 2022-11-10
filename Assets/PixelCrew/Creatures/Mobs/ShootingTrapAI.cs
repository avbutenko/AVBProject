using Assets.PixelCrew.Components.ColliderBased;
using Assets.PixelCrew.Components.GoBased;
using Assets.PixelCrew.Utils;
using System;
using UnityEngine;
namespace Assets.PixelCrew.Components.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {

        [SerializeField] private LayerCheck _vision;

        [Header("Melee")]
        [SerializeField] private CoolDown _meleeCoolDown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private CoolDown _rangeCoolDown;
        [SerializeField] private SpawnComponent _rangeAtttack;
        [SerializeField] private SpawnComponent _rangeParticles;

        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack != null && _meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCoolDown.IsReady)
                    {
                        MeleeAttack();
                        return;
                    }
                }

                if (_rangeCoolDown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void RangeAttack()
        {
            _rangeCoolDown.Reset();
            _animator.SetTrigger(Range);
        }
        private void MeleeAttack()
        {
            _meleeCoolDown.Reset();
            _animator.SetTrigger(Melee);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAtttack.Spawn();
            _rangeParticles?.Spawn();
        }

    }
}

