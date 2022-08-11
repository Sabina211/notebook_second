using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NotebookSecond.Data
{
    public class ApiWorkerData : IWorkerData
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient httpClient;
        private string url;
        private readonly ILogger<ApiWorkerData> logger;

        public ApiWorkerData(ILogger<ApiWorkerData> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            this.logger = logger;
            httpClient = _httpClientFactory.CreateClient("httpClient");
            url = httpClient.BaseAddress + "Workers";
        }

        public IEnumerable<Worker> GetWorkers()
        {
            try
            {
                string json = httpClient.GetStringAsync(url).Result;
                return JsonConvert.DeserializeObject<IEnumerable<Worker>>(json);
            }
            catch (AggregateException ex)
            {
                logger.LogError(ex, $"Нужно запустить приложение с апи\n {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }    
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
                logger.LogError($"Ошибка при обращении к апи result.StatusCode = {result.StatusCode}");
                logger.LogError($"Ошибка при обращении к апи result.StatusCode = {result.Content.ReadAsStringAsync().Result}");
                return false;
            }
            else return true;
        }
    }
}
