namespace HighBoard.Web.Common.ValueObjects;

/// <summary>
/// Api sonucu başarılı dönerse sonuç verisi için kullanılan sınıf
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="data">sonuç verisi</param>
public class ApiSuccessDataResult<T>(T data) : ApiDataResult<T>(data, true)
{
}