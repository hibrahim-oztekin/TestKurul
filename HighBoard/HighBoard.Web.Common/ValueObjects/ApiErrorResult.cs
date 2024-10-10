namespace HighBoard.Web.Common.ValueObjects;

/// <summary>
/// Api sonucu hata dönerse kullanılan sınıf
/// </summary>
/// <param name="message">hata mesajı</param>
public class ApiErrorResult(string message) : ApiResult(false, message)
{
}