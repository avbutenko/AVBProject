using System;
using UnityEngine;
using Assets.PixelCrew.Utils;
using UnityEditor.Animations;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Components.ColliderBased;
using Assets.PixelCrew.Components.Health;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Components.GoBased;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Definitions.Repositories.Items;

namespace Assets.PixelCrew.Components.Creatures.Hero
{

    public class MyHero : Creature, ICanAddInInventory
    {

        [SerializeField] private CheckCircleOverlap _interActionCheck;
        [SerializeField] private LayerCheck _wallCheck;

        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private CoolDown _throwCoolDown;

        [Space]
        [Header("Animator Settings")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        [Space]
        [Header("Throw")]
        [SerializeField] private SpawnComponent _throwSpawner;

        private bool _allowDoubleJump;
        private bool _isOnWall;
        private GameSession _session;
        private HealthComponent _health;
        private float _defaultGravityScale;
        private readonly CoolDown _speedUpCoolDown = new CoolDown();
        private float _additionalSpeed;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

        private const string SwordId = "Sword";
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId) return SwordCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged; /*subscribe*/
            _session.Data.Inventory.OnChanged += InventoryLogHandler;


            _health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged; /*Unsubscribe*/
            _session.Data.Inventory.OnChanged -= InventoryLogHandler;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
        }

        private void InventoryLogHandler(string id, int value)
        {
            Debug.Log($"Inventory changed: {id} : {value}");
        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        protected override void Update()
        {
            base.Update();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }

            Animator.SetBool(IsOnWall, _isOnWall);
        }

        protected override float CalculateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;
            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump && _session.Perks.IsDoubleJumpSupported && !_isOnWall)
            {
                _allowDoubleJump = false;
                DoJumpVfx();
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (CoinCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Math.Min(CoinCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);


            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void Interact()
        {
            _interActionCheck.Check();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public override void Attack()
        {
            if (SwordCount <= 0) return;
            base.Attack();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unarmed;
        }

        public void OnPerformThrow()
        {
            Sounds.Play("Range");

            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
            _throwSpawner.SetPrefab(throwableDef.Projectile);
            _throwSpawner.Spawn();

            _session.Data.Inventory.Remove(throwableId, 1);
        }
        public void Throw()
        {
            if (_throwCoolDown.IsReady && CanThrow)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCoolDown.Reset();
            }

        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }

        public void UseInventory()
        {
            if (IsSelectedItem(ItemTag.Throwable))
            {
                Throw();
            }
            else if (IsSelectedItem(ItemTag.Potion))
            {
                UsePotion();
            }
        }

        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    _session.Data.Hp.Value += (int)potion.Value;
                    break;
                case Effect.SpeedUp:
                    _speedUpCoolDown.Value = _speedUpCoolDown.TimeLeft + potion.Time;
                    _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                    _speedUpCoolDown.Reset();
                    break;
            }

            Sounds.Play("Potion");
            _session.Data.Inventory.Remove(potion.Id, 1);
        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        protected override float CalculateSpeed()
        {
            if (_speedUpCoolDown.IsReady)
                _additionalSpeed = 0f;

            return base.CalculateSpeed() + _additionalSpeed;
        }

    }
}
