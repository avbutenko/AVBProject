﻿using UnityEngine;
using UnityEngine.InputSystem;
using AVBProject.Creatures;
using Assets.PixelCrew.Components.Creatures;

namespace Assets.PixelCrew.Components.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private MyHero _hero;

        private HeroInputAction _InputAction;

        private void Awake()
        {
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

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Throw();
            }
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Use();
            }
        }
    }
}
