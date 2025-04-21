using EmployAI.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Http;

namespace EmployAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;

        public HomeController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {


            return View();
        }

        [HttpGet]
        public IActionResult AIConsult()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AIConsult(UplodeFile model)
        {
            model.Username = HttpContext.Session.GetString("Username") ?? " ";

            if (model.Path != null && model.Path.Length > 0)
            {
                // Define the upload directory and create it if it doesn't exist
                var uploadDir = Path.Combine("AIConsultFile");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Create a unique file name
                var uniqueFileName = $"{Path.GetFileNameWithoutExtension(model.Path.FileName)}_{Guid.NewGuid()}{Path.GetExtension(model.Path.FileName)}";
                var pathToSaveDb = Path.Combine(uploadDir, uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(pathToSaveDb, FileMode.Create))
                {
                    await model.Path.CopyToAsync(stream);
                }

                // Prepare to call the stored procedure
                string message;
                int responseCode;
                //string connectionString = "Server=.;Database=EmployAI;User Id=Naqleh;Password=Ab@12345;";
                string connectionString = "Server = .; Database = EmployAI; Trusted_Connection = True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand("AddAIConsult", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserName", model.Username);
                        command.Parameters.AddWithValue("@Path", pathToSaveDb);
                        command.Parameters.AddWithValue("@Lang", model.Lang);

                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(messageParam);

                        SqlParameter responseCodeParam = new SqlParameter("@ResponseCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(responseCodeParam);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();

                        message = messageParam.Value.ToString();
                        responseCode = (int)responseCodeParam.Value;

                        if (responseCode == 0)
                        {
                            TempData["SuccessMessage"] = message;
                            return RedirectToAction("AIConsult");
                        }
                    }
                }
            }

            ModelState.AddModelError("", "File upload failed.");
            return View("AIConsult", model);
        }
        [HttpGet]
        public IActionResult Analyze()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Analyze(AnalyzeForm model)
        {
            var username = HttpContext.Session.GetString("Username") ?? " ";
            model.Username = username;
            // Use a list to collect messages and response codes
            var messages = new List<string>();
            var responseCodes = new List<int>();
            //string connectionString = "Server=.;Database=EmployAI;User Id=Naqleh;Password=Ab@12345;";
            string connectionString = "Server = .; Database = EmployAI; Trusted_Connection = True;";
            var filesToUpload = new List<IFormFile>();
            var languages = new List<string>();



            for (int i = 0; i < model.Path.Count; i++)
            {
                var file = model.Path[i];
                var language = model.Lang[i];

                if (file != null && file.Length > 0)
                {
                    var allowedExtensions = new[] { ".pdf", ".docx", ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["ErrorMessage"] = "Only PDF, DOCX, or image files are allowed.";
                        return RedirectToAction("Analyze");
                    }

                    filesToUpload.Add(file);
                    languages.Add(language);

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "AnalyzeFile");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var pathToSaveDb = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(pathToSaveDb, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Prepare to call the stored procedure
                    string message;
                    int responseCode;

                    using (var connection = new SqlConnection(connectionString))
                    {
                        using (var command = new SqlCommand("AddAnalyse", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@UserName", username);
                            command.Parameters.AddWithValue("@Path", pathToSaveDb);
                            command.Parameters.AddWithValue("@Lang", language);

                            SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                            {
                                Direction = ParameterDirection.Output
                            };
                            command.Parameters.Add(messageParam);

                            SqlParameter responseCodeParam = new SqlParameter("@ResponseCode", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            command.Parameters.Add(responseCodeParam);

                            connection.Open();
                            await command.ExecuteNonQueryAsync();

                            message = messageParam.Value.ToString();
                            responseCode = (int)responseCodeParam.Value;

                            messages.Add(message);
                            responseCodes.Add(responseCode);
                        }
                    }
                }
            }

            var isSingleFile = filesToUpload.Count == 1;

            if (isSingleFile)
            {
                // إرسال طلب POST إلى /upload-cv
                var file = filesToUpload.First();
                var language = languages.First();

                var success = await UploadSingleCv(file, language);
                if (success)
                {
                    TempData["SuccessMessage"] = "File uploaded successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "File upload failed.";
                }
            }
            else
            {
                var success = await UploadMultipleCvs(filesToUpload, languages);
                if (success)
                {
                    TempData["SuccessMessage"] = "All files uploaded successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Some files failed to upload.";
                }
            }

            return RedirectToAction("Analyze");
        }

        [HttpGet]
        public IActionResult feedback()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult feedback(FeedBack model)
        {
            model.UserName = HttpContext.Session.GetString("Username");
            if (ModelState.IsValid)
            {

                string connectionString = "Server = .; Database = EmployAI; Trusted_Connection = True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("AddNewFeedBack", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserName", model.UserName);
                        command.Parameters.AddWithValue("@DetailsFeedBack", model.DetailsFeedBack);

                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(messageParam);

                        SqlParameter responseCodeParam = new SqlParameter("@ResponseCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(responseCodeParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        string message = messageParam.Value.ToString();
                        int responseCode = (int)responseCodeParam.Value;

                        if (responseCode == 0) // Success
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        ModelState.AddModelError("", message); // Add error message
                    }
                }
            }
            return View(model);
        }

        public IActionResult login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = "Server = .; Database = EmployAI; Trusted_Connection = True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("RegisterUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@Username", model.Username);
                        command.Parameters.AddWithValue("@Password", model.Password);

                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(messageParam);

                        SqlParameter responseCodeParam = new SqlParameter("@ResponseCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(responseCodeParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        string message = messageParam.Value.ToString();
                        int responseCode = (int)responseCodeParam.Value;

                        if (responseCode == 0) // Success
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        ModelState.AddModelError("", message); // Add error message
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = "Server = .; Database = EmployAI; Trusted_Connection = True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("LoginUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Username", model.Username);
                        command.Parameters.AddWithValue("@Password", model.Password);

                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(messageParam);

                        SqlParameter responseCodeParam = new SqlParameter("@ResponseCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(responseCodeParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        string message = messageParam.Value.ToString();
                        int responseCode = (int)responseCodeParam.Value;

                        if (responseCode == 0) // Success
                        {
                            HttpContext.Session.SetString("Username", model.Username);
                            return RedirectToAction("Index", "Home");
                        }
                        ModelState.AddModelError("", message); // Add error message
                    }
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }


        [HttpGet("search-resumes")]
        public async Task<IActionResult> SearchResumes(string text, int? years_of_experience, string education, string job_title, int? limit)
        {
            var api = _configuration["BaseAPIUrl"];

            // Validation for the required 'text' parameter
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("Text parameter is required.");
            }

            _httpClient.DefaultRequestHeaders.Add("x-api-key", "DEMO-API-KEY");

            // Construct the external API URL
            var url = $"{api}/search-resumes?text={text}&years_of_experience={years_of_experience}&education={education}&job_title={job_title}&limit={limit}";

            try
            {
                // Send GET request to external API
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error fetching data from external API.");
                }

                // Read the response content as a string
                var data = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Data: {data}");
                // Return the response data as JSON
                return Ok(JsonConvert.DeserializeObject<List<Resume>>(data));
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<bool> UploadSingleCv(IFormFile file, string language)
        {
            var api = _configuration["BaseAPIUrl"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", "DEMO-API-KEY");


                var formData = new MultipartFormDataContent();

                // Add file content
                var fileContent = new StreamContent(file.OpenReadStream());
                formData.Add(fileContent, "file", file.FileName);

                // Add language as string content
                formData.Add(new StringContent(language), "language");

                // Make the HTTP POST request
                var response = await client.PostAsync($"{api}/upload-cv", formData);

                // Return whether the request was successful
                return response.IsSuccessStatusCode;
            }
        }


        private async Task<bool> UploadMultipleCvs(List<IFormFile> files, List<string> languages)
        {

            var api = _configuration["BaseAPIUrl"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", "DEMO-API-KEY");

                var formData = new MultipartFormDataContent();

                // Loop through files and languages and add them to the form data
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var language = languages[i];

                    var fileContent = new StreamContent(file.OpenReadStream());
                    formData.Add(fileContent, "files", file.FileName); // Add the file to the form data

                    formData.Add(new StringContent(language), "language"); // Add the corresponding language
                }

                // Send the POST request with the form data
                var response = await client.PostAsync($"{api}/upload-cvs", formData);

                // Return whether the request was successful
                return response.IsSuccessStatusCode;
            }
        }

    }
}
