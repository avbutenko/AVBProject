using UnityEngine;
using UnityEngine.InputSystem;
using AVBProject.Creatures;
using Assets.PixelCrew.Components.Creatures;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Utils;

namespace Assets.PixelCrew.Components.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private MyHero _hero;

        private HeroInputAction _InputAction;
        private GameSession _session;

        private void Awake()
        {
            _session = GameSession.Instance;
            _InputAction = new HeroInputAction();
            _InputAction.Hero.Movement.performed += OnMovement;
            _InputAction.Hero.Movement.canceled += OnMovement;
        }

        private void OnEnable()
        {
            _InputAction.Enable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            _hero.SetDirection(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Attack();
            }
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.NextItem();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                WindowUtils.CreateWindow("UI/PauseWindow");
            }
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.StartThrowing();
            }

            if (context.canceled)
            {
                _hero.UseInventory();
            }
        }

        public void OnUsePerk(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.UsePerk();
        }

        public void OnToogleFlashlight(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.ToogleFlashlight();
        }
    }
}
