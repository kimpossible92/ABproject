using SnakeMaze.Audio;
using SnakeMaze.SO;
using SnakeMaze.SO.FoodSO;
using SnakeMaze.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeMaze.UI
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject inGameHUDGroup;
        [SerializeField] private GameObject finishPanel;
        [SerializeField] private PlayerVariableSO player;
        [SerializeField] private TextMeshProUGUI points;
        [SerializeField] private TextMeshProUGUI finalScore;
        [SerializeField] private TextMeshProUGUI finalGold;
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject looseText;
        [SerializeField] private BusGameManagerSO gameManager;
        [SerializeField] private BusFoodSO busFoodSo;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button deathMenuButton;
        [SerializeField] private AudioRequest tapRequest;
        private bool _isFinishPanelActive;
        private bool _isPausePanelActive;

        private void Start()
        {
            _isFinishPanelActive = false;
            _isPausePanelActive = false;
            ResetPoints();
        }

        private void PressResumeButton()
        {
            if (!gameManager.GameStarted) return;
            tapRequest.PlayAudio();
            gameManager.PauseGame?.Invoke(false);
        }

        private void PressPauseButton()
        {
            if (!gameManager.GameStarted || _isFinishPanelActive || _isPausePanelActive) return;
            tapRequest.PlayAudio();
            gameManager.PauseGame?.Invoke(true);
        }

        private void SwitchPausePanel(bool pause)
        {
            _isPausePanelActive = pause;
            pausePanel.SetActive(pause);
        }

        private void PressMenuButton()
        {
            gameManager.EndGame?.Invoke();
        }

        private void ResetPoints()
        {
            player.Points = 0;
            points.text = player.Points.ToString();
        }

        private void AddPoints(int amount)
        {
            player.Points += amount;
            points.text = player.Points.ToString();
        }

        private void OnPlayerWin()
        {
            SwitchFinishPanel();
            OnWinSetFinalScore();
        }

        private void OnPlayerLoose()
        {
            SwitchFinishPanel();
            OnLooseSetFinalScore();
        }

        private void SwitchFinishPanel()
        {
            if (_isPausePanelActive) return;

            finishPanel.SetActive(!_isFinishPanelActive);
            _isFinishPanelActive = !_isFinishPanelActive;
        }

        /// <summary>
        /// Hides the ingame HUD and Sets the final score when the player wins.
        /// </summary>
        private void OnWinSetFinalScore()
        {
            inGameHUDGroup.SetActive(false);
            finalScore.text = player.Points.ToString();
            winText.SetActive(true);
            finalGold.text = EconomyManager.SetCoinsFromPoint(true, player.Points).ToString();
        }

        /// <summary>
        /// Hides the ingame HUD and sets the final score when the player loses.
        /// </summary>
        private void OnLooseSetFinalScore()
        {
            inGameHUDGroup.SetActive(false);
            finalScore.text = player.Points.ToString();
            looseText.SetActive(true);
            finalGold.text = EconomyManager.SetCoinsFromPoint(false, player.Points).ToString();
        }

        private void OnEnable()
        {
            gameManager.PlayerDeath += OnPlayerLoose;
            gameManager.WinGame += OnPlayerWin;
            gameManager.PauseGame += SwitchPausePanel;
            resumeButton.onClick.AddListener(PressResumeButton);
            pauseButton.onClick.AddListener(PressPauseButton);
            menuButton.onClick.AddListener(PressMenuButton);
            deathMenuButton.onClick.AddListener(PressMenuButton);
            busFoodSo.OnEatFoodPoints += AddPoints;
        }

        private void OnDisable()
        {
            gameManager.PlayerDeath -= OnPlayerLoose;
            gameManager.WinGame -= OnPlayerWin;
            gameManager.PauseGame -= SwitchPausePanel;
            resumeButton.onClick.RemoveListener(PressResumeButton);
            pauseButton.onClick.RemoveListener(PressPauseButton);
            menuButton.onClick.RemoveListener(PressMenuButton);
            deathMenuButton.onClick.RemoveListener(PressMenuButton);
            busFoodSo.OnEatFoodPoints -= AddPoints;
        }
    }
}