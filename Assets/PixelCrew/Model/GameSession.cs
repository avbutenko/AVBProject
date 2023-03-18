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
using Assets.PixelCrew.Model.Definitions.Player;
using System.Diagnostics;

namespace Assets.PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;

        public static GameSession Instance { get; private set; }
        public PlayerData Data => _data;
        private PlayerData _save;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }
        public PerksModel Perks { get; private set; }
        public StatsModel StatsModel { get; private set; }
        public ShopModel ShopModel { get; private set; }

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
                Instance = this;
                StartSession(_defaultCheckPoint);
            }
        }

        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);
            LoadUIs();
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

            Perks = new PerksModel(_data);
            _trash.Retain(Perks);

            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);

            _data.Hp.Value = (int)StatsModel.GetValue(StatId.Hp);

            ShopModel = new ShopModel(_data);
            _trash.Retain(ShopModel);
        }

        private void LoadUIs()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
            LoadScreenControls();
        }

        [Conditional("USE_ON_SCREEN_CONTROLS")]
        private void LoadScreenControls()
        {
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
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
            if (Instance == this)
                Instance = null;
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