namespace HighBoard.Application.Caching;

public class DistributedCacheService : IDistributedCacheService
{
    private readonly IDistributedCache _distributedCache;

    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new();

    private readonly IDistributedCacheSettings _cacheSettings;

    public DistributedCacheService(IDistributedCache distributedCache, IDistributedCacheSettings cacheSettings)
    {
        _distributedCache = distributedCache;
        _cacheSettings = cacheSettings;
        _distributedCacheEntryOptions.SetSlidingExpiration(TimeSpan.FromSeconds(cacheSettings.SlidingExpirationSecond)).SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheSettings.AbsoluteExpirationSecond));
    }

    /// <summary>
    /// Önbelleğe ekleme yapan metod
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <param name="value">değer</param>
    public void Set(string key, object value)
    {
        // Değer boş ise ekleme yapılmaz
        if (value == null)
        {
            return;
        }

        // Anahtar daha önce kullanıldı ise ekleme yapılmaz
        if (Any(key))
        {
            return;
        }

        // değeri serileştir
        var serialized = value.Serialize();

        // değeri şifrele
        var encrypted = serialized.Encrypt();

        // ekle
        _distributedCache.SetStringAsync(key, encrypted, _distributedCacheEntryOptions);

        // anahtarı listeye ekle
        AddToKeys(key);
    }

    /// <summary>
    /// Önbelleğe ekleme yapan asenkron metod 
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <param name="value">değer</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SetAsync(string key, object value, CancellationToken cancellationToken = default)
    {
        // Değer boş ise ekleme yapılmaz
        if (value == null)
        {
            return;
        }

        // Anahtar daha önce kullanıldı ise ekleme yapılmaz
        if (await AnyAsync(key, cancellationToken))
        {
            return;
        }

        // değeri serileştir
        var serialized = value.Serialize();

        // değeri şifrele
        var encrypted = serialized.Encrypt();


        // ekle
        await _distributedCache.SetStringAsync(key, encrypted, _distributedCacheEntryOptions, cancellationToken);

        // anahtarı listeye ekle
        await AddToKeysAsync(key, cancellationToken);
    }

    /// <summary>
    /// Anahtar önbellekte mevcut mu?
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <returns></returns>
    public bool Any(string key)
    {
        return !string.IsNullOrEmpty(_distributedCache.GetString(key));
    }

    /// <summary>
    /// Anahtar önbellekte mevcut mu?
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> AnyAsync(string key, CancellationToken cancellationToken = default)
    {
        return !string.IsNullOrEmpty(await _distributedCache.GetStringAsync(key, cancellationToken));
    }

    /// <summary>
    /// önbellekteki değeri döndüren metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public T Get<T>(string key)
    {
        var result = _distributedCache.GetString(key);
        return !string.IsNullOrEmpty(result) ? result.Decrypt().Deserialize<T>()! : default!;
    }

    /// <summary>
    /// önbellekteki değeri döndüren asenkron metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        // önbellekteki string değer
        var result = await _distributedCache.GetStringAsync(key, cancellationToken);

        // string boş değilse şifreli halini çöz ve nesneye dönüştür
        return !string.IsNullOrEmpty(result) ? result.Decrypt().Deserialize<T>()! : default!;
    }

    /// <summary>
    /// önbellekteki değeri yoksa kaydedip döndüren metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public T Get<T>(string key, Func<T> func)
    {
        if (!Any(key))
        {
            Set(key, func()!);
        }
        return Get<T>(key);
    }

    /// <summary>
    /// önbellekteki değeri yoksa kaydedip döndüren asenkron metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T> GetAsync<T>(string key, Func<T> func, CancellationToken cancellationToken = default)
    {
        if (!await AnyAsync(key, cancellationToken))
        {
            await SetAsync(key, func()!, cancellationToken);
        }
        return await GetAsync<T>(key, cancellationToken);
    }

    /// <summary>
    /// önbellekteki değeri silen metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public void Remove(string key)
    {
        _distributedCache.Remove(key);
        RemoveFromKeys(key);
    }

    /// <summary>
    /// önbellekteki değeri silen metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
        await RemoveFromKeysAsync(key, cancellationToken);
    }


    /// <summary>
    /// önek ile başlayan anahtarlara ait verileri silen metod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ClearStartsWithPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        List<string> newKeys = [];
        List<string> deletedKeys = [];

        var oldKeys = await GetAllKeysAsync(cancellationToken);
        if (oldKeys != null)
        {
            foreach (var key in oldKeys)
            {
                if (key.StartsWith(prefix))
                {
                    deletedKeys.Add(key);
                }
                else
                {
                    newKeys.Add(key);
                }
            }

            foreach (var key in deletedKeys)
            {
                await _distributedCache.RemoveAsync(key, cancellationToken);
            }
            await _distributedCache.RemoveAsync(_cacheSettings.MainKey, cancellationToken);


            var serialized = newKeys.Serialize();

            var encrypted = serialized.Encrypt();

            await _distributedCache.SetStringAsync(_cacheSettings.MainKey, encrypted, _distributedCacheEntryOptions, cancellationToken);
        }

    }

    /// <summary>
    /// Önbelleğe kaydedilen anahtarları dönen metod
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllKeys()
    {
        List<string> list = [];
        if (Any(_cacheSettings.MainKey))
        {
            list = Get<List<string>>(_cacheSettings.MainKey);
        }
        return list;
    }

    /// <summary>
    /// Önbelleğe kaydedilen anahtarları dönen metod
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetAllKeysAsync(CancellationToken cancellationToken = default)
    {
        List<string> list = [];
        if (await AnyAsync(_cacheSettings.MainKey, cancellationToken))
        {
            list = await GetAsync<List<string>>(_cacheSettings.MainKey, cancellationToken);
        }
        return list;
    }

    /// <summary>
    /// Önek ile başlayan anahtar önbellekte mevcut mu?
    /// </summary>
    /// <param name="key">anahtar</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> AnyAWithPrefixAsync(string prefix, string value, CancellationToken cancellationToken = default)
    {
        var result = false;
        var keyList = await GetAllKeysAsync(cancellationToken);
        foreach (var key in keyList.Where(key => key.StartsWith(prefix)))
        {
            var str = await GetAsync<string>(key, cancellationToken);
            if (string.IsNullOrEmpty(str) || !str.Equals(value)) continue;
            result = true;
            break;
        }
        return result;
    }

    // Anahtarı listeye ekle
    private void AddToKeys(string key)
    {
        var keyList = GetAllKeys();

        if (!keyList.Contains(key))
        {
            keyList.Add(key);
        }
        _distributedCache.Remove(_cacheSettings.MainKey);

        var serialized = keyList.Serialize();

        var encrypted = serialized.Encrypt();

        _distributedCache.SetStringAsync(_cacheSettings.MainKey, encrypted, _distributedCacheEntryOptions);
    }

    // Anahtarı listeye ekle
    private async Task AddToKeysAsync(string key, CancellationToken cancellationToken = default)
    {
        var keyList = await GetAllKeysAsync(cancellationToken);

        if (!keyList.Contains(key))
        {
            keyList.Add(key);
        }
        await _distributedCache.RemoveAsync(_cacheSettings.MainKey, cancellationToken);

        var serialized = keyList.Serialize();

        var encrypted = serialized.Encrypt();

        await _distributedCache.SetStringAsync(_cacheSettings.MainKey, encrypted, _distributedCacheEntryOptions, cancellationToken);
    }

    // Anahtarı listeden sil
    private async Task RemoveFromKeysAsync(string key, CancellationToken cancellationToken = default)
    {
        var keyList = await GetAllKeysAsync(cancellationToken);

        keyList.Remove(key);

        await _distributedCache.RemoveAsync(_cacheSettings.MainKey, cancellationToken);

        var serialized = keyList.Serialize();

        var encrypted = serialized.Encrypt();

        await _distributedCache.SetStringAsync(_cacheSettings.MainKey, encrypted, _distributedCacheEntryOptions, cancellationToken);
    }

    // Anahtarı listeden sil
    private void RemoveFromKeys(string key)
    {
        var keyList = GetAllKeys();
        keyList.Remove(key);

        _distributedCache.Remove(_cacheSettings.MainKey);

        var serialized = keyList.Serialize();

        var encrypted = serialized.Encrypt();

        _distributedCache.SetStringAsync(_cacheSettings.MainKey, encrypted, _distributedCacheEntryOptions);
    }
}