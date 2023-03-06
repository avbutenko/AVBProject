using Assets.PixelCrew.Components.Creatures.Hero;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PixelCrew.Creatures.Hero
{
    public class InputEnableComponent : MonoBehaviour
    {
        private PlayerInput _input;
        private HeroInputReader _inputReader;

        private void Start()
        {
            _input = FindObjectOfType<PlayerInput>();
            _inputReader = FindObjectOfType<HeroInputReader>();
        }

        public void SetInput(bool isEnabled)
        {
            _input.enabled = isEnabled;
            _inputReader.enabled = isEnabled;
        }

    }
}