using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using SnakeMaze.PlayFab;
using SnakeMaze.SO.PlayFab;
using SnakeMaze.Utils;
using UnityEngine;

namespace SnakeMaze.SO.PlayFabManager
{
    [CreateAssetMenu(fileName = "PlayFabManager", menuName = "Scriptables/PlayFab/PlayFabManager")]
    public class PlayFabManagerSO : InitiableSO
    {
        [SerializeField] private bool isTest = true;
        [SerializeField] private BusServerCallSO busServerCallSo;
        private string _displayName;
        private bool _isLogged;

        public static string PLAYFAB_TITLE_ID_TEST = "E0CD8";
        public static string PLAYFAB_TITLE_RELEASE = "6F99B";

        public string DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }

        public override void InitScriptable()
        {
            SetupPlayFabServer();
        }

        #region LOGIN

        public void LoginSuccess() => _isLogged = true;
        private void SetupPlayFabServer()
        {
            PlayFabSettings.TitleId = isTest ? PLAYFAB_TITLE_ID_TEST : PLAYFAB_TITLE_RELEASE;
        }

        public void Login(Action<LoginResult> onSuccess, Action<PlayFabError> onFail)
        {
            var request = new LoginWithCustomIDRequest()
            {
                CreateAccount = true,
                CustomId = SystemInfo.deviceUniqueIdentifier,

                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };

            PlayFabClientAPI.LoginWithCustomID(request, onSuccess, onFail);
        }

        public void GetLoginData(Action<LoginDataResult> onSuccess, Action onFail)
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = nameof(GetLoginData)
            };

            PlayFabClientAPI.ExecuteCloudScript<LoginDataResult>(
                request,
                result =>
                {
                    LoginDataResult serverResponse = (LoginDataResult) result.FunctionResult;
                    
                    if(!serverResponse.isSuccess)
                    {
                        onFail();
                        _isLogged = false;
                    }
                    else
                    {
                        onSuccess(serverResponse);
                    }
                },
                error =>
                {
                    _isLogged = false;
                    
                });
        }

        #endregion

        #region TITLE_DATA

        public void GetTitleData(Action<GetTitleDataResult> onSuccess, Action<PlayFabError> onError)
        {
            if (!_isLogged) return;
            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), onSuccess, onError);
        }

        #endregion

        #region CREATE_ACCOUNT

        public void CreateAccount(string nickname, Action onSuccess, Action onFail)
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = nameof(CreateAccount),
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript<BaseServerResult>(
                request,
                result =>
                {
                    BaseServerResult serverResponse = (BaseServerResult) result.FunctionResult;
                    if(!serverResponse.isSuccess)
                    {
                        onFail();
                    }
                    else
                    {
                        onSuccess();
                    }
                },
                error =>
                {
                    onFail();
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.Error);
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.ErrorMessage);
                });
        }

        #endregion

        #region USER_DATA

        public void GetItemFromInventory(string itemId, Action<ItemInstance> onSuccess, Action onFail)
        {
            if (!_isLogged) return;
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = nameof(GetItemFromInventory),
                FunctionParameter = new {itemId = itemId},
            };
            PlayFabClientAPI.ExecuteCloudScript<ItemInstanceData>(
                request,
                result =>
                {
                    Debug.Log(result.FunctionResult);
                    ItemInstanceData serverResponse = (ItemInstanceData) result.FunctionResult;
                    if (!serverResponse.isSuccess)
                    {
                        Debug.LogWarning("Error getting item from inventory: " + serverResponse.error);
                        onFail();
                    }
                    else
                    {
                        Debug.Log("Getting item from inventory successful ");
                        onSuccess(serverResponse.itemInstance);
                        
                    }
                },
                error =>
                {
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.Error);
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.ErrorMessage);
                });
            
        }
        [ContextMenu("Test Score")]
        public void UpdateUserScore()
        {
            UpdateScore(50);
        }

        public void UpdateScore(int newHighScore)
        {
            if (!_isLogged) return;
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = nameof(UpdateScore),
                FunctionParameter = new {highScore = newHighScore},
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript<BaseServerResult>(
                request,
                result =>
                {
                    BaseServerResult serverResponse = (BaseServerResult) result.FunctionResult;
                    if(!serverResponse.isSuccess)
                        Debug.Log("Error updating score: " + serverResponse.error);
                    else
                        Debug.Log("Updating score successful ");
                },
                error =>
                {
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.Error);
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.ErrorMessage);
                    busServerCallSo.OnServerResponse?.Invoke();
                });
        }
        [ContextMenu("Test UpdateSkins")]
        public void UpdateSpaceSkins()
        {
            UpdateCurrentSkins("Astronaut","Space");
        }
        public void UpdateCurrentSkins(string snakeSkin, string mazeSkin)
        {
            if (!_isLogged) return;
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = nameof(UpdateCurrentSkins),
                FunctionParameter = new {snakeSkin = snakeSkin, mazeSkin = mazeSkin},
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript<BaseServerResult>(
                request,
                result =>
                {
                    BaseServerResult serverResponse = (BaseServerResult) result.FunctionResult;
                    if(!serverResponse.isSuccess)
                        Debug.Log("Error updating skins: " + serverResponse.error);
                    else
                        Debug.Log("Updating skins successfully");
                },
                error =>
                {
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.Error);
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.ErrorMessage);
                });
        }


        [ContextMenu("Test Gold")]
        public void UpdateUserGold()
        {
            AddSCCurrency(50, (x) => { });
        }

        public void AddSCCurrency(int newGold, Action<int> onSuccsess)
        {
            if (!_isLogged) return;
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = nameof(AddSCCurrency),
                FunctionParameter = new {amount = newGold},
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript<IntTestPlayFab>(
                request,
                result =>
                {
                    IntTestPlayFab serverResponse = (IntTestPlayFab) result.FunctionResult;
                    if(!serverResponse.isSuccess)
                    {
                        Debug.Log("Error updating gold: " + serverResponse.error);
                    }
                    else
                    {
                        onSuccsess(serverResponse.balance);
                        Debug.Log("PlayFab Total Gold: " + serverResponse.balance);
                    }
                },
                error =>
                {
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.Error);
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.ErrorMessage);
                });
        }

        public void UpdateUserInventoryItemCustomData(ItemInstance item, Action onSuccess)
        {
            if (!_isLogged) return;
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = nameof(UpdateUserInventoryItemCustomData),
                FunctionParameter = new {itemInstanceId = item.ItemInstanceId, itemId = item.ItemId},
                GeneratePlayStreamEvent = true
            };
            PlayFabClientAPI.ExecuteCloudScript<BaseServerResult>(
                request,
                result =>
                {
                    BaseServerResult serverResponse = (BaseServerResult) result.FunctionResult;
                    if (serverResponse.isSuccess)
                    {
                        Debug.Log($"CustomData of {item.ItemId} updated successfully ");
                        onSuccess();
                    }

                    else
                    {
                        Debug.Log($"Error updating CustomData of {item.ItemId} ");
                        Debug.Log("Error: " + serverResponse.error);
                        busServerCallSo.OnServerResponse?.Invoke();
                    }
                },
                error =>
                {
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.Error);
                    Debug.LogError("CUIDADO FUNCTION FAILED: " + error.ErrorMessage);
                    busServerCallSo.OnServerResponse?.Invoke();
                });
        }

        #endregion

        #region PURCHASES

        [ContextMenu("TestPurchase")]
        public void PurchaseDefaultSkins()
        {
            if (!_isLogged) return;
            PurchaseItem(Constants.SpaceMazeSkin, 5000, "SC", instances =>
                {
                    Debug.Log("Success!");
                    PurchaseItem(Constants.AstronautSnakeSkin, 5000, "SC", instances =>
                        {
                            Debug.Log("Success!");
                            busServerCallSo.OnServerResponse?.Invoke();
                        },
                        error =>
                        {
                            Debug.LogError(error.GenerateErrorReport());
                            busServerCallSo.OnServerResponse?.Invoke();
                        });
                },
                error =>
                {
                    Debug.LogError(error.GenerateErrorReport());
                    busServerCallSo.OnServerResponse?.Invoke();
                });
        }

        public void PurchaseItem(string item, int gold, string moneyType, Action<List<ItemInstance>> onSuccess,
            Action<PlayFabError> onFail)
        {
            if (!_isLogged) return;
            var request = new PurchaseItemRequest()
            {
                ItemId = item,
                Price = gold,
                VirtualCurrency = moneyType
            };
            busServerCallSo.OnServerCall?.Invoke();
            PlayFabClientAPI.PurchaseItem(
                request,
                result =>
                {
                    foreach (var item in result.Items)
                    {
                        UpdateUserInventoryItemCustomData(item, ()=>onSuccess(result.Items));
                    }
                },
                error =>
                {
                    onFail(error);
                    busServerCallSo.OnServerResponse?.Invoke();
                });
        }

        #endregion
    }
}