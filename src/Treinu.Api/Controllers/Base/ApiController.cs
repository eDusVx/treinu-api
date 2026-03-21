using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Treinu.Domain.Enums;
using Treinu.Domain.Errors;

namespace Treinu.Api.Controllers.Base;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected IActionResult HandleFailure(Result result)
    {
        if (result.IsSuccess) throw new InvalidOperationException();

        var domainError = result.Errors.FirstOrDefault() as DomainError;

        if (domainError == null)
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Detail = result.Errors.FirstOrDefault()?.Message ?? "Um erro desconhecido ocorreu."
            });

        return domainError.Type switch
        {
            ErrorType.NotFound => NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Status = StatusCodes.Status404NotFound,
                Detail = domainError.Message
            }),
            ErrorType.Conflict => Conflict(new ProblemDetails
            {
                Title = "Conflict",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                Status = StatusCodes.Status409Conflict,
                Detail = domainError.Message
            }),
            _ => BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Detail = domainError.Message
            })
        };
    }

    protected IActionResult HandleFailure<T>(Result<T> result)
    {
        return HandleFailure(result.ToResult());
    }
}