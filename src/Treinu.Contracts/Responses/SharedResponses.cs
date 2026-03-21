namespace Treinu.Contracts.Responses;

public record TokenDto(string AccessToken, string RefreshToken);

public record PaginationResponse<T>(
    IEnumerable<T> Data,
    int Total,
    int Page,
    int Limit,
    int TotalPages
);
