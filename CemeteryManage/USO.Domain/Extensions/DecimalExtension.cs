
namespace USO.Domain.Extensions
{
    using System.Globalization;
    using System.Threading;

    public static class DecimalExtension
    {
        public static string PriceString4(this decimal? price, string currency, string defaultValue="0")
        {
            currency = currency ?? string.Empty;
            return price.GetValueOrDefault(0) != 0 ? string.Format("{0:0.0000} {1}", decimal.Round(price.Value, 4), currency) : defaultValue;
        }

        public static string PriceString4(this decimal price, string currency, string defaultValue = "0")
        {
            return price != 0 ? string.Format("{0:0.0000} {1}", decimal.Round(price, 4), currency) : defaultValue;
        }

        public static string PriceString2(this decimal? price, string currency, string defaultValue = "0")
        {
            currency = currency ?? string.Empty;
            return price.GetValueOrDefault(0) != 0 ? string.Format("{0:0.00} {1}", decimal.Round(price.Value, 2), currency) : defaultValue;
        }

        public static string PriceString2(this decimal price, string currency, string defaultValue = "0")
        {
            return price != 0 ? string.Format("{0:0.00} {1}", decimal.Round(price, 2), currency) : defaultValue;
        }

        public static string FormatCurrency(this decimal target, string currencyCode)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture("EN-US");
            if (new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).ISOCurrencySymbol.Equals(currencyCode))
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }
            if (currencyCode != "USD")
            {
                foreach (var info in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                {
                    if (new RegionInfo(info.LCID).ISOCurrencySymbol.Equals(currencyCode))
                    {
                        cultureInfo = info;
                    }
                }
            }

            var myCIclone = (CultureInfo)cultureInfo.Clone();
            const int decimalDigits = 2;

            myCIclone.NumberFormat.CurrencyDecimalDigits = decimalDigits;
            return string.Format(myCIclone, "{0:c}", new object[] { target });
        }

        public static decimal ConvertNullToZero(this decimal? instance)
        {
            if (instance == null)
                return 0;

            return instance.Value;
        }
    }
}
