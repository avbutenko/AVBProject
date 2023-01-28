using Assets.PixelCrew.Model.Data;
using Assets.PixelCrew.UI;
using Assets.PixelCrew.Utils;
using Assets.PixelCrew.Utils.Disposables;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.PixelCrew.Components.LevelManagement;
using System.Linq;

namespace Assets.PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;
        public PlayerData Data => _data;
        private PlayerData _save;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }

        private readonly List<string> _checkPoints = new List<string>();
        private void Awake()
        {

            var existingSession = GetExistingSession();

            if (existingSession != null)
            {
                existingSession.StartSession(_defaultCheckPoint);
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckPoint);
            }
        }

        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);
            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkPoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkPoints.Last();

            foreach (var checkPoint in checkPoints)
            {
                if (checkPoint.ID == lastCheckPoint)
                {
                    checkPoint.SpawnHero();
                    break;
                }

            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private GameSession GetExistingSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return gameSession;
            }

            return null;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            _trash.Dispose();
            InitModels();
        }
        public bool IsChecked(string id)
        {
            return _checkPoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (!_checkPoints.Contains(id))
            {
                Save();
                _checkPoints.Add(id);
            }

        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        private readonly List<string> _removedItems = new List<string>();
        public bool RestoreState(string Id)
        {
            return _removedItems.Contains(Id);
        }

        public void StoreState(string Id)
        {
            if (!_removedItems.Contains(Id))
                _removedItems.Add(Id);
        }
    }
}