using System;
using SnakeMaze.PlayFab;
using SnakeMaze.SO;
using SnakeMaze.SO.Items;
using SnakeMaze.SO.PlayFabManager;
using SnakeMaze.SO.UserDataSO;
using SnakeMaze.Utils;
using UnityEngine;

namespace SnakeMaze.User
{
    public class UserManager : MonoBehaviour
    {
        [SerializeField] private EventSO logInEvent;
        [SerializeField] private EventSO playFabServerResponse;
        [SerializeField] private UserDataControllerSO userDataControllerSo;
        [SerializeField] private UserInventorySO userInventorySo;
        [SerializeField] private CatalogSO catalogSo;
        [SerializeField] private PlayFabManagerSO playFabManager;
        [SerializeField] private BusGameManagerSO busGameManagerSo;
        [SerializeField] private PlayerVariableSO player;


        private void Awake()
        {
            logInEvent.CurrentAction += OnServerLogin;
        }

        private void OnDestroy()
        {
            logInEvent.CurrentAction -= OnServerLogin;
        }

        private void OnServerLogin()
        {
            playFabManager.GetLoginData(
                loginData =>
                {
                    LoadUserData(loginData);
                    LoadCatalog(loginData, LoadUserInventory);
                    Debug.Log("Server HighScore: " + loginData.loginData.readOnlyData["HighScore"].Value);
                },
                () => { playFabServerResponse.CurrentAction?.Invoke(); });
        }

        private void LoadCatalog(LoginDataResult loginData, Action<LoginDataResult> onSuccess)
        {
            catalogSo.InitCatalog(loginData, onSuccess );
        }

        private void LoadUserInventory(LoginDataResult loginData)
        {
            userInventorySo.LoadInventory(loginData);
        }

        private void LoadUserData(LoginDataResult loginData)
        {
            userDataControllerSo.LoadData(loginData);
        }

        private void OnWinUpdate()
        {
            UpdateScoreData();
            UpdateCurrencyData(true);
        }

        private void OnLooseUpdate()
        {
            Debug.LogWarning("UPDATING SCORE");
            UpdateScoreData();
            UpdateCurrencyData(false);
        }

        private void UpdateScoreData()
        {
            var points = player.Points;
            Debug.Log("local points: " + userDataControllerSo.HighScore);
            Debug.Log("new socre: " + points);
            if (points <= userDataControllerSo.HighScore) return;

            playFabManager.UpdateScore(player.Points);
            userDataControllerSo.HighScore = points;
            Debug.Log(" userDataControllerSo.HighScore: " + userDataControllerSo.HighScore);
            Debug.Log("Score updated");
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        private void UpdateCurrencyData(bool hasWon)
        {
            var coins = EconomyManager.SetCoinsFromPoint(hasWon, player.Points);
            userDataControllerSo.SoftCoins += coins;
            playFabManager.AddSCCurrency( coins, CheckCurrency );
            Debug.Log("Coins received: " + coins);
            Debug.Log("New coins: " + userDataControllerSo.SoftCoins);
            
        }

        private void CheckCurrency(int gold)
        {
            if (userDataControllerSo.SoftCoins == gold) return;
            
            Debug.Log("Local gold not equal to server, updating local");
            userDataControllerSo.SoftCoins = gold;
            
        }


        private void OnEnable()
        {
            busGameManagerSo.PlayerDeath += OnLooseUpdate;
            busGameManagerSo.WinGame += OnWinUpdate;
        }

        private void OnDisable()
        {
            busGameManagerSo.PlayerDeath -= OnLooseUpdate;
            busGameManagerSo.WinGame -= OnWinUpdate;
        }
    }
}