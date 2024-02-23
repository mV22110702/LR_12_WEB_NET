using System.Globalization;
using LR_12_WEB_NET.Enums;

namespace LR_12_WEB_NET.ApiClient;

/// <summary>
/// Helper class to work with currency slugs by ids.
/// </summary>
public static class CurrencySymbol
{
    /// <summary>
    /// Converts string symbols to enums
    /// </summary>
    /// <param name="symbols"></param>
    /// <returns></returns>
    ///<exception cref="ArgumentOutOfRangeException">Invalid currency symbol</exception>
    public static List<CurrencyId> SymbolsToIds(List<string> symbols)
    {
        return symbols.Select<string, CurrencyId>(SymbolToId).ToList();
    }

    /// <summary>
    /// Converts string symbol to enum
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Invalid currency symbol</exception>
    public static CurrencyId SymbolToId(string symbol)
    {
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        if (!Enum.GetNames<CurrencyId>().Contains(textInfo.ToTitleCase(symbol.ToLower())))
        {
            throw new ArgumentOutOfRangeException(nameof(symbol), symbol, "Invalid currency symbol");
        }

        Enum.TryParse(symbol, true, out CurrencyId currencyId);

        return currencyId;
    }

    /// <summary>
    /// Get list of symbols.
    /// <param name="ids">List of currency ids</param>
    /// <returns>List of symbols</returns>
    /// </summary>
    public static List<string> IdsToSymbols(List<CurrencyId> ids)
    {
        return ids.Select(id => IdToSymbolMap[id]).ToList();
    }

    /// <summary>
    /// Get list of id numbers.
    /// <param name="ids">List of currency ids</param>
    /// <returns>List of id numbers</returns>
    /// </summary>
    public static List<int> IdsToNumbers(List<CurrencyId> ids)
    {
        return ids.Select(id => id.GetHashCode()).ToList();
    }

    /// <summary>
    /// Converts list of numbers to list of enums
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Invalid currency id</exception>
    public static List<CurrencyId> NumbersToIds(List<int> ids)
    {
        return ids.Select(NumberToId).ToList();
    }

    /// <summary>
    /// Converts number to enum
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Invalid currency id</exception>
    public static CurrencyId NumberToId(int id)
    {
        Enum.TryParse<CurrencyId>(id.ToString(), out CurrencyId currencyId);
        if (!Enum.IsDefined<CurrencyId>(currencyId))
        {
            throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid currency id");
        }

        return currencyId;
    }

    /// <summary>
    /// Map of currency id to slug.
    /// </summary>
    public static readonly IDictionary<CurrencyId, string> IdToSymbolMap = new Dictionary<CurrencyId, string>()
    {
        [CurrencyId.Usd] = "USD",
        [CurrencyId.All] = "ALL",
        [CurrencyId.Dzd] = "DZD",
        [CurrencyId.Ars] = "ARS",
        [CurrencyId.Amd] = "AMD",
        [CurrencyId.Aud] = "AUD",
        [CurrencyId.Azn] = "AZN",
        [CurrencyId.Bhd] = "BHD",
        [CurrencyId.Bdt] = "BDT",
        [CurrencyId.Byn] = "BYN",
        [CurrencyId.Bmd] = "BMD",
        [CurrencyId.Bob] = "BOB",
        [CurrencyId.Bam] = "BAM",
        [CurrencyId.Brl] = "BRL",
        [CurrencyId.Bgn] = "BGN",
        [CurrencyId.Khr] = "KHR",
        [CurrencyId.Cad] = "CAD",
        [CurrencyId.Clp] = "CLP",
        [CurrencyId.Cny] = "CNY",
        [CurrencyId.Cop] = "COP",
        [CurrencyId.Crc] = "CRC",
        [CurrencyId.Hrk] = "HRK",
        [CurrencyId.Cup] = "CUP",
        [CurrencyId.Czk] = "CZK",
        [CurrencyId.Dkk] = "DKK",
        [CurrencyId.Dop] = "DOP",
        [CurrencyId.Egp] = "EGP",
        [CurrencyId.Eur] = "EUR",
        [CurrencyId.Gel] = "GEL",
        [CurrencyId.Ghs] = "GHS",
        [CurrencyId.Gtq] = "GTQ",
        [CurrencyId.Hnl] = "HNL",
        [CurrencyId.Hkd] = "HKD",
        [CurrencyId.Huf] = "HUF",
        [CurrencyId.Isk] = "ISK",
        [CurrencyId.Inr] = "INR",
        [CurrencyId.Idr] = "IDR",
        [CurrencyId.Irr] = "IRR",
        [CurrencyId.Iqd] = "IQD",
        [CurrencyId.Ils] = "ILS",
        [CurrencyId.Jmd] = "JMD",
        [CurrencyId.Jpy] = "JPY",
        [CurrencyId.Jod] = "JOD",
        [CurrencyId.Kzt] = "KZT",
        [CurrencyId.Kes] = "KES",
        [CurrencyId.Kwd] = "KWD",
        [CurrencyId.Kgs] = "KGS",
        [CurrencyId.Lbp] = "LBP",
        [CurrencyId.Mkd] = "MKD",
        [CurrencyId.Myr] = "MYR",
        [CurrencyId.Mur] = "MUR",
        [CurrencyId.Mxn] = "MXN",
        [CurrencyId.Mdl] = "MDL",
        [CurrencyId.Mnt] = "MNT",
        [CurrencyId.Mad] = "MAD",
        [CurrencyId.Mmk] = "MMK",
        [CurrencyId.Nad] = "NAD",
        [CurrencyId.Npr] = "NPR",
        [CurrencyId.Twd] = "TWD",
        [CurrencyId.Nzd] = "NZD",
        [CurrencyId.Nio] = "NIO",
        [CurrencyId.Ngn] = "NGN",
        [CurrencyId.Nok] = "NOK",
        [CurrencyId.Omr] = "OMR",
        [CurrencyId.Pkr] = "PKR",
        [CurrencyId.Pab] = "PAB",
        [CurrencyId.Pen] = "PEN",
        [CurrencyId.Php] = "PHP",
        [CurrencyId.Pln] = "PLN",
        [CurrencyId.Gbp] = "GBP",
        [CurrencyId.Qar] = "QAR",
        [CurrencyId.Ron] = "RON",
        [CurrencyId.Rub] = "RUB",
        [CurrencyId.Sar] = "SAR",
        [CurrencyId.Rsd] = "RSD",
        [CurrencyId.Sgd] = "SGD",
        [CurrencyId.Zar] = "ZAR",
        [CurrencyId.Krw] = "KRW",
        [CurrencyId.Ssp] = "SSP",
        [CurrencyId.Ves] = "VES",
        [CurrencyId.Lkr] = "LKR",
        [CurrencyId.Sek] = "SEK",
        [CurrencyId.Chf] = "CHF",
        [CurrencyId.Thb] = "THB",
        [CurrencyId.Ttd] = "TTD",
        [CurrencyId.Tnd] = "TND",
        [CurrencyId.Try] = "TRY",
        [CurrencyId.Ugx] = "UGX",
        [CurrencyId.Uah] = "UAH",
        [CurrencyId.Aed] = "AED",
        [CurrencyId.Uyu] = "UYU",
        [CurrencyId.Uzs] = "UZS",
        [CurrencyId.Vnd] = "VND",
    };
}