using Newtonsoft.Json;
using SchoolManagement.Model;
using System.Net.Http.Headers;

namespace SchoolManagement.Services
{
    public class ApiUserService
    {
        private readonly HttpClient _httpClient;
        private string _ApiURLPath = "http://localhost:5188/";
        public ApiUserService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_ApiURLPath);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<LoginResponseDTO> AuthenticateUser(LoginRequestDTO userDetails)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<LoginRequestDTO>($"api/Users/UserLogin", userDetails);
            response.EnsureSuccessStatusCode();
            var contents = await response.Content.ReadAsStringAsync();
            var APIResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(contents);
            return APIResponse;
        }
    }
}
