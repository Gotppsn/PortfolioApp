using System.Net.Http.Json;
using PortfolioApp.Shared.Models;

namespace PortfolioApp.Client.Services
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProjectDto>> GetProjectsAsync(bool includePrivate = false)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/projects?includePrivate={includePrivate}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ProjectDto>>() ?? new List<ProjectDto>();
                }
                return new List<ProjectDto>();
            }
            catch
            {
                return new List<ProjectDto>();
            }
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/projects/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProjectDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProjectDto?> CreateProjectAsync(CreateProjectDto project)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/projects", project);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProjectDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateProjectAsync(UpdateProjectDto project)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/projects/{project.Id}", project);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/projects/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<TechnologyDto>> GetTechnologiesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/projects/technologies");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<TechnologyDto>>() ?? new List<TechnologyDto>();
                }
                return new List<TechnologyDto>();
            }
            catch
            {
                return new List<TechnologyDto>();
            }
        }

        public async Task<TechnologyDto?> AddTechnologyAsync(AddTechnologyDto technology)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/projects/technologies", technology);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TechnologyDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProjectImageDto?> AddProjectImageAsync(int projectId, MultipartFormDataContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/projects/{projectId}/images", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProjectImageDto>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteProjectImageAsync(int imageId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/projects/images/{imageId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    public interface IProjectService
    {
        Task<List<ProjectDto>> GetProjectsAsync(bool includePrivate = false);
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto?> CreateProjectAsync(CreateProjectDto project);
        Task<bool> UpdateProjectAsync(UpdateProjectDto project);
        Task<bool> DeleteProjectAsync(int id);
        Task<List<TechnologyDto>> GetTechnologiesAsync();
        Task<TechnologyDto?> AddTechnologyAsync(AddTechnologyDto technology);
        Task<ProjectImageDto?> AddProjectImageAsync(int projectId, MultipartFormDataContent content);
        Task<bool> DeleteProjectImageAsync(int imageId);
    }
}