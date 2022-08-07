using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NotebookSecond.Entities;
using NotebookSecond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NotebookSecond.Data
{
    public class ApiAccountData : IAccountData
    {
        private HttpClient httpClient { get; set; }
        //private string url = @"https://localhost:5005/api/Account/login";
        private readonly ILogger<ApiAccountData> logger;
        public ApiAccountData(ILogger<ApiAccountData> logger, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }
        public async Task<bool> Login(LoginUser loginUser)
        {
            string url = @"https://localhost:5005/api/Account/login";
            var content = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(url, content).Result;
            var cookies = result.Headers.GetValues(HeaderNames.SetCookie);
            httpClient.DefaultRequestHeaders.Add("Cookie", cookies);
            bool success = CheckResult(result);

            /*Worker newWorker =  new ApiWorkerData(httpClient).AddWorker(new Worker()
            {
                Name = "Markus"
            });*/

            return true;
        }

        public async Task<bool> Registration(RegisterUser registerUser)
        {
            string url = @"https://localhost:5005/api/Account/register";
            var content = new StringContent(JsonConvert.SerializeObject(registerUser), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(url, content).Result;
            bool success = CheckResult(result);
            return success;
        }

        public void Logout()
        {
            string url = @"https://localhost:5005/api/Account/logout";
            var result = httpClient.PostAsync(url, null).Result;
        }
        private bool CheckResult(HttpResponseMessage result)
        {
            if (!(result.StatusCode.ToString() == "OK"))
            {
                logger.LogError($"Ошибка при обращении к апи result.StatusCode = {result.StatusCode}\n {result.Content.ReadAsStringAsync().Result}");
                return false;
            }
            else return true;
        }

    }
}
