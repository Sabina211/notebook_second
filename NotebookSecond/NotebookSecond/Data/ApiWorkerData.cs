using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NotebookSecond.Data
{
    public class ApiWorkerData : IWorkerData
    {
        private HttpClient httpClient { get; set; }
        private string url = @"https://localhost:5005/api/Workers";
        private readonly ILogger<ApiWorkerData> logger;

        public ApiWorkerData(ILogger<ApiWorkerData> logger, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public IEnumerable<Worker> GetWorkers()
        {
            string json = httpClient.GetStringAsync(url).Result;
          
            return JsonConvert.DeserializeObject<IEnumerable<Worker>>(json);

        }

        public Worker AddWorker(Worker worker)
        {
            var content = new StringContent(JsonConvert.SerializeObject(worker), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(url, content).Result;
            bool success = CheckResult(result);
            if (success)
            {
                try
                {
                    Worker newWorker = JsonConvert.DeserializeObject<Worker>(result.Content.ReadAsStringAsync().Result);
                    return newWorker;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "При добавлении сотрудника не удалось преобразовать ответ в Worker");
                    throw;
                }
            }
            return new Worker { Id=Guid.Empty};
        }

        [Authorize]
        public Worker EditWorker(Worker worker)
        {
            var result = httpClient.PutAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(worker), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;
            bool success = CheckResult(result);
            if (success)
            {
                try
                {
                    Worker editedWorker = JsonConvert.DeserializeObject<Worker>(result.Content.ReadAsStringAsync().Result);
                    return editedWorker;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "При изменении сотрудника не удалось преобразовать ответ в Worker");
                    throw;
                }
            }
            return new Worker { Id = Guid.Empty };
        }

        [Authorize]
        public bool RemoveWorker(Worker worker)
        {
            var uri = new Uri(url + $"/{worker.Id}") ;
            var result = httpClient.DeleteAsync(uri).Result;
            return CheckResult(result); ;
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
