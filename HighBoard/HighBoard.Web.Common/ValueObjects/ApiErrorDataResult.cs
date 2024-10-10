namespace HighBoard.Web.Common.ValueObjects;

/// <summary>
/// Api sonucu başarısız ise kullanılan sınıf
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="data">sonuç verisi</param>
/// <param name="message">hata mesajı</param>
public class ApiErrorDataResult<T>(T data, string message) : ApiDataResult<T>(data, false, message)
{
}