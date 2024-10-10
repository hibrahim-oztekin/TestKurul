namespace HighBoard.Domain;

/// <summary>
/// Redis önbellekleme işlemlerini yapan sınıf
/// </summary>
public interface IDistributedCacheService
{
    /// <summary>
    /// Önbelleğe ekleme yapan metod
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <param name="value">değer</param>
    void Set(string key, object value);

    /// <summary>
    /// Önbelleğe ekleme yapan asenkron metod 
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <param name="value">değer</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SetAsync(string key, object value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Anahtar önbellekte mevcut mu?
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <returns></returns>
    bool Any(string key);

    /// <summary>
    /// Anahtar önbellekte mevcut mu?
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// önbellekteki değeri döndüren metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    T Get<T>(string key);

    /// <summary>
    /// önbellekteki değeri döndüren asenkron metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// önbellekteki değeri yoksa kaydedip döndüren metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    T Get<T>(string key, Func<T> func);

    /// <summary>
    /// önbellekteki değeri yoksa kaydedip döndüren asenkron metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T> GetAsync<T>(string key, Func<T> func, CancellationToken cancellationToken = default);

    /// <summary>
    /// önbellekteki değeri silen metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    void Remove(string key);

    /// <summary>
    /// önbellekteki değeri silen metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// önek ile başlayan anahtarlara ait verileri silen metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ClearStartsWithPrefixAsync(string prefix, CancellationToken cancellationToken = default);

    /// <summary>
    /// Önbelleğe kaydedilen anahtarları dönen metod
    /// </summary>
    /// <returns></returns>
    List<string> GetAllKeys();

    /// <summary>
    /// Önbelleğe kaydedilen anahtarları dönen metod
    /// </summary>
    /// <returns></returns>
    Task<List<string>> GetAllKeysAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Önek ile başlayan anahtar önbellekte mevcut mu?
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAWithPrefixAsync(string prefix, string value, CancellationToken cancellationToken = default);
}