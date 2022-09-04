using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfNotebook.Models;
using WpfNotebook.Views;

namespace WpfNotebook.ViewModels
{
    class EditWorkerVM : Bindable
    {
        private readonly HttpClient _httpClient;
        private Worker editWorker;
        public Worker EditWorker
        {
            get => editWorker;
            set
            {
                editWorker = value;
                OnPropertyChanged("EditWorker");
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
        public ICommand SaveCommand
        {
            get;
        }
        public EditWorkerVM(MainWindowVM mainWindowVM, HttpClient httpClient, string baseUrl, Worker currentWorker)
        {
            EditWorker = new Worker 
            {
                Id = currentWorker.Id,
                Surname = currentWorker.Surname,
                Name = currentWorker.Name,
                Patronymic = currentWorker.Patronymic,
                PhoneNumber = currentWorker.PhoneNumber,
                Address = currentWorker.Address,
                Description = currentWorker.Description
            };
            _httpClient = httpClient;
            SaveCommand = new RelayCommand(obj =>
            {
                if (EditWorker.Name == null || EditWorker.Name == "")
                {
                    ErrorEnable = "Visible";
                    ErrorText = "Необходимо заполнить обязательные поля";
                    return;
                }
                ErrorEnable = "Hidden";
                var content = new StringContent(JsonConvert.SerializeObject(EditWorker), Encoding.UTF8, "application/json");
                string url = baseUrl + "Workers";
                var result = httpClient.PutAsync(url, content).Result;
                if (!(result.StatusCode.ToString() == "OK"))
                {
                    ErrorText = $"Ошибка.  StatusCode ={result.StatusCode}\n {result.Content.ReadAsStringAsync().Result}";
                    ErrorEnable = "Visible";
                    return;
                }
                Window window = Application.Current.Windows.OfType<EditWorkerWindow>().SingleOrDefault(w => w.IsActive);
                mainWindowVM.UpdateWorkers();
                window.Close();

            });

        }
    }
}
