using Microsoft.AspNetCore.Mvc;

namespace TagsApi.Common
{
    public interface IProblemDetailsFactory
    {
        ProblemDetails CreateProblemDetails(Exception ex);
        ProblemDetails CreateProblemDetails(string? detail, string? title = null, int? statusCode = null);
    }

    public class ProblemDetailsFactory : IProblemDetailsFactory
    {
        private const string DEFAULT_TYPE = "about:blank";
        private const string DEFAULT_DETAIL = "An unexpected problem occured";
        private const string DEFAULT_TITLE = "Something went wrong";

        public ProblemDetails CreateProblemDetails(Exception exception) => new()
        {
            Type = DEFAULT_TYPE,
            Detail = !string.IsNullOrWhiteSpace(exception.Message) ? exception.Message : DEFAULT_DETAIL,
            Title = "Internal server error",
            Status = StatusCodes.Status500InternalServerError
        };

        public ProblemDetails CreateProblemDetails(string? detail, string? title = null, int? statusCode = null) => new()
        {
            Type = DEFAULT_TYPE,
            Detail = !string.IsNullOrWhiteSpace(detail) ? detail : DEFAULT_DETAIL,
            Title = title ?? DEFAULT_TITLE,
            Status = statusCode ?? StatusCodes.Status400BadRequest,
        };
    }
}