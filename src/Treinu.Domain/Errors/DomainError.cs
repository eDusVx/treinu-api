using FluentResults;
using Treinu.Domain.Enums;

namespace Treinu.Domain.Errors;

public class DomainError : Error
{
    public ErrorType Type { get; }
    public string Code { get; }

    public DomainError(string code, string message, ErrorType type) 
        : base(message)
    {
        Code = code;
        Type = type;
        Metadata.Add("ErrorCode", code);
        Metadata.Add("ErrorType", type);
    }
}
