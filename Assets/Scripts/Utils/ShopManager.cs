using System;
using System.Collections.Generic;
using PlayFab.ClientModels;
using SnakeMaze.Enums;
using SnakeMaze.SO;
using SnakeMaze.SO.UserDataSO;
using SnakeMaze.UI;
using SnakeMaze.User;
using UnityEngine;

namespace SnakeMaze.Utils
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private List<BuyButton> buyButtonList;
        [SerializeField] private List<SelectButton> selectButtonList;
        [SerializeField] private BusBuySkinSO buySkinSo;

        private void Start()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            foreach (var buyButton in buyButtonList)
            {
                buyButton.gameObject.SetActive(!buyButton.Item.Available);
            }
            foreach (var selectButton in selectButtonList)
            {
                selectButton.gameObject.SetActive(selectButton.Item.Available);
            }
        }

        private void SetSelectButtonActive(string itemId)
        {
            foreach (var selectButton in selectButtonList)
            {
                Debug.Log("Button :" + selectButton.MyButton);
                if (selectButton.Item.ItemId == itemId)
                {
                    selectButton.gameObject.SetActive(true);
                }
            }
        }

        private void OnEnable()
        {
            buySkinSo.OnBuySkin += SetSelectButtonActive;
        }

        private void OnDisable()
        {
            buySkinSo.OnBuySkin -= SetSelectButtonActive;
        }
    }
}
