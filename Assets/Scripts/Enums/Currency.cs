using UnityEngine;

namespace SnakeMaze.Enums
{
    public enum Currency
    {
        HC, SC
    }

    public static class CurrencyUtils
    {
        public static Currency StringToCurrency(string value)
        {
            Currency result;
            switch (value)
            {
                case "HC":
                    result = Currency.HC;
                    break;
                default:
                    result = Currency.SC;
                    break;
            }

            return result;
        }
        public static string CurrencyToString(Currency value)
        {
            var result = value switch
            {
                Currency.HC => "HC",
                Currency.SC => "SC",
                _ => throw new Exceptions.NotEnumTypeSupportedException()
            };

            return result;
        }
    }
}
