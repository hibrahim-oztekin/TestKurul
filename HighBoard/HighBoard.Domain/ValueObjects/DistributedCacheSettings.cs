namespace HighBoard.Domain.ValueObjects;

public class DistributedCacheSettings : IDistributedCacheSettings
{
    /// <summary>
    /// Bağlantı bilgisi
    /// </summary>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Bu süre boyunca istenmezse, önbelleğe alınmış bir nesne silinir
    /// </summary>
    public double SlidingExpirationSecond { get; set; }

    /// <summary>
    /// önbelleğe alınmış nesnenin sona erme süresi
    /// </summary>
    public double AbsoluteExpirationSecond { get; set; }

    /// <summary>
    /// Ana anahtar
    /// </summary>
    public required string MainKey { get; set; }

    /// <summary>
    /// Redis kullanılacak mı?
    /// </summary>
    public bool UseRedisCache { get; set; }
}