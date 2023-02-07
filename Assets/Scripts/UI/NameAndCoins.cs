using System;
using SnakeMaze.Enums;
using SnakeMaze.SO;
using SnakeMaze.SO.UserDataSO;
using TMPro;
using UnityEngine;

namespace SnakeMaze.UI
{
    public class NameAndCoins : MonoBehaviour
    {
        [SerializeField] private UserDataControllerSO userDataControllerSo;
        [SerializeField] private EventSO updateCoins;
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private TextMeshProUGUI softCoins;
        [SerializeField] private TextMeshProUGUI hardCoins;

        void Start()
        {
            displayName.text = userDataControllerSo.NickName;
            UpdateCoins();
        }

        public void UpdateCoins()
        {
           softCoins.text = "x" + userDataControllerSo.SoftCoins.ToString();
           hardCoins.text = "x" + userDataControllerSo.HardCoins.ToString();
        }

        private void OnEnable()
        {
            updateCoins.CurrentAction += UpdateCoins;
        }
        private void OnDisable()
        {
            updateCoins.CurrentAction -= UpdateCoins;
        }
    }
}
