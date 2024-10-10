using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HighBoard.Domain.Helpers;
public static partial class StringHelper
{
    public static string ClearForHtml(this string str) => CleanHtmlRegex().Replace(str, string.Empty).Trim();


    /// <summary>
    /// Enum olarak verilen parametrenin Description değerini döndürür.
    /// </summary>
    /// <param name="e">Enum</param>
    /// <returns>string</returns>
    public static string GetEnumDescription(this Enum e) => e.GetType().GetMember(e.ToString()).FirstOrDefault()!.GetCustomAttribute<DescriptionAttribute>()!.Description;

    /// <summary>
    /// string olarak verilen parametrenin değerini SEO'ya (Arama Motoru Optimazsyonu) uygun olarak dönüştürerek geri döndürür.
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static string ToStringForSeo(this string inputString)
    {
        inputString = inputString.Replace("Ç", "c");
        inputString = inputString.Replace("ç", "c");
        inputString = inputString.Replace("Ğ", "g");
        inputString = inputString.Replace("ğ", "g");
        inputString = inputString.Replace("I", "i");
        inputString = inputString.Replace("ı", "i");
        inputString = inputString.Replace("İ", "i");
        inputString = inputString.Replace("i", "i");
        inputString = inputString.Replace("Ö", "o");
        inputString = inputString.Replace("ö", "o");
        inputString = inputString.Replace("Ş", "s");
        inputString = inputString.Replace("ş", "s");
        inputString = inputString.Replace("Ü", "u");
        inputString = inputString.Replace("ü", "u");
        inputString = inputString.Trim().ToLower();
        inputString = WhiteSpaceRegex().Replace(inputString, "-");
        inputString = SeoRegex().Replace(inputString, string.Empty);
        return inputString;
    }

    /// <summary>
    /// string olarak verilen parametrenin değerindeki kelimelerin ilk harflerini büyük harfe �evirir.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="useOriginal">false olması durumunda "Ve, İle" bağlaçları küçük yapılır.</param>
    /// <returns></returns>
    public static string ToTitleCase(this string str, bool useOriginal = false)
    {
        var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());

        if (!useOriginal)
        {
            result = result.Replace(" Ve ", " ve ");
            result = result.Replace(" İle ", " ile ");
            return result;
        }
        return result;
    }

    public static string ListToString(this List<string> list, string delimeter) => string.Join(delimeter, [.. list]);

    public static string TemplateParser(this string templateText, string regExString, string value)
    {
        Regex regExToken = new(regExString, RegexOptions.IgnoreCase);
        return regExToken.Replace(templateText, value);
    }

    public static string ToFormatPhoneNumber(this string phoneNumber)
    {
        phoneNumber = OnlyNumberRegex().Replace(phoneNumber, "");
        // ilk karakter alınıyor
        var firstChar = phoneNumber[..1];

        // toplam karakter sayısı
        switch (phoneNumber.Length)
        {
            // toplam karakter sayısı 10 ama ilk karakter 0 değilse (5322222222)
            case 10 when firstChar != "0":

                // başına 0 ekleniyor (05322222222)
                phoneNumber = $"0{phoneNumber}";
                break;

            // toplam karakter sayısı 11 ve ilk karakter 0 ise işlem yapılmıyor (05322222222)
            case 11 when firstChar == "0":
                break;

            // toplam karakter sayısı 11 ama ilk karakter 0 değilse (*5322222222)
            case 11 when firstChar != "0":

                // sondan 10 karakter alınıyor
                phoneNumber = phoneNumber[^10..];

                // başına 0 ekleniyor
                phoneNumber = $"0{phoneNumber}";
                break;

            // toplam karakter sayısı 12 ve ilk karakter 9 ise (905322222222)
            case 12 when firstChar == "9":

                // baştan 1 karakter atılıyor (05322222222)
                phoneNumber = phoneNumber[1..];
                break;
            default:
                {
                    // toplam karakter sayısı 12den fazla ise (0905322222222)
                    if (phoneNumber.Length > 12)
                    {
                        // sondan 11 karakter alınıyor
                        phoneNumber = phoneNumber[^11..];
                    }

                    break;
                }
        }

        return phoneNumber;
    }

    [GeneratedRegex("[^0-9]")]
    private static partial Regex OnlyNumberRegex();

    [GeneratedRegex(@"<[^>]+>|&nbsp;")]
    private static partial Regex CleanHtmlRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhiteSpaceRegex();

    [GeneratedRegex(@"[^A-Za-z0-9_-]")]
    private static partial Regex SeoRegex();
}
