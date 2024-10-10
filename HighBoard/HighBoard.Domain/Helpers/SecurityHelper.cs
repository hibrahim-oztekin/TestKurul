using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace HighBoard.Domain.Helpers;

/// <summary>
/// Şifreleme işlemleri için yardımcı sınıf
/// </summary>
public static class SecurityHelper
{
    private const string AesKey = "8de62175a88641c09e6de4435b4d1e0e";
    private static readonly byte[] Salt = "Adfnet1234567890"u8.ToArray();

    /// <summary>
    /// Düz metni şifreli hale getirir.
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Encrypt(this string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return string.Empty;


        byte[] encryptedBytes;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(AesKey);
            aes.IV = Salt;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            encryptedBytes = msEncrypt.ToArray();
        }

        return Convert.ToBase64String(encryptedBytes);

    }

    /// <summary>
    /// Şifreli bir ifadeyi şifresiz haline getirir.
    /// </summary>
    /// <param name="encryptedText"></param>
    /// <returns></returns>
    public static string Decrypt(this string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText)) return string.Empty;

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(AesKey);
        aes.IV = Salt;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream msDecrypt = new(Convert.FromBase64String(encryptedText));
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);

        var plaintext = srDecrypt.ReadToEnd();

        return plaintext;
    }

    /// <summary>
    /// HMACSHA256 algoritması kullanarak özet oluşturur.
    /// </summary>
    /// <param name="inputString"></param>
    /// <param name="keyString"></param>
    /// <returns></returns>
    public static string ToHmacSha256(string inputString, string keyString)
    {
        var keyBytes = Encoding.UTF8.GetBytes(keyString);
        using HMACSHA256 hash = new(keyBytes);
        var inputBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        return Convert.ToBase64String(inputBytes);
    }

    /// <summary>
    /// Kurallara uygun parola olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="password">parola</param>
    /// <param name="minLength">en az boyut</param>
    /// <param name="numUpper">en az gerekli büyük harf sayısı</param>
    /// <param name="numLower">en az gerekli küçük harf sayısı</param>
    /// <param name="numNumbers">en az gerekli rakam sayısı</param>
    /// <param name="numSpecial">en az gerekli özel karakter sayısı</param>
    /// <returns></returns>
    public static bool ValidatePassword(this string password, int minLength = 8, int numUpper = 1, int numLower = 1, int numNumbers = 1, int numSpecial = 1)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        Regex upper = new("[A-Z]");
        Regex lower = new("[a-z]");
        Regex number = new("[0-9]");
        Regex special = new("[^a-zA-Z0-9]");

        if (password.Length < minLength)
        {
            return false;
        }

        if (upper.Matches(password).Count < numUpper)
        {
            return false;
        }
        if (lower.Matches(password).Count < numLower)
        {
            return false;
        }
        if (number.Matches(password).Count < numNumbers)
        {
            return false;
        }
        return special.Matches(password).Count >= numSpecial;
    }

    /// <summary>
    /// SHA512 algoritması kullanarak özet oluşturur. 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToSha512(this string str)
    {
        var encryptedBytes = SHA512.HashData(Encoding.UTF8.GetBytes(str));
        StringBuilder stringBuilder = new();
        foreach (var t in encryptedBytes)
        {
            stringBuilder.Append(t.ToString("x2"));
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Parolanın BCrypt algoritmasına göre özetini çıkarır
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string ToHashPassword(this string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    /// <summary>
    /// Parolayı doğrular. 
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordHash"></param>
    /// <returns></returns>
    public static bool VerifyPassword(this string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    /// <summary>
    /// Rastgele parola oluşturur.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="useUpperCaseChars"></param>
    /// <param name="useLowerCaseChars"></param>
    /// <param name="useSpecialChars"></param>
    /// <returns></returns>
    public static string CreatePassword(int length, bool useUpperCaseChars = false, bool useLowerCaseChars = false, bool useSpecialChars = false)
    {
        const string numericChars = "0123456789";
        const string upperCaseChars = "ABCDEFGHJKLMNPQRSTWXYZ";
        const string lowerCaseChars = "abcdefgijkmnopqrstwxyz";
        const string specialChars = "%#+-!@?";
        List<char[]> charGroups =
        [
            numericChars.ToCharArray()
        ];

        if (useUpperCaseChars)
        {
            charGroups.Add(upperCaseChars.ToCharArray());
        }

        if (useLowerCaseChars)
        {
            charGroups.Add(lowerCaseChars.ToCharArray());
        }

        if (!useSpecialChars)
        {
            charGroups.Add(specialChars.ToCharArray());
        }

        var charsLeftInGroup = new int[charGroups.Count];
        for (var i = 0; i < charsLeftInGroup.Length; i++)
        {
            charsLeftInGroup[i] = charGroups[i].Length;
        }
        var leftGroupsOrder = new int[charGroups.Count];
        for (var i = 0; i < leftGroupsOrder.Length; i++)
        {
            leftGroupsOrder[i] = i;
        }
        var randomBytes = new byte[4];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var seed = BitConverter.ToInt32(randomBytes, 0);
        Random random = new(seed);
        var password = new char[length];
        var lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
        for (var i = 0; i < password.Length; i++)
        {
            var nextLeftGroupsOrderIdx = lastLeftGroupsOrderIdx == 0 ? 0 : random.Next(0, lastLeftGroupsOrderIdx);
            var nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
            var lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;
            var nextCharIdx = lastCharIdx == 0 ? 0 : random.Next(0, lastCharIdx + 1);
            password[i] = charGroups[nextGroupIdx][nextCharIdx];
            if (lastCharIdx == 0)
            {
                charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
            }
            else
            {
                if (lastCharIdx != nextCharIdx)
                {
                    (charGroups[nextGroupIdx][lastCharIdx], charGroups[nextGroupIdx][nextCharIdx]) = (charGroups[nextGroupIdx][nextCharIdx], charGroups[nextGroupIdx][lastCharIdx]);
                }
                charsLeftInGroup[nextGroupIdx]--;
            }
            if (lastLeftGroupsOrderIdx == 0)
            {
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            }
            else
            {
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    (leftGroupsOrder[lastLeftGroupsOrderIdx], leftGroupsOrder[nextLeftGroupsOrderIdx]) = (leftGroupsOrder[nextLeftGroupsOrderIdx], leftGroupsOrder[lastLeftGroupsOrderIdx]);
                }
                lastLeftGroupsOrderIdx--;
            }
        }
        return new string(password);
    }

    /// <summary>
    /// Rastgele ID oluşturur.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>

    public static string CreateId(int length)
    {
        StringBuilder sb = new();

        var letters = "ABCDEFGHIJKLMNOPRSTUVYZQXW".ToCharArray().Select(c => c.ToString()).ToArray();

        var numbers = DateTime.Now.Ticks.ToString().ToCharArray().Select(c => c.ToString()).ToArray();

        for (var i = 1; i <= length; i++)
        {
            Random random1 = new();
            var int1 = random1.Next(letters.Length);
            var str1 = letters[int1];
            sb.Append(str1);

            Random random2 = new();
            var int2 = random2.Next(numbers.Length);
            var str2 = numbers[int2];
            sb.Append(str2);

            Random random3 = new();
            var int3 = random3.Next(letters.Length);
            var str3 = letters[int3];
            sb.Append(str3);

            Random random4 = new();
            var int4 = random4.Next(numbers.Length);
            var str4 = numbers[int4];
            sb.Append(str4);

            if (i != length)
            {
                sb.Append('-');
            }
        }

        return sb.ToString();
    }

}