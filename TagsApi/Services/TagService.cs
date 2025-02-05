using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using TagsApi.Common;
using TagsApi.Models;

namespace TagsApi.Services
{
    public interface ITagService
    {
        Task<Result<GetTagResponse, ProblemDetails>> GetAsync(int minValues);
    }

    public class TagService : ITagService
    {
        private const string HTTP_CLIENT_NAME = "StackExchangeHttpClient";
        private const int MAX_PAGE_SIZE = 100;
        private readonly HttpClient _httpClient;
        private readonly IProblemDetailsFactory _problemDetailsFactory;

        public TagService(IHttpClientFactory httpClientFactory, IProblemDetailsFactory problemDetailsFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
            _problemDetailsFactory = problemDetailsFactory;
        }

        public async Task<Result<GetTagResponse, ProblemDetails>> GetAsync(int minValues)
        {
            var tags = new List<Tag>();
            var pagesNeeded = (int)Math.Ceiling((double)minValues / MAX_PAGE_SIZE);
            int currentPage = 1;

            while (currentPage <= pagesNeeded)
            {
                QueryBuilder queryBuilder = new()
                {
                    { "site", "stackoverflow" },
                    { "pagesize", MAX_PAGE_SIZE.ToString() },
                    { "page", currentPage.ToString() }
                };
                
                var endpoint = $"2.3/tags{queryBuilder}";

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_httpClient.BaseAddress!, endpoint),
                };

                request.Headers.Add("User-Agent", "TagsApi/1.0");
                //needed, otherwise request will be blocked

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var tagResponse = await response.Content.ReadFromJsonAsync<GetTagResponse>();
                    if (tagResponse is not null || tagResponse?.Items is not null)
                    {
                        currentPage++;
                        tags.AddRange(tagResponse.Items);
                        continue;
                    }            
                }
                return Result.Failure<GetTagResponse, ProblemDetails>(
                    _problemDetailsFactory.CreateProblemDetails(response.ReasonPhrase, statusCode: (int)response.StatusCode));
            }

            return Result.Success<GetTagResponse, ProblemDetails>(new GetTagResponse() { Items = tags});
        }
    }
}