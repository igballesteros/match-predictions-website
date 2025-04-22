using System.Net.Http.Json;
using SharedDtos;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PredictionsClient.Services
{
    public class MatchService
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<MatchService> _logger;
        private readonly AuthenticationStateProvider _authStateProvider;

        public MatchService(HttpClient httpClient,
            ILogger<MatchService> logger,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _logger = logger;
            _authStateProvider = authStateProvider;
        }


        public async Task<PaginatedResponse<MatchDto>> GetMatchesPaginated(
            int pageNumber = 1,
            int pageSize = 5,
            bool? upcomingOnly = null,
            bool? completedOnly = null)
        {
            try
            {
                var queryParams = new Dictionary<string, string>
                {
                    ["pageNumber"] = pageNumber.ToString(),
                    ["pageSize"] = pageSize.ToString()
                };

                if (upcomingOnly.HasValue)
                    queryParams.Add("upcomingOnly", upcomingOnly.Value.ToString());
                if (completedOnly.HasValue)
                    queryParams.Add("completedOnly", completedOnly.Value.ToString());

                var queryString = await new FormUrlEncodedContent(queryParams).ReadAsStringAsync();
                var response = await _httpClient.GetAsync($"api/matches?{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PaginatedResponse<MatchDto>>()
                        ?? new PaginatedResponse<MatchDto>();
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Unauthorized access to matches API");
                    throw new UnauthorizedAccessException("Authentication required");
                }

                _logger.LogError($"Failed to get matches. Status: {response.StatusCode}");
                return new PaginatedResponse<MatchDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while fetching matches");
                throw new ApplicationException("Network error occurred. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching matches");
                throw;
            }

        }

        [Obsolete("Prefer using paginated version for regular users. Admin only.")]
        public async Task<IEnumerable<MatchDto>> GetAllMatches(
        bool? upcomingOnly = null,
        bool? completedOnly = null)
        {
            try
            {
                var queryParams = new Dictionary<string, string>();
                if (upcomingOnly.HasValue)
                    queryParams.Add("upcomingOnly", upcomingOnly.Value.ToString());
                if (completedOnly.HasValue)
                    queryParams.Add("completedOnly", completedOnly.Value.ToString());

                var queryString = await new FormUrlEncodedContent(queryParams).ReadAsStringAsync();
                var response = await _httpClient.GetAsync($"api/matches/all?{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<MatchDto>>()
                        ?? Enumerable.Empty<MatchDto>();
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Attempted to access admin endpoint without privileges");
                    throw new UnauthorizedAccessException("Admin privileges required");
                }

                _logger.LogError($"Failed to get all matches. Status: {response.StatusCode}");
                return Enumerable.Empty<MatchDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all matches");
                throw;
            }
        }

        public async Task<MatchDto?> GetMatchById(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<MatchDto>($"api/matches/{id}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Match with ID {MatchId} not found", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting match with ID {MatchId}", id);
                throw;
            }
        }

        public async Task<MatchDto?> CreateMatch(CreateMatchDto createMatchDto)
        {
            if (!await IsUserAdmin())
            {
                _logger.LogWarning("Non-admin user attempted to create match");
                throw new UnauthorizedAccessException("Admin privileges required");
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/matches", createMatchDto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<MatchDto>();
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Bad request creating match: {Error}", error);
                    throw new ArgumentException(error);
                }
                _logger.LogError("Failed to create match. Status: {StatusCode}", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating match");
                throw;
            }
        }

        public async Task<bool> UpdateMatchResult(int id, UpdateMatchDto updateDto)
        {
            if (!await IsUserAdmin())
            {
                _logger.LogWarning("Non-admin user attempted to update match");
                throw new UnauthorizedAccessException("Admin privileges required");
            }

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/matches/{id}/result", updateDto);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Bad request updating match: {Error}", error);
                    throw new ArgumentException(error);
                }

                _logger.LogError("Failed to update match. Status: {StatusCode}", response.StatusCode);
                return false;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating match");
                throw;
            }
        }

        public async Task<bool> DeleteMatch(int id)
        {
            if (!await IsUserAdmin())
            {
                _logger.LogWarning("Non-admin user attempted to update match");
                throw new UnauthorizedAccessException("Admin privileges required");
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"api/matches/{id}");
                return response.IsSuccessStatusCode;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting match");
                throw;
            }
        }

        private async Task<bool> IsUserAdmin()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.IsInRole("Admin");
        }
    }


    public class PaginatedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; } = new List<T>();
    }
}