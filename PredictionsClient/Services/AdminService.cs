using SharedDtos;
using System.Net.Http.Json;

namespace PredictionsClient.Services
{
    public class AdminService
    {

        private readonly HttpClient _http; // client dependency

        public AdminService(HttpClient http) // dependency injection
        {
            _http = http;
        }

        // get all users
        public async Task<List<UserDto>> GetUsersAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<UserDto>>("api/users");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                throw;
            }
        }

        // create new match
        public async Task AddMatchAsync(MatchDto match)
        {
            try
            {
                // post request with match data
                await _http.PostAsJsonAsync("api/matches", match);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error adding matches: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateMatchResultAsync(int matchId, UpdateMatchDto updateDto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/matches/{matchId}/result", updateDto);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                // Handle specific status codes if needed
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Bad request: {errorMessage}");
                }

                return false;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error updating match result: {ex.Message}");
                return false;
            }
        }
    }
}