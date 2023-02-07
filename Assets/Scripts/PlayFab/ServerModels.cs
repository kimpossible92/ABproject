using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

namespace SnakeMaze.PlayFab
{
    public class BaseServerResult
    {
        public bool isSuccess;
        public string error;
    }

    [Serializable]
    public class LoginDataResult : BaseServerResult
    {
        public LoginDataStructure loginData;
    }

    public class LoginDataStructure : BaseServerResult
    {
        public Dictionary<string, UserDataRecord> readOnlyData;
        public List<ItemInstance> inventory;
        public CurrencyData currency;
        public List<CatalogItem> catalog;
    }

    public class IntTestPlayFab : BaseServerResult
    {
        public int balance;
    }

    public class CurrencyData : BaseServerResult
    {
        public int softCoins;
        public int hardCoins;
    }

    public class SkinData : BaseServerResult
    {
        public ItemInstance[] data;
    }

    public class ErrorData
    {
        public PlayFabErrorCode errorCode;
    }

    public class ErrorDataFull
    {
        public PlayFabError error;
    }

    public class ErrorDataTest : ErrorData
    {
        public String nickname;
    }

    public class CustomData
    {
        public string SkinType;
    }

    public class ServerEconomy
    {
        public int PointToCoinRatio;
        public int CoinLooseRatioOnDeath;
    }

    public class PlayerVariables
    {
        public float NormalSpeed;
        public float BoostSpeed;
        public float TimeBetweenTicks;
        public float MinTimeBetweenTicks;
        public float SpeedChangeAmount;
        public float ChangeSpeedRate;
        
    }

    public class ItemInstanceData : BaseServerResult
    {
        public ItemInstance itemInstance;
    }

}