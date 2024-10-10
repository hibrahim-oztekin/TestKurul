namespace HighBoard.Domain.ValueObjects;

public class ApplicationValidationResult(string propertyName, string errorMessage)
{

    /// <summary>
    /// Doğrulanamayan özellik
    /// </summary>
    public string PropertyName { get; set; } = propertyName;

    /// <summary>
    /// Doğrulama hata iletisi
    /// </summary>
    public string ErrorMessage { get; set; } = errorMessage;

}