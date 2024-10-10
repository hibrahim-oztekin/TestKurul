namespace HighBoard.Web.Common.ValueObjects;

/// <summary>
/// Api sonuçlarında sonuç verisi için kullanılan sınıf
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiDataResult<T> : ApiResult, IApiDataResult<T>
{
    public ApiDataResult(T data, bool success, string message) : base(success, message) => Data = data;

    public ApiDataResult(T data, bool success) : base(success) => Data = data;

    /// <summary>
    /// Sonuç verisi
    /// </summary>
    public T Data { get; }
}
