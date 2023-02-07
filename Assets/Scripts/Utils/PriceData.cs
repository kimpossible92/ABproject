using System;
using SnakeMaze.Enums;

namespace SnakeMaze.Utils
{
    [Serializable]
    public class FullPriceData 
    {
        private PriceData _softCoinsPriceData = new PriceData(Currency.SC);
        private PriceData _hardCoinsPriceData= new PriceData(Currency.HC);
        private bool _canBeBoughtWithHc;
        private bool _canBeBoughtWithSc;


        public PriceData SoftCoinsPriceData
        {
            get => _softCoinsPriceData;
            set => _softCoinsPriceData = value;
        }

        public PriceData HardCoinsPriceData
        {
            get => _hardCoinsPriceData;
            set => _hardCoinsPriceData = value;
        }

        public bool CanBeBoughtWithHc
        {
            get => _canBeBoughtWithHc;
            set => _canBeBoughtWithHc = value;
        }

        public bool CanBeBoughtWithSc
        {
            get => _canBeBoughtWithSc;
            set => _canBeBoughtWithSc = value;
        }
    }
    [Serializable]
    public class PriceData
    {
        public int Price;
        public Currency CurrencyType;

        public PriceData(Currency currency)
        {
            CurrencyType = currency;
        }
    }
}
