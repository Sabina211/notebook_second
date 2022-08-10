using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNotebook.Models;

namespace WpfNotebook.ViewModels
{
    class MainWindowVM : Bindable
    {
        string baseUrl = @"https://localhost:5005/api/";
        HttpClient httpClient = new HttpClient();
        private object clientsData;
        public object ClientsData
        {
            get => clientsData;
            set
            {
                clientsData = value;
                OnPropertyChanged("ClientsData");
            }
        }
        private LoginUser loginUser;
        public LoginUser LoginUser
        {
            get => loginUser;
            set
            {
                loginUser = value;
                OnPropertyChanged("LoginUser");
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
        private string errorText = "";
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
        public ICommand LoginCommand { get; }
        public MainWindowVM()
        {
            LoginUser = new LoginUser();
            LoginCommand = new RelayCommand(obj =>
            {
                Login();
            });
                var workersList = GetWorkers();
            ClientsData = workersList;
        }

        private void Login()
        {
            if (LoginUser.Login == null || LoginUser.Password == null)
            {
                ErrorText = "Необходимо заполить обязательные поля";
                ErrorEnable = "Visible";
                return;
            }
            //string url = @"https://localhost:5005/api/Account/login";
            string url = baseUrl + "Account/login";
            var content = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(url, content).Result;
            if (!(result.StatusCode.ToString() == "OK"))
            {
                ErrorEnable = "Visible";
                ErrorText = $"Необходимо заполить обязательные поля. StatusCode ={result.StatusCode}\n {result.Content.ReadAsStringAsync().Result}";
                return;
            }
            UserWithRolesEdit loginedUser = JsonConvert.DeserializeObject<UserWithRolesEdit>(result.Content.ReadAsStringAsync().Result);
            ErrorEnable = "Visible";
            ErrorText = (loginedUser.UserRoles.Count != 0) ? $"Вы вошли как {loginedUser.UserName} с ролью {loginedUser.UserRoles[0]}"
            : $"Вы вошли как {loginedUser.UserName}, ролей нет";
            LoginUser.Login = "";
            LoginUser.Password = "";
        }

        public IEnumerable<Worker> GetWorkers()
        {
            string url = baseUrl + "Workers";
            //HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;
            var workers = JsonConvert.DeserializeObject<IEnumerable<Worker>>(result);
            return workers;
        }
    }
}
