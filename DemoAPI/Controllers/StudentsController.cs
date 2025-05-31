using Azure;
using DemoAPI.Data;
using DemoAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private  ApplicationDbContext _db;                                                  //  'dependency injection' is used here to inject the ApplicationDbContext into the controller ,may be.
        private readonly ILogger<StudentsController> _logger;                               // to generate 'logs' adding this ILogger dependency injection

        public StudentsController(ApplicationDbContext context , ILogger<StudentsController> logger )          
        {
            _db = context;
            _logger = logger;
        }


        [HttpGet]
        [Authorize]
        //[ResponseCache(Duration = 30)]   
        [ResponseCache(CacheProfileName = "apiCache30")] 
        public IActionResult GetAllStudents(int pagesize , int pagenumber)                                           // StudentEntity is model class name
        {                                                                                     // StudentRegister is database Table name 
            _logger.LogInformation("Fetching All Student List");

            int totalCount = _db.StudentRegister.Count();
            var studentList = _db.StudentRegister.Skip(pagesize * (pagenumber - 1)).Take(pagesize).ToList();        // Paging 
            var result = new PageResult<StudentEntity>
            {
                items = studentList,
                CurrentPage = pagenumber,
                Pagesize = pagesize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagenumber)
            };


            return Ok(result);
        }

        [HttpGet("GetStudentsById")]
        [Authorize]
        [ResponseCache(CacheProfileName = "apiCache30")]
        public ActionResult<StudentEntity> GetStudentDetails(Int32 Id)
        {
            if (Id == 0) { 
                _logger.LogError("Log_error :Student id is 0 ");
                return BadRequest(); }                                                           // status code : 400
            var StudentDetails = _db.StudentRegister.FirstOrDefault(x => x.Id == Id);

            if (StudentDetails == null) { return NotFound(); }        // status code : 404
            return StudentDetails;
        }



        [HttpPost("AddStudent")]
        [Authorize]
        public ActionResult<StudentEntity> AddStudent([FromBody] StudentEntity studentDetails)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }        // status code : 400
            _db.StudentRegister.Add(studentDetails);
            _db.SaveChanges();

            return Ok(studentDetails);                                          // status code : 200
        }
        [HttpPost("UpdateStudentDetails")]
        [Authorize]
        public ActionResult<StudentEntity> UpdateStudent(Int32 Id, [FromBody] StudentEntity studentDetails)
        {
            if (studentDetails == null) { return BadRequest(studentDetails); }                       // status code : 400
            var StudentDetailsFromDB = _db.StudentRegister.FirstOrDefault(x => x.Id == Id);
            if (StudentDetailsFromDB == null) { return NotFound(); }                                // status code : 404

            StudentDetailsFromDB.Name = studentDetails.Name;
            StudentDetailsFromDB.Age = studentDetails.Age;
            StudentDetailsFromDB.Standard = studentDetails.Standard;
            StudentDetailsFromDB.EmailAddress = studentDetails.EmailAddress;
            _db.SaveChanges();

            return Ok(studentDetails);                                          // status code : 200
        }



        
        
        [HttpPut("DeleteStudent")]
        [Authorize(Roles = "Admin")]
        public ActionResult<StudentEntity> Delete(Int32 Id) 
        {
            var StudentDetails = _db.StudentRegister.FirstOrDefault(x => x.Id == Id);
            if (StudentDetails == null) { return NotFound(); }
            _db.Remove(StudentDetails);
            _db.SaveChanges();
            return NoContent();                                              // status code : 204  
        }


        //---------------------------------------------------------------------------------------------------------------------------

        [HttpDelete("DeletEStudent")]
        public ActionResult<StudentEntity> DeletEStudent(Int32 Id)
        {
            var StudentDetails = _db.StudentRegister.FirstOrDefault(x => x.Id == Id);
            if (StudentDetails == null) { return NotFound(); }
            _db.Remove(StudentDetails);
            _db.SaveChanges();
            return NoContent();                                              // status code : 204  
        }



        //  PATCH METHOD (partial/conditional update)
        [HttpPatch("partialUpdate")]
        public ActionResult<StudentEntity> PatchStudent(int id, [FromBody] StudentEntity partialData)
        {
            var student = _db.StudentRegister.FirstOrDefault(x => x.Id == id);
            if (student == null) return NotFound();

            if (!string.IsNullOrEmpty(partialData.Name))
                student.Name = partialData.Name;
            if (partialData.Age != 0)
                student.Age = partialData.Age;
            if (!string.IsNullOrEmpty(partialData.Standard))
                student.Standard = partialData.Standard;
            if (!string.IsNullOrEmpty(partialData.EmailAddress))
                student.EmailAddress = partialData.EmailAddress;

            _db.SaveChanges();
            return Ok(student);
        }

        //  Put METHOD (full entity update)
        [HttpPut("UpdateStudent")]
        public ActionResult<StudentEntity> PutStudent(int id, [FromBody] StudentEntity updatedStudent)
        {
            if (updatedStudent == null || id != updatedStudent.Id)
            {
                return BadRequest();
            }

            var studentFromDb = _db.StudentRegister.FirstOrDefault(x => x.Id == id);
            if (studentFromDb == null)
            {
                return NotFound();
            }

            // Full update - overwrite all fields
            studentFromDb.Name = updatedStudent.Name;
            studentFromDb.Age = updatedStudent.Age;
            studentFromDb.Standard = updatedStudent.Standard;
            studentFromDb.EmailAddress = updatedStudent.EmailAddress;

            _db.SaveChanges();

            return Ok(studentFromDb); // 200 OK
        }


    }
}


/*
Operation-> Correct Verb  ->  Purpose  :

Get student by ID	 -> GET	 ->	Get student info
Create new student ->   POST   ->  Add a student
Update all fields ->	PUT	/ ->	Replace the entire resource
Update few fields  ->	PATCH	 ->	Modify part of the resource
Delete student	 -> DELETE	 ->	Remove student
*/

