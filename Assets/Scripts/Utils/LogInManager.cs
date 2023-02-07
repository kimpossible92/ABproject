using System.Collections.Generic;
using SnakeMaze.SO.PlayFabManager;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using SnakeMaze.SO;
using TMPro;
using SnakeMaze.PlayFab;
using SnakeMaze.SO.UserDataSO;

namespace SnakeMaze.Utils
{
    public class LogInManager : MonoBehaviour
    {
        [SerializeField] private PlayFabManagerSO playFabManagerSo;
        [SerializeField] private EventSO logInEvent;
        [SerializeField] private EventSO playFabServerResponse;
        [SerializeField] private GameObject createAccountPanel;
        [SerializeField] private GameObject invalidUsernameText;
        [SerializeField] private GameObject duplicateErrorText;
        [SerializeField] private GameObject unknownErrorText;
        [SerializeField] private TextMeshProUGUI nickname;
        [SerializeField] private UserDataControllerSO userDataControllerSo;
        [SerializeField] private PlayerVariableSO playerVariableSo;

        private string _currentDisplayName;

        #region DATA

        public GameVersion gameVersion;
        // public EconomyModel serverEconomy;

        private void Start()
        {
            ServerLogin();
        }

        #endregion

        private void Awake()
        {
            logInEvent.CurrentAction += LoadServerData;
        }

        private void OnDestroy()
        {
            logInEvent.CurrentAction -= LoadServerData;
        }

        #region LOGIN

        private void ServerLogin()
        {
            playFabManagerSo.Login(OnLogginSuccess, OnLogginFailed);
        }

        private void OnLogginSuccess(LoginResult loginResult)
        {
            playFabManagerSo.LoginSuccess();
            Debug.Log("User login: " + loginResult.PlayFabId);
            Debug.Log("User newly created: " + loginResult.NewlyCreated);
            // Debug.Log("User Name: " + loginResult.InfoResultPayload.PlayerProfile.DisplayName);

            if (loginResult.NewlyCreated ||
                string.IsNullOrWhiteSpace(loginResult.InfoResultPayload.PlayerProfile.DisplayName))
            {
                createAccountPanel.SetActive(true);
            }
            else
            {
                logInEvent.CurrentAction?.Invoke();
                playFabManagerSo.DisplayName = loginResult.InfoResultPayload.PlayerProfile.DisplayName;
                userDataControllerSo.NickName = loginResult.InfoResultPayload.PlayerProfile.DisplayName;
                Debug.Log("User Name: " + loginResult.InfoResultPayload.PlayerProfile.DisplayName);
            }
        }

        public void CreateAccount()
        {
            invalidUsernameText.SetActive(false);
            duplicateErrorText.SetActive(false);
            unknownErrorText.SetActive(false);

            if (!CheckNickname(nickname.text)) return;

            playFabManagerSo.CreateAccount(_currentDisplayName,
                onSuccess: () =>
                {
                    SetNickname(_currentDisplayName);
                },
                onFail: () =>
                {
                    Debug.LogError("Create Account Failed!");
                }
            );
        }

        private void SetNickname(string value)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(
                new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = value
                },
                resultCallback: (result) =>
                {
                    logInEvent.CurrentAction?.Invoke();
                    createAccountPanel.SetActive(false);
                    playFabManagerSo.DisplayName = value;
                    userDataControllerSo.NickName = value;
                },
                errorCallback: (error) =>
                {
                    Debug.Log(error.Error);
                    Debug.Log(error.ErrorMessage);
                    Debug.Log(error.ErrorDetails);
                    Debug.LogWarning("Nickname error");
                    CreateAccountFailed(error.Error == PlayFabErrorCode.DuplicateUsername);
                }
            );
        }

        private bool CheckNickname(string value)
        {
            _currentDisplayName = value.Trim();
            if (_currentDisplayName.Length < 3 || _currentDisplayName.Length > 25)
            {
                invalidUsernameText.SetActive(true);
                return false;
            }

            return true;
        }

        private void CreateAccountFailed(bool unkown)
        {
            if (unkown)
                unknownErrorText.SetActive(true);
            else
                duplicateErrorText.SetActive(true);
        }

        private void OnLogginFailed(PlayFabError error)
        {
            playFabServerResponse.CurrentAction?.Invoke();
            Debug.LogError("Login failed: " + error.ErrorMessage);
        }

        #endregion


        #region LOAD_SERVER_DATA

        public void LoadServerData()
        {
            playFabManagerSo.GetTitleData(
                titleData => { LoadGameSetup(titleData.Data); },
                error => { Debug.LogError("Get title data failed: " + error.ErrorMessage); });
        }

        private void LoadGameSetup(Dictionary<string, string> data)
        {
            SetPlayFabVersion(data["GameVersion"]);
            SetPlayFabEconomyModel(data["EconomySetup"]);
            SetPlayerVariables(data["PlayerConfigData"]);

        }

        private void SetPlayFabVersion(string version) => JsonUtility.FromJsonOverwrite(version, gameVersion);

        private void SetPlayFabEconomyModel(string economyJson)
        {
            var serverEconomy = new ServerEconomy();
            JsonUtility.FromJsonOverwrite(economyJson, serverEconomy);
            EconomyManager.SetRatios(serverEconomy);
        }

        private void SetPlayerVariables(string playerJason)
        {
            if (string.IsNullOrEmpty(playerJason)) return;
            
            Debug.LogWarning(playerJason);
            
            var playerVariables = new PlayerVariables();
            JsonUtility.FromJsonOverwrite(playerJason, playerVariables);
            Debug.LogWarning(playerVariables.NormalSpeed);
            playerVariableSo.InitVariables(playerVariables);
        }

        #endregion
    }
}