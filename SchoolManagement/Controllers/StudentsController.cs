using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagement.Models;
using SchoolManagement.Services;
using System.Net.Http.Headers;

namespace SchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApiService _apiService;
        //private string baseURL = "http://localhost:5188/";
        public StudentsController()
        {
            _apiService = new ApiService();
        }


        [Authorize(Roles = "Admin, Student")]
        [ResponseCache(Duration = 30)]
        public async Task<IActionResult> Index()
        {
            List<StudentEntity> lstStudents = new List<StudentEntity>();
            lstStudents = await _apiService.GetAllStudents(HttpContext.Session.GetString("APIToken"));
            return View(lstStudents);
            #region old_code
            //-------------------------------------------OLD------------------------------------------------------
            //List<StudentEntity> lstStudents = new List<StudentEntity>();
            //using (var _httpClient = new HttpClient())
            //{
            //    _httpClient.BaseAddress = new Uri(baseURL + "api/Students/");
            //    _httpClient.DefaultRequestHeaders.Accept.Clear();
            //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage getData = await _httpClient.GetAsync("");
            //    if (getData.IsSuccessStatusCode)
            //    {
            //        string result = getData.Content.ReadAsStringAsync().Result;
            //        lstStudents = JsonConvert.DeserializeObject<List<StudentEntity>>(result);
            //    }
            //    else return View("ErrorPage");
            //}
            //return View(lstStudents);
            #endregion
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        { 
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStudent(StudentEntity studentEntity)
        {
            await _apiService.AddStudent(studentEntity, HttpContext.Session.GetString("APIToken"));
            return RedirectToAction("Index");
            #region old_code
            //using (var _httpClient = new HttpClient())
            //    {
            //        _httpClient.BaseAddress = new Uri(baseURL + "api/Students/");        // _httpClient.BaseAddress = new Uri(baseURL + "api/Students/AddStudent/");
            //        _httpClient.DefaultRequestHeaders.Accept.Clear();
            //        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        HttpResponseMessage getData = await _httpClient.PostAsJsonAsync("", studentEntity);
            //        if (getData.IsSuccessStatusCode)
            //        {
            //            return RedirectToAction("Index");
            //        }
            //        else return View("ErrorPage");
            //    }
            #endregion
        }

        [Authorize(Roles = "Admin , Student")]
        public async Task<IActionResult> Details(int Id)
        {
            StudentEntity studentDetails = new StudentEntity();
            studentDetails = await _apiService.GetStudentDetailsById(Id, HttpContext.Session.GetString("APIToken"));
            return View(studentDetails);
            #region old_code
            //StudentEntity studentDetails = new StudentEntity();
            //using (var _httpClient = new HttpClient())
            //{
            //    _httpClient.BaseAddress = new Uri(baseURL + "api/Students/");
            //    _httpClient.DefaultRequestHeaders.Accept.Clear();
            //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage getData = await _httpClient.GetAsync($"GetStudentsById?Id={Id}");
            //    if (getData.IsSuccessStatusCode)
            //    {
            //        string result = getData.Content.ReadAsStringAsync().Result;
            //        studentDetails = JsonConvert.DeserializeObject<StudentEntity>(result);
            //    }
            //    else return View("ErrorPage");
            //}
            //return View(studentDetails);
            #endregion
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStudentDetails(int Id, StudentEntity studentEntity)
        {
            await _apiService.UpdateStudent(Id, studentEntity, HttpContext.Session.GetString("APIToken"));
            return RedirectToAction("Index");
            #region old_code
            //using (var _httpClient = new HttpClient())
            //{
            //    _httpClient.BaseAddress = new Uri(baseURL + "api/Students/");
            //    _httpClient.DefaultRequestHeaders.Accept.Clear();
            //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage getData = await _httpClient.PostAsJsonAsync($"UpdateStudentDetails?Id={Id}", studentEntity);
            //    if (getData.IsSuccessStatusCode)
            //    {
            //        return RedirectToAction("Index");
            //    }
            //    else return View("ErrorPage");
            //}
            #endregion
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            StudentEntity studentDetails = new StudentEntity();
            studentDetails = await _apiService.GetStudentDetailsById(Id, HttpContext.Session.GetString("APIToken"));
            return View(studentDetails);
            #region old_code
            //StudentEntity studentDetails = new StudentEntity();
            //using (var _httpClient = new HttpClient())
            //{
            //    _httpClient.BaseAddress = new Uri(baseURL + "api/Students/");
            //    _httpClient.DefaultRequestHeaders.Accept.Clear();
            //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage getData = await _httpClient.GetAsync($"GetStudentsById?Id={Id}");
            //    if (getData.IsSuccessStatusCode)
            //    {
            //        string result = getData.Content.ReadAsStringAsync().Result;
            //        studentDetails = JsonConvert.DeserializeObject<StudentEntity>(result);
            //    }
            //    else return View("ErrorPage");
            //}
            //return View(studentDetails);
            #endregion
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int Id)
        {
            await _apiService.DeleteStudentDetails(Id , HttpContext.Session.GetString("APIToken"));
            return RedirectToAction("Index");
            #region old_code
            //using (var _httpClient = new HttpClient())
            //{
            //    _httpClient.BaseAddress = new Uri(baseURL + "api/Students/");
            //    _httpClient.DefaultRequestHeaders.Accept.Clear();
            //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage getData = await _httpClient.PutAsync($"DeleteStudent?Id={Id}", null);
            //    if (getData.IsSuccessStatusCode)
            //    {
            //        return RedirectToAction("Index");
            //    }
            //    else return View("ErrorPage");
            //}
            #endregion
        }


        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}
