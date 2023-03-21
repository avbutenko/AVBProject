using Assets.PixelCrew.Components.LevelManagement;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.PixelCrew.UI.Windows.GameOver
{
    public class GameOverWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();
            if (_playerInput != null)
                _playerInput.enabled = false;
        }

        protected override void Start()
        {
            base.Start();
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");
            var session = GameSession.Instance;
            Destroy(session.gameObject);
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;

            if (_playerInput != null)
            {
                _playerInput.enabled = true;
            }

        }
    }
}



