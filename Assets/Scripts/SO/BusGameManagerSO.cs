using System;
using SnakeMaze.Interfaces;
using UnityEngine;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "GameManagerSO", menuName = "Scriptables/BusSO/BusGameManagerSO")]
    public class BusGameManagerSO : ResseteableSO
    {
        public Action StartGame;
        public Action PlayerDeath;
        public Action EndGame;
        public Action<bool> PauseGame;
        public Action WinGame;
        private bool _gameStarted;
        private bool _gamePaused;

        public bool GamePaused
        {
            get => _gamePaused;
            set => _gamePaused = value;
        }



        public bool GameStarted
        {
            get => _gameStarted;
            set => _gameStarted = value;
        }

        private void OnEnable()
        {
            StartGame += SetGameStartedTrue;
            EndGame += SetGameStartedFalase;
            WinGame += SetGameStartedFalase;
            PauseGame += SetTimeScale;
        }

        private void OnDisable()
        {
            StartGame -= SetGameStartedTrue;
            EndGame -= SetGameStartedFalase;
            WinGame -= SetGameStartedFalase;
            PauseGame -= SetTimeScale;
        }

        public override void ResetValues()
        {
            _gameStarted = false;
            SetTimeScale(false);
        }

        private void SetTimeScale(bool paused)
        {
            Time.timeScale = paused ? 0 : 1;
            _gamePaused = paused;
        }
        private void SetGameStartedTrue() => _gameStarted = true;
        private void SetGameStartedFalase() => _gameStarted = false;
    }
}
