using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.Model;
using System.Xml.Linq;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // CRUD Operations

        private readonly StudentDbContext _studentDbContext;

        public StudentsController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            return _studentDbContext.Students.ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            return _studentDbContext.Students.Find(id);
        }
        [HttpPost("addStudent")]
        public async Task<ActionResult<string>> AddStudent(Student register)
        {
            var student = new Student
            {
                Id = register.Id,
                Name = register.Name,
                Department = register.Department,
                Year = register.Year,
                RollNumber = register.RollNumber
            };

            _studentDbContext.Students.Add(student);
            await _studentDbContext.SaveChangesAsync();
            return "Student Added Successfully";
        }

        [HttpPost("updateStudent")]
        public async Task<ActionResult<string>> UpdateStudent(Student update)
        {
            var cuurentStudent = _studentDbContext.Students.Where(x => x.Id == update.Id).FirstOrDefault<Student>();
            if (cuurentStudent != null)
            {
                cuurentStudent.Id = update.Id;
                cuurentStudent.Name = update.Name;
                cuurentStudent.Department = update.Department;
                cuurentStudent.Year = update.Year;
                cuurentStudent.RollNumber = update.RollNumber;
                _studentDbContext.SaveChanges();
            }
            else
            {
                return "Student Details NOT Found";
            }
            return "Student Details updated Successfully";
        }

        [HttpDelete("deleteStudent/{id}")]
        public async Task<ActionResult<string>> DeleteStudent(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Not a valid Student");
            }
            var currentStudent = _studentDbContext.Students.Where(s=>s.Id == Id).FirstOrDefault<Student>();
            if (currentStudent != null)
            {
                _studentDbContext.Entry(currentStudent).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _studentDbContext.SaveChanges();
            }
            else
            {
                return "Student Details not found";
            }
            return "Student Details Deleted Successfully";
        }
    }
}
