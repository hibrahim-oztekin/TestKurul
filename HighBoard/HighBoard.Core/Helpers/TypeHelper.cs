using System.Text.Json;
using System.Text;

namespace HighBoard.Core.Helpers;

public static class TypeHelper
{

    /// <summary>
    /// Nesneyi "byte"a çevirir
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static byte ToByte(this object obj, byte defaultValue = default) => obj == null ? defaultValue : !byte.TryParse(obj.ToString(), out var result) ? defaultValue : result;

    /// <summary>
    /// Nesneyi "int"e çevirir
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int ToInt(this object? obj, int defaultValue = default) => obj == null ? defaultValue : !int.TryParse(obj.ToString(), out var result) ? defaultValue : result;

    /// <summary>
    /// Nesneyi "double"a çevirir
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double ToDouble(this object obj, double defaultValue = default) => obj == null ? defaultValue : !double.TryParse(obj.ToString(), out var result) ? defaultValue : result;

    /// <summary>
    /// Nesneyi "decimal"a çevirir
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal ToDecimal(this object obj, decimal defaultValue = default) => obj == null ? defaultValue : !decimal.TryParse(obj.ToString(), out var result) ? defaultValue : result;

    /// <summary>
    /// Nesneyi "DateTime"a çevirir
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this object obj, DateTime defaultValue = default) => obj == null || string.IsNullOrEmpty(obj.ToString()) ? defaultValue : !DateTime.TryParse(obj.ToString(), out var result) ? defaultValue : result;

    /// <summary>
    /// Nesne nümerik mi?
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsNumeric(this string input) => int.TryParse(input, out var result);

    public static async Task<T> DeserializeFromStringAsync<T>(this Stream stream, CancellationToken token)
    {
        var result = await JsonSerializer.DeserializeAsync<T>(stream, JsonConstants.DefaultSerializerOptions, token)!;
        return result!;
    }


    /// <summary>
    /// Nesneyi "Enum"a çevirir
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T ToEnum<T>(this string value) => (T)Enum.Parse(typeof(T), value, true);

    /// <summary>
    /// string bir ifadeyi değerine göre bolean olarak döner
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    public static bool ToBoolean(this string str)
    {

        string[] trueStrings = ["1", "Y", "y", "yes", "true", "Evet", "E", "evet", "on"];

        string[] falseStrings = ["0", "N", "n", "no", "false", "Hayır", "Hayir", "H", "hayır", "hayir", "off"];

        if (trueStrings.Contains(str, StringComparer.OrdinalIgnoreCase))
        {
            return true;
        }

        if (falseStrings.Contains(str, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        throw new InvalidCastException("Yalnızca şu ifadeler dönüştürülür: " + string.Join(", ", trueStrings) + " ve " + string.Join(", ", falseStrings));

    }

    /// <summary>
    /// Nesneyi byte dizisine çevirir
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static byte[] ToByteArray(this object obj) => JsonSerializer.SerializeToUtf8Bytes(obj);

    /// <summary>
    /// Byte dizisini nesneye döndürür
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="byteArray"></param>
    /// <returns></returns>
    public static T FromByteArray<T>(this byte[] byteArray) => JsonSerializer.Deserialize<T>(byteArray)!;

    /// <summary>
    /// JsonDocument nesnesini string olarak döner
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    public static string JsonDocumentToString(this JsonDocument document)
    {
        using MemoryStream ms = new();
        using Utf8JsonWriter writer = new(ms, new JsonWriterOptions { Indented = true });
        document.WriteTo(writer);
        writer.Flush();
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    /// <summary>
    /// Nesneyi string olarak döner
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Serialize(this object obj) => JsonSerializer.Serialize(obj, JsonConstants.DefaultSerializerOptions);

    /// <summary>
    /// String bir değeri nesneye çevirir
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T? Deserialize<T>(this string json) => JsonSerializer.Deserialize<T>(json, JsonConstants.DefaultSerializerOptions) ?? default;
}
