using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfNotebook.Models;
using WpfNotebook.Views;

namespace WpfNotebook.ViewModels
{
    class AddWorkerVM : Bindable
    {
        private HttpClient httpClient { get; set; }
        private Worker newWorker;
        public Worker NewWorker
        {
            get => newWorker;
            set
            {
                newWorker = value;
                OnPropertyChanged("NewWorker");
            }
        }
        private string errorEnable = "Hidden";
        public string ErrorEnable
        {
            get
            {
                return errorEnable;
            }
            set
            {
                errorEnable = value;
                OnPropertyChanged("ErrorEnable");
            }
        }
        private string errorText;
        public string ErrorText
        {
            get
            {
                return errorText;
            }
            set
            {
                errorText = value;
                OnPropertyChanged("ErrorText");
            }
        }
        public ICommand SaveCommand { get; }

        public AddWorkerVM(MainWindowVM mainWindowVM, HttpClient httpClient, string baseUrl)
        {
            NewWorker = new Worker();
            this.httpClient = httpClient;
            SaveCommand = new RelayCommand(obj =>
            {
                if (NewWorker.Name==null || NewWorker.Name == "")
                {
                    ErrorEnable = "Visible";
                    ErrorText = "Необходимо заполнить обязательные поля";
                    return;
                }
                ErrorEnable = "Hidden";
                var content = new StringContent(JsonConvert.SerializeObject(NewWorker), Encoding.UTF8, "application/json");
                string url = baseUrl + "Workers";
                var result = httpClient.PostAsync(url, content).Result;
                if (!(result.StatusCode.ToString() == "OK"))
                {
                    ErrorText = $"Ошибка.  StatusCode ={result.StatusCode}\n {result.Content.ReadAsStringAsync().Result}";
                    ErrorEnable = "Visible";
                    return;
                }
                Window window = Application.Current.Windows.OfType<AddWorkerWindow>().SingleOrDefault(w => w.IsActive);
                mainWindowVM.UpdateWorkers();
                window.Close();

            });

        }
    }
}
