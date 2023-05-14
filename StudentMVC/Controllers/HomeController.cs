using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAPI.Model;
using StudentMVC.Models;
using System.Data;
using System.Diagnostics;

namespace StudentMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        string baseURL = "https://localhost:7009";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public async Task<IActionResult> DisplayStudent()
        {
            // Calling the Web API
            IList<StudentEntity> students = new List<StudentEntity>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("/api/Students");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    students = JsonConvert.DeserializeObject<List<StudentEntity>>(results);
                }
                else
                {
                    Console.WriteLine("Error Calling Student API");
                }
                ViewData.Model = students;
            }
            return View();
        }

        public async Task<ActionResult<String>> AddStudent(StudentEntity student)
        {
            StudentEntity studentEntity = new StudentEntity()
            {
                Id = student.Id,
                Name = student.Name,
                Department = student.Department,
                Year = student.Year,
                RollNumber = student.RollNumber
            };
            if (student.Id != 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL + "/api/Students/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage getData = await client.PostAsJsonAsync<StudentEntity>("addStudent", student);
                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        Console.WriteLine("Error Calling Student API");
                    }
                }
            }
            return View();
        }
        public async Task<ActionResult<String>> UpdateStudent(StudentEntity updates)
        {
            var update = new StudentEntity();
            if(update != null)
            {
                update.Id = updates.Id;
                update.Name = updates.Name;
                update.Department = updates.Department;
                update.Year = updates.Year;
                update.RollNumber = updates.RollNumber;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL + "/api/Students/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.PostAsJsonAsync<StudentEntity>("updateStudent", updates);
                if (getData.IsSuccessStatusCode)
                {
                    return RedirectToAction("DisplayStudent", "Home");
                }
                else
                {
                    Console.WriteLine("Cannot Update Student Data");
                }
            }
            return View();
        }
        
        public ActionResult DeleteStudent (int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7009/api/Students/");
                var deleteTask = client.DeleteAsync("deleteStudent/" + id.ToString());

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("DisplayStudent");
                }
            }
            return RedirectToAction("DisplayStudent");
        }
    }
}