using SnakeMaze.Enums;
using SnakeMaze.Exceptions;
using SnakeMaze.Utils;
using UnityEngine;

namespace SnakeMaze.SO
{
    public abstract class AbstractSkinItemSO : InitiableSO
    {
        public abstract string ItemId { get; }
        
        public abstract FullPriceData ItemPriceData{get;set;}

        public abstract bool Available { get; set; }
        

        public void SetPriceAndCurrency(int price, Currency currency)
        {
            switch (currency)
            {
                case Enums.Currency.HC:
                    ItemPriceData.HardCoinsPriceData.Price = price;
                    break;
                case Enums.Currency.SC:
                    ItemPriceData.SoftCoinsPriceData.Price = price;
                    break;
                default:
                    throw new NotEnumTypeSupportedException();
            }
            
        }

        public abstract void HasCurrencyType(Currency currency);
    }
}
