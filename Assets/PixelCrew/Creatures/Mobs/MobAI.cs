using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AVBProject.Components;
using System;
using Assets.PixelCrew.Components.ColliderBased;
using Assets.PixelCrew.Components.GoBased;
using Assets.PixelCrew.Components.Creatures.Mobs.Patrolling;
using Assets.PixelCrew.Utils;

namespace Assets.PixelCrew.Components.Creatures
{
    public class MobAI : MonoBehaviour
    {
        [Space]
        [Header("Checkers")]
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [Space]
        [Header("Projectiles")]
        [SerializeField] private int _projectileNum = 0;

        [Space]
        [Header("Cool Downs")]
        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _rangeAttackCoolDown = 1f;
        [SerializeField] private float _attackCoolDown = 1f;
        [SerializeField] private float _missCoolDown = 0.5f;

        private IEnumerator _current;
        private GameObject _target;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");

        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private Patrol _patrol;

        private bool _isDead;

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }
        public void OnTargetInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;
            StartState(AgrToTarget());
        }

        private IEnumerator AgrToTarget()
        {
            LookAtTarget();
            _particles.Spawn("Exclamation");

            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToTarget());

        }

        private void LookAtTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);

        }

        private IEnumerator GoToTarget()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else if (!_canAttack.IsTouchingLayer && _projectileNum > 0)
                {
                    StartState(RangeAttack());
                }
                else
                {

                    SetDirectionToTarget();
                }

                yield return null;
            }

            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("Miss");
            yield return new WaitForSeconds(_missCoolDown);

            StartState(_patrol.DoPatrol());
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCoolDown);
            }

            StartState(GoToTarget());
        }

        private IEnumerator RangeAttack()
        {
            _creature.RangeAttack();
            _projectileNum -= 1;
            yield return new WaitForSeconds(_rangeAttackCoolDown);

            StartState(GoToTarget());

        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
            {
                StopCoroutine(_current);
            }

            _current = coroutine;
            StartCoroutine(coroutine);
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);

            _creature.SetDirection(Vector2.zero);
            if (_current != null)
            {
                StopCoroutine(_current);
            }
        }
    }
}