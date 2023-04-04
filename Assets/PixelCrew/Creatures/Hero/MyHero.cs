using System.Collections;
using UnityEngine;
using Assets.PixelCrew.Utils;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Components.ColliderBased;
using Assets.PixelCrew.Components.Health;
using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.Components.GoBased;
using Assets.PixelCrew.Model.Definitions;
using Assets.PixelCrew.Model.Definitions.Repositories.Items;
using Assets.PixelCrew.Model.Definitions.Player;
using Assets.PixelCrew.Components.Creatures.Hero.Features;
using Assets.PixelCrew.Effects.CameraRelated;
using Assets.PixelCrew.Creatures.Hero.Features;

namespace Assets.PixelCrew.Components.Creatures.Hero
{

    public class MyHero : Creature, ICanAddInInventory
    {

        [SerializeField] private CheckCircleOverlap _interActionCheck;
        [SerializeField] private float _slamDownVelocity;

        [Space]
        [Header("Wall Slide & Jump Settings")]
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private float wallSlidingSpeed = 2f;
        [SerializeField] private float wallJumpingTime = 0.2f;
        [SerializeField] private float wallJumpingDuration = 0.4f;
        [SerializeField] private Vector2 wallJumpingPower = new Vector2(4f, 15f);
        private bool isWallJumping;
        private float wallJumpingDirection;
        private float wallJumpingCounter;

        [Space]
        [Header("Animator Settings")]
        [SerializeField] private RuntimeAnimatorController _armed;
        [SerializeField] private RuntimeAnimatorController _unarmed;

        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        [Space]
        [Header("Throw")]
        [SerializeField] private SpawnComponent _throwSpawner;
        [SerializeField] private CoolDown _throwCoolDown;

        [Space]
        [Header("Super throw")]
        [SerializeField] private CoolDown _superThrowCoolDown;
        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;

        [Space]
        [Header("Dash")]
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private float _dashPower = 24f;
        [SerializeField] private float _dashTime = 0.2f;

        [Space]
        [Header("Shield")]
        [SerializeField] private HeroShield _shield;

        [Space]
        [Header("Flashlight")]
        [SerializeField] private HeroFlashlight _flashlight;

        [Space]
        [Header("Invisibility")]
        [SerializeField] private HeroInvisibility _invis;

        [Space]
        [Header("Pistol")]
        [SerializeField] private GameObject _pistol;

        private bool _allowDoubleJump;
        private bool _isOnWall;
        private bool _isDashing;
        private bool _superThrow;
        private GameSession _session;
        private HealthComponent _health;
        private float _defaultGravityScale;
        private readonly CoolDown _speedUpCoolDown = new CoolDown();
        private float _additionalSpeed;
        private CameraShakeEffect _cameraShake;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

        private const string SwordId = "Sword";
        private const string PistolId = "pistol";
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
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _session = GameSession.Instance;
            _health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged; /*subscribe*/
            _session.Data.Inventory.OnChanged += InventoryLogHandler;

            _session.StatsModel.OnUpgraded += OnHeroUpgraded;

            _health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int)_session.StatsModel.GetValue(statId);
                    _session.Data.Hp.Value = health;
                    _health.SetHealth(health);
                    break;

            }
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

            if (id == PistolId)
                UpdatePistol();
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
            if (_isDashing) return;

            base.Update();

            /*            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
                        if (_wallCheck.IsTouchingLayer && moveToSameDirection)
                        {
                            _isOnWall = true;
                            Rigidbody.gravityScale = 0;
                        }
                        else
                        {
                            _isOnWall = false;
                            Rigidbody.gravityScale = _defaultGravityScale;
                        }*/

            WallSlide();
            WallJump();

            Animator.SetBool(IsOnWall, _isOnWall);
        }

        private void WallSlide()
        {
            if (_wallCheck.IsTouchingLayer && !IsGrounded && Direction.x != 0f)
            {
                _isOnWall = true;
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Mathf.Clamp(Rigidbody.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                _isOnWall = false;
            }
        }

        private void WallJump()
        {
            if (_isOnWall)
            {
                isWallJumping = false;
                wallJumpingCounter = wallJumpingTime;

                CancelInvoke(nameof(StopWallJumping));
            }
            else
            {
                wallJumpingCounter -= Time.deltaTime;
            }

            if (Direction.y > 0 && wallJumpingCounter > 0f && Direction.x * transform.lossyScale.x < 0)
            {
                isWallJumping = true;
                Rigidbody.velocity = new Vector2(Direction.x * wallJumpingPower.x, wallJumpingPower.y);
                wallJumpingCounter = 0f;
                UpdateSpriteDirection(Direction);
                Invoke(nameof(StopWallJumping), wallJumpingDuration);
            }
        }

        private void StopWallJumping()
        {
            isWallJumping = false;
        }


        protected override void FixedUpdate()
        {
            if (_isDashing) return;
            if (!isWallJumping)
            {
                base.FixedUpdate();
            }
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
                _session.Perks.CoolDown.Reset();
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
            _cameraShake?.Shake();
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinCount, 5);
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
            var hpModify = GetComponentInChildren<HPhandlerComponent>(); //to get damage delta value from AttackRange subobject
            var damageValue = hpModify.GetOriginalDeltaValue();
            damageValue = ModifyDamageByCrit(damageValue);
            hpModify.SetDelta(damageValue);
            base.Attack();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unarmed;
        }

        private void UpdatePistol()
        {
            Animator.runtimeAnimatorController = _unarmed;
            _pistol.SetActive(true);
        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
        public void OnDoThrow()
        {
            if (_superThrow && _session.Perks.IsSuperThrowSupported)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;

                var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
                _session.Perks.CoolDown.Reset();
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }

            _superThrow = false;
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");

            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
            _throwSpawner.SetPrefab(throwableDef.Projectile);

            var instance = _throwSpawner.SpawnInstance();
            ApplyRangeDamageStat(instance);

            _session.Data.Inventory.Remove(throwableId, 1);
        }

        private void ApplyRangeDamageStat(GameObject projectile)
        {
            var hpModify = projectile.GetComponent<HPhandlerComponent>();
            var damageValue = (int)_session.StatsModel.GetValue(StatId.RangeDamage);
            damageValue = ModifyDamageByCrit(damageValue);
            hpModify.SetDelta(-damageValue);
        }

        private int ModifyDamageByCrit(int damage)
        {
            var critChange = _session.StatsModel.GetValue(StatId.CriticalDamage);
            if (Random.value * 100 <= critChange)
            {
                return damage * 2;
            }

            return damage;
        }
        private void PerformThrowing()
        {
            if (!_throwCoolDown.IsReady || !CanThrow) return;

            if (_superThrowCoolDown.IsReady) _superThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCoolDown.Reset();
        }
        public void StartThrowing()
        {
            _superThrowCoolDown.Reset();
        }
        public void UseInventory()
        {
            if (IsSelectedItem(ItemTag.Throwable))
            {
                PerformThrowing();
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
                    _health.SetHealth(_session.Data.Hp.Value);
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

        public void UsePerk()
        {
            if (_session.Perks.IsShieldSupported)
            {
                _shield.Use();
                _session.Perks.CoolDown.Reset();
            }
            else if (_session.Perks.IsDashSupported)
            {
                StartCoroutine(Dash());
                _session.Perks.CoolDown.Reset();
            }
            else if (_session.Perks.IsInvisSupported)
            {
                _invis.Use();
                _session.Perks.CoolDown.Reset();
            }
        }

        private IEnumerator Dash()
        {
            _isDashing = true;
            Rigidbody.gravityScale = 0;
            Rigidbody.velocity = new Vector2(Direction.x * _dashPower, 0f);
            _trailRenderer.emitting = true;
            yield return new WaitForSeconds(_dashTime);

            _trailRenderer.emitting = false;
            Rigidbody.gravityScale = _defaultGravityScale;
            _isDashing = false;
        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        protected override float CalculateSpeed()
        {
            if (_speedUpCoolDown.IsReady)
                _additionalSpeed = 0f;

            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _additionalSpeed;
        }

        public void ToogleFlashlight()
        {
            var isActive = _flashlight.gameObject.activeSelf;
            _flashlight.gameObject.SetActive(!isActive);
        }

    }
}
