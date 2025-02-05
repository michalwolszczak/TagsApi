using MongoDB.Driver;
using TagsApi.Models;
using TagsApi.Dtos;
using TagsApi.Extensions;

namespace TagsApi.Services
{    
    public interface ITagRepoService
    {        
        Task<List<TagDto>> GetAsync(GetTagQuery tagRequest);
        Task InsertManyAsync(List<TagDto> newTags);
        Task DeleteAllAsync();
    }

    public class TagRepoService : ITagRepoService
    {
        private readonly IMongoCollection<TagDto> _tagsCollection;

        public TagRepoService(IMongoCollection<TagDto> tagCollection)
        {
            _tagsCollection = tagCollection;
        }

        public async Task<List<TagDto>> GetAsync(GetTagQuery tagQuery) 
        {            
            return await _tagsCollection.Find(_ => true)
                .ApplyQueryParameters(tagQuery)
                .ToListAsync();
        }
        public async Task InsertManyAsync(List<TagDto> newTags) 
        {
            if (newTags.Count > 0) 
                await _tagsCollection.InsertManyAsync(newTags); 
        }

        public async Task DeleteAllAsync() => await _tagsCollection.DeleteManyAsync(_ => true);
    }
}