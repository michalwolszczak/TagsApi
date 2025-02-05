using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using TagsApi.Dtos;
using TagsApi.Models;

namespace TagsApi.Services
{
    public interface IInitializeService
    {
        Task<UnitResult<ProblemDetails>> InitializeAsync(InitializeRequest initializeRequest);
    }

    public class InitializeService : IInitializeService
    {
        private readonly ITagRepoService _tagRepoService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly ITagSharedCalculationService _tagSharedCalculationService;

        public InitializeService(ITagService tagService, ITagRepoService tagRepoService, IMapper mapper, ITagSharedCalculationService tagSharedCalculationService)
        {
            _tagService = tagService;
            _tagRepoService = tagRepoService;
            _mapper = mapper;
            _tagSharedCalculationService = tagSharedCalculationService;
        }

        public async Task<UnitResult<ProblemDetails>> InitializeAsync(InitializeRequest initializeRequest)
        {
            var result = await _tagService.GetAsync(initializeRequest.MinValues);
            if (result.IsFailure)
                return result.Error;

            if (initializeRequest is not null && initializeRequest.DeleteExisting)
            {
                await _tagRepoService.DeleteAllAsync();
            }

            var tags = result.Value.Items;
            var tagsDto = _mapper.Map<List<TagDto>>(tags);

            _tagSharedCalculationService.CalculateSharesPercentage(tagsDto);

            await _tagRepoService.InsertManyAsync(tagsDto);

            return UnitResult.Success<ProblemDetails>();
        }
    }
}
