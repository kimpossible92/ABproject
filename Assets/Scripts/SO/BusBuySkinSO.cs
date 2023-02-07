using System;
using SnakeMaze.Enums;
using SnakeMaze.UI;
using UnityEngine;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "BusBuySkin", menuName = "Scriptables/BusSO/BusBuySkinSO")]
    public class BusBuySkinSO : ScriptableObject
    {
        public Action<string> OnBuySkin;
        public Action<int, Currency> OnBuySkinPrice;
    }
}
