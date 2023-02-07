using SnakeMaze.PlayFab;
using UnityEngine;

namespace SnakeMaze.Utils
{
    public static class EconomyManager
    {
        private static  float _pointToCoinRatio = 100f;
        private static  float _coinLooseRatioOnDeath=2f;

        public static int SetCoinsFromPoint(bool hasWon, int points)
        {
            var ratio = hasWon ?  _pointToCoinRatio : _coinLooseRatioOnDeath * _pointToCoinRatio;
            var coins = (int) Mathf.Floor(points/ratio);
            return coins;
        }

        public static void SetRatios(ServerEconomy serverEconomy)
        {
            _pointToCoinRatio = serverEconomy.PointToCoinRatio;
            _coinLooseRatioOnDeath = serverEconomy.CoinLooseRatioOnDeath;
            Debug.Log("Ratios succesfully updated");
            Debug.Log("PointToCoinRatio: " + _pointToCoinRatio);
            Debug.Log("CoinLooseRatioOnDeath: " + _coinLooseRatioOnDeath);
        }
    }
}
