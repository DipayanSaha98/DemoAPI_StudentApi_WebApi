using SchoolManagement.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SchoolManagement.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string _ApiURLPath = "http://localhost:5188/";
        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_ApiURLPath);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PageResult<StudentEntity>> GetAllStudents(string Token , int pagesize, int pagenumber)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);   // this is how need to set the token headers programmatically in the code like used to do in postman .

            HttpResponseMessage response = await _httpClient.GetAsync($"api/Students?pagesize={pagesize}&pagenumber={pagenumber}");
            response.EnsureSuccessStatusCode();

            var contents = await response.Content.ReadAsStringAsync();
            var APIResponse = JsonConvert.DeserializeObject<PageResult<StudentEntity>>(contents);
            return APIResponse;
        }

        public async Task AddStudent(StudentEntity student, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);   // this is how need to set the token headers programmatically in the code like used to do in postman .

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/Students/AddStudent", student);
            response.EnsureSuccessStatusCode();
        }


        public async Task<StudentEntity> GetStudentDetailsById(int id, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);   // this is how need to set the token headers programmatically in the code like used to do in postman .

            HttpResponseMessage response = await _httpClient.GetAsync($"api/Students/GetStudentsById?id={id}");
            response.EnsureSuccessStatusCode();
            var contents = await response.Content.ReadAsStringAsync();
            var APIResponse = JsonConvert.DeserializeObject<StudentEntity>(contents);
            return APIResponse;
        }
        public async Task UpdateStudent(int id, StudentEntity student, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);   // this is how need to set the token headers programmatically in the code like used to do in postman .

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/Students/UpdateStudentDetails?id={id}", student);
            response.EnsureSuccessStatusCode();
        }


        public async Task DeleteStudentDetails(int id, string Token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);   // this is how need to set the token headers programmatically in the code like used to do in postman .

            HttpResponseMessage response = await _httpClient.PutAsync($"api/Students/DeleteStudent?id={id}",null);
            response.EnsureSuccessStatusCode();

        } 
    } 
}
