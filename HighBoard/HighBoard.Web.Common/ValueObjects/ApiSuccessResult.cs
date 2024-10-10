namespace HighBoard.Web.Common.ValueObjects;

/// <summary>
/// Api sonucu başarılı dönerse kullanılan sınıf
/// </summary>
/// <param name="message">bilgi mesajı</param>
public class ApiSuccessResult(string message) : ApiResult(true, message)
{
}