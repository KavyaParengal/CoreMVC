using System.Text;
using Microsoft.AspNetCore.Mvc;
using MVCExample1.Models;
using Newtonsoft.Json;

namespace MVCExample1.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult EmployeeReport()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult InsertEmployee()
        {
            return View();
        }

        public IActionResult ViewEmployeeReport_Model()
        {
            return View();
        }

        public IActionResult InsertEmployeeModel()
        {
            return View();
        }

        public string getAPIData(string datas)     //Get API Response
        {
            // Split the input string 'datas' using '$' as the delimiter
            //string[] datastring = datas.Split("$");
            // Construct the API path using the second and first elements of the split array
            string ApiPath = "http://localhost:5051/" + datas;

            // Create an instance of HttpClient to make the HTTP request
            using (var client = new HttpClient())
            {
                // Initialize a variable to hold the response data
                string data = "";
                // Set the base address of the HttpClient to the constructed API path
                client.BaseAddress = new Uri(ApiPath);
                // Make a GET request to the API and wait for the result
                HttpResponseMessage result = client.GetAsync(client.BaseAddress).Result;
                // Check if the response indicates success
                if (result.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    data = result.Content.ReadAsStringAsync().Result;
                }
                // Return the response data 
                return data;
            }

        }

        public async Task<dynamic> postAPIData(string datas)
        {
            string[] dataString1 = datas.Split("$");

            // Define the API URL end point
            string apiPath = "http://localhost:5051/" + dataString1[0];

            //Initialize a variable to hold the response data
            var data = "";

            //Split the input string 'dataString1[1]' using '~' as the delimiter
            string[] dataString2 = dataString1[1].Split("~");

            //create an instance of HttpClient to make the HTTP request
            using (HttpClient client = new HttpClient()) 
            { 
                //Serialize the data 
                string content = JsonConvert.SerializeObject(new
                {
                    id = dataString2[0],
                    name = dataString2[1],
                    designation = dataString2[2],
                    department = dataString2[3]
                });
                //Convert the JSON string to a byte array
                var buffer=Encoding.UTF8.GetBytes(content);
                //create ByteArrayContent from the byte  array
                var byteContent=new ByteArrayContent(buffer);
                //Set the content type to JSON
                byteContent.Headers.ContentType = new
                    System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //Make the POST request to the API
                HttpResponseMessage result= await client.PostAsync(apiPath, byteContent);
                //check if the response indicates success
                if (result.IsSuccessStatusCode)
                {
                    data=result.Content.ReadAsStringAsync().Result;
                }
            }
            //return the response data
            return data;
        }

        public List<EmployeeModel> getAPIData_model(string datas)     //Get API Response
        {
            // Split the input string 'datas' using '$' as the delimiter
            //string[] datastring = datas.Split("$");
            // Construct the API path using the second and first elements of the split array
            string ApiPath = "http://localhost:5051/" + datas;

            // Create an instance of HttpClient to make the HTTP request
            using (var client = new HttpClient())
            {
                // Initialize a variable to hold the response data
                List<EmployeeModel> employees = new List<EmployeeModel>();
                // Set the base address of the HttpClient to the constructed API path
                client.BaseAddress = new Uri(ApiPath);
                // Make a GET request to the API and wait for the result
                HttpResponseMessage result = client.GetAsync(client.BaseAddress).Result;
                // Check if the response indicates success
                if (result.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    var jsonData = result.Content.ReadAsStringAsync().Result;
                    var apiResponse = JsonConvert.DeserializeObject<List<dynamic>>(jsonData);
                    foreach(var item in apiResponse)
                    {
                        var employee = new EmployeeModel
                        {
                            EmpCode = item.id,
                            EmpName = item.name,
                            Department = item.department,
                            Designation = item.designation,   
                        };
                        employees.Add(employee);
                    }
                }
                // Return the response data 
                return employees;
            }

        }

        public async Task<IActionResult> postAPIData_model([FromBody] EmployeeModel employee)
        {

            // Define the API URL end point
            string apiPath = "http://localhost:5051/insertEmployee";

            //Initialize a variable to hold the response data
            var apiData = new
            {
                id = employee.EmpCode,
                name = employee.EmpName,
                department = employee.Department,
                designation = employee.Designation,
            };
            var data = "";
            //create an instance of HttpClient to make the HTTP request
            using (HttpClient client = new HttpClient())
            {
                //Serialize the data 
                string content = JsonConvert.SerializeObject(apiData);
                //Convert the JSON string to a byte array
                var buffer = Encoding.UTF8.GetBytes(content);
                //create ByteArrayContent from the byte  array
                var byteContent = new ByteArrayContent(buffer);
                //Set the content type to JSON
                byteContent.Headers.ContentType = new
                    System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //Make the POST request to the API
                HttpResponseMessage result = await client.PostAsync(apiPath, byteContent);
                //check if the response indicates success
                if (result.IsSuccessStatusCode)
                {
                    data = await result.Content.ReadAsStringAsync();
                    return Ok(data);
                }
                else
                {
                    return StatusCode((int)result.StatusCode, "Error");
                }
            }
            //return the response data
            //return data;
        }
    }
}