using System;
using System.Collections.Generic;
using PlayFab.ClientModels;
using SnakeMaze.Enums;
using SnakeMaze.PlayFab;
using UnityEngine;
using Currency = SnakeMaze.Enums.Currency;

namespace SnakeMaze.SO.Items
{
    [CreateAssetMenu(fileName = "Catalog", menuName = "Scriptables/CatalogSO")]
    public class CatalogSO : ScriptableObject
    {
        [SerializeField] private List<AbstractSkinItemSO> catalogList;
        public List<AbstractSkinItemSO> CatalogList => catalogList;

        public void InitCatalog(LoginDataResult loginData, Action<LoginDataResult> onSuccess)
        {
            List<CatalogItem> serverCatalog = loginData.loginData.catalog;
            var dictionary = new Dictionary<string, CatalogItem>();
            foreach (var item in serverCatalog)
            {
                dictionary.Add(item.ItemId, item);
            }

            foreach (var item in catalogList)
            {
                var catalogItem = dictionary[item.ItemId];
                if(catalogItem.VirtualCurrencyPrices.ContainsKey("HC"))
                {
                    item.HasCurrencyType(Currency.HC);
                    item.SetPriceAndCurrency(
                        (int) catalogItem.VirtualCurrencyPrices[CurrencyUtils.CurrencyToString(Currency.HC)], Currency.HC);
                    
                }
                if(catalogItem.VirtualCurrencyPrices.ContainsKey("SC"))
                {
                    item.HasCurrencyType(Currency.SC);
                    item.SetPriceAndCurrency(
                        (int) catalogItem.VirtualCurrencyPrices[CurrencyUtils.CurrencyToString(Currency.SC)], Currency.SC);
                }
            }
            Debug.Log("Catalog Initiliazided");
            onSuccess(loginData);
        }
    }
}