using System;
using SnakeMaze.Enums;
using SnakeMaze.Utils;
using UnityEngine;

namespace SnakeMaze.SO.Items
{
    [CreateAssetMenu(fileName = "SnakeSkinItem", menuName = "Scriptables/SnakeSkinItemSO")]
    public class SnakeSkinItemSO : AbstractSkinItemSO
    {
        [SerializeField] private SnakeSkinSO currentSkin;
        [SerializeField] private string itemId;

        private bool _available;
        private FullPriceData _fullPriceData = new FullPriceData();

        public SnakeSkinSO Item => currentSkin;


        public override string ItemId => itemId;

        public override FullPriceData ItemPriceData
        {
            get=>_fullPriceData;
            set=>_fullPriceData=value;
        }


        public override bool Available
        {
            get => _available;
            set => _available = value;
        }

        public override void HasCurrencyType(Currency currency)
        {
            switch (currency)
            {
                case Currency.HC:
                    ItemPriceData.CanBeBoughtWithHc = true;
                    break;
                case Currency.SC:
                    ItemPriceData.CanBeBoughtWithSc = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currency), currency, null);
            }
        }

        public override void InitScriptable()
        {
            Available = itemId==Constants.DefaultSnakeSkin;
        }
    }
}