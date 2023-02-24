using UnityEngine;
using Assets.PixelCrew.Model;
using Assets.PixelCrew.UI.LevelsLoader;

namespace Assets.PixelCrew.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();
            session.Save();
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
        }
    }
}
