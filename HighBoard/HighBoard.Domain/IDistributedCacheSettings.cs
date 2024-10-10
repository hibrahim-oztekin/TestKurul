namespace HighBoard.Domain;

public interface IDistributedCacheSettings
{

    /// <summary>
    /// Bağlantı bilgisi
    /// </summary>
    string ConnectionString { get; set; }

    /// <summary>
    /// Bu süre boyunca istenmezse, önbelleğe alınmış bir nesne silinir
    /// </summary>
    double SlidingExpirationSecond { get; set; }

    /// <summary>
    /// önbelleğe alınmış nesnenin sona erme süresi
    /// </summary>
    double AbsoluteExpirationSecond { get; set; }

    /// <summary>
    /// Ana anahtar
    /// </summary>
    public string MainKey { get; set; }

    /// <summary>
    /// Redis kullanılacak mı?
    /// </summary>
    bool UseRedisCache { get; set; }
}