using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TagsApi.Dtos;
using TagsApi.Models;
using TagsApi.Services;

namespace TagsApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ITagRepoService _tagRepoService;
        private readonly IInitializeService _initializeService;

        public TagsController(ITagService tagService, ITagRepoService tagRepoService, IInitializeService initializeService)
        {
            _tagService = tagService;
            _tagRepoService = tagRepoService;
            _initializeService = initializeService;
        }

        [HttpGet("tags")]
        [SwaggerOperation(Summary = "Pobiera tagi", Description = "Zwraca listę tagów dostępnych w systemie.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<TagDto>), ContentTypes = ["application/json"])]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        public async Task<ActionResult<List<TagDto>>> GetTagsAsync([FromQuery] GetTagQuery tagRequest)
        {
            return Ok(await _tagRepoService.GetAsync(tagRequest));
        }

        [HttpPost("tags/initialize")]
        [SwaggerOperation(Summary = "Dodaje tagi do bazy", Description = "Pobiera listę tagów dostępnych w SO, a następnie dodaje je do bazy.")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails), ContentTypes = ["application/problem+json"])]
        public async Task<IActionResult> InitializeTagsAsync([FromBody][Required] InitializeRequest initializeRequest)
        {
            var result = await _initializeService.InitializeAsync(initializeRequest);
            if (result.IsSuccess) return Ok();

            return BadRequest(result.Error);
        }
    }
}
