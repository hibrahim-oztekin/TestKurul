namespace HighBoard.Core.ValueObjects;

public record struct IdCodeName(Guid Id, string? Code, string? Name);
