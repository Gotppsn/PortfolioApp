using System.Net.Http.Json;
using PortfolioApp.Shared.Models;

namespace PortfolioApp.Client.Services
{
    public class CodeSnippetService : ICodeSnippetService
    {
        private readonly HttpClient _httpClient;

        public CodeSnippetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CodeSnippetDto>> GetCodeSnippetsAsync(bool includePrivate = false, string? tag = null, string? language = null)
        {
            try
            {
                var url = $"api/codesnippets?includePrivate={includePrivate}";
                if (!string.IsNullOrEmpty(tag))
                {
                    url += $"&tag={Uri.EscapeDataString(tag)}";
                }
                if (!string.IsNullOrEmpty(language))
                {
                    url += $"&language={Uri.EscapeDataString(language)}";
                }

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<CodeSnippetDto>>() ?? new List<CodeSnippetDto>();
                }
                return new List<CodeSnippetDto>();
            }
            catch
            {
                return new List<CodeSnippetDto>();
            }
        }

        public async Task<CodeSnippetDto?> GetCodeSnippetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/codesnippets/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CodeSnippetDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<CodeSnippetDto?> CreateCodeSnippetAsync(CreateCodeSnippetDto snippet)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/codesnippets", snippet);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CodeSnippetDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateCodeSnippetAsync(UpdateCodeSnippetDto snippet)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/codesnippets/{snippet.Id}", snippet);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCodeSnippetAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/codesnippets/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<string>> GetLanguagesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/codesnippets/languages");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
                }
                return new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<List<TagDto>> GetTagsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/codesnippets/tags");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<TagDto>>() ?? new List<TagDto>();
                }
                return new List<TagDto>();
            }
            catch
            {
                return new List<TagDto>();
            }
        }
    }

    public interface ICodeSnippetService
    {
        Task<List<CodeSnippetDto>> GetCodeSnippetsAsync(bool includePrivate = false, string? tag = null, string? language = null);
        Task<CodeSnippetDto?> GetCodeSnippetByIdAsync(int id);
        Task<CodeSnippetDto?> CreateCodeSnippetAsync(CreateCodeSnippetDto snippet);
        Task<bool> UpdateCodeSnippetAsync(UpdateCodeSnippetDto snippet);
        Task<bool> DeleteCodeSnippetAsync(int id);
        Task<List<string>> GetLanguagesAsync();
        Task<List<TagDto>> GetTagsAsync();
    }
}