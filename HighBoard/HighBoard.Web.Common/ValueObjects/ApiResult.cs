namespace HighBoard.Web.Common.ValueObjects;

/// <summary>
/// Api sonucu kullanılan sınıf
/// </summary>
/// <param name="success">başarı durumu</param>
public class ApiResult(bool success) : IApiResult
{

    public ApiResult(bool success, string message) : this(success)
    {
        Message = message;
    }
    /// <summary>
    /// Başarılı mı?
    /// </summary>
    public bool Success { get; } = success;

    /// <summary>
    /// Mesaj metni
    /// </summary>
    public string Message { get; }


}