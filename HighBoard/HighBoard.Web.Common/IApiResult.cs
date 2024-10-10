namespace HighBoard.Web.Common;

/// <summary>
/// Api sonuçları için arayüz
/// </summary>
public interface IApiResult
{
    /// <summary>
    /// Başarılı mı?
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// Mesaj metni
    /// </summary>
    string Message { get; }
}