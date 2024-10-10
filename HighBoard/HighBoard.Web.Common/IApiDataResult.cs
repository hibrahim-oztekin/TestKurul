namespace HighBoard.Web.Common;

/// <summary>
/// Api sonuçlarında sonuç verisi amacıyla kullanılan sınıflar için arayüz
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IApiDataResult<T> : IApiResult
{
    /// <summary>
    /// Sonuç verisi
    /// </summary>
    T Data { get; }
}
