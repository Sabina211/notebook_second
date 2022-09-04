using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfNotebook.Models;
using WpfNotebook.Views;

namespace WpfNotebook.ViewModels
{
    class MainWindowVM : Bindable
    {
        string baseUrl = @"http://localhost:83/api/";
        //string baseUrl = @"https://localhost:5005/api/";
        HttpClient httpClient = new HttpClient();
        public UserWithRolesEdit CurrentUser { get; set; } = null;
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
        private Worker currentWorker;
        public Worker CurrentWorker
        {
            get => currentWorker;
            set
            {
                currentWorker = value;
                OnPropertyChanged("CurrentWorker");
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
        public ICommand LogoutCommand { get; }
        public ICommand AddWorkerCommand { get; }
        public ICommand EditWorkerCommand { get; }
        public ICommand DeleteWorkerCommand { get; }

        public MainWindowVM()
        {
            LoginUser = new LoginUser();
            LoginCommand = new RelayCommand(obj =>
            {
                Login();
            });
            LogoutCommand = new RelayCommand(obj =>
            {
                Logout();
            });
            AddWorkerCommand = new RelayCommand(obj =>
            {
                if (CurrentUser == null) { MessageBox.Show("Для добавления сотрудника необходимо авторизоваться"); return; }
                AddWorkerWindow addClientWindow = new AddWorkerWindow(this, httpClient, baseUrl);
                addClientWindow.Show();
            });
            EditWorkerCommand = new RelayCommand(obj =>
            {
                if (CurrentUser == null || !CurrentUser.UserRoles.Contains("admin")) { MessageBox.Show("Для редактирования сотрудника необходимо авторизоваться с ролью админиcтратора.\n" +
                    " Редактирование и удаление сотрудников может осуществлять только пользователь с ролью администратора"); return; }
                EditWorkerWindow editClientWindow = new EditWorkerWindow(this, httpClient, baseUrl, CurrentWorker);
                editClientWindow.Show();
            });
            DeleteWorkerCommand = new RelayCommand(obj =>
            {
                if (CurrentUser == null || !CurrentUser.UserRoles.Contains("admin"))
                {
                    MessageBox.Show("Для удаления сотрудника необходимо авторизоваться с ролью админиcтратора.\n" +
                                    " Редактирование и удаление сотрудников может осуществлять только пользователь с ролью администратора"); return;
                }
                DeleteWorker();

            });
            UpdateWorkers();
        }

        private void DeleteWorker()
        {
            MessageBoxResult messagebox = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника с именем " +
                $"{CurrentWorker.Surname} {CurrentWorker.Name} {CurrentWorker.Patronymic}?", "Подтверждение операции", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (messagebox == MessageBoxResult.Yes)
            {
                var uri = new Uri(baseUrl + $"Workers/{CurrentWorker.Id}");
                var result = httpClient.DeleteAsync(uri).Result;
                if (!(result.StatusCode.ToString() == "OK"))
                {
                    MessageBox.Show("При удалении сотрудника произошла ошибка. Удаление не выполнено");
                }
                UpdateWorkers();
            }
        }

        private void Login()
        {
            if (LoginUser.Login == null || LoginUser.Password == null)
            {
                ErrorText = "Необходимо заполить обязательные поля";
                ErrorEnable = "Visible";
                return;
            }
            string url = baseUrl + "Account/login";
            var content = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(url, content).Result;
            if (!(result.StatusCode.ToString() == "OK"))
            {
                ErrorEnable = "Visible";
                ErrorText = $"Некорректные данные. StatusCode ={result.StatusCode}\n {result.Content.ReadAsStringAsync().Result}";
                return;
            }
            UserWithRolesEdit loginedUser = JsonConvert.DeserializeObject<UserWithRolesEdit>(result.Content.ReadAsStringAsync().Result);
            ErrorEnable = "Visible";
            ErrorText = (loginedUser.UserRoles.Count != 0) ? $"Вы вошли как {loginedUser.UserName} с ролью {loginedUser.UserRoles[0]}"
            : $"Вы вошли как {loginedUser.UserName}, ролей нет";
            CurrentUser = GetCurrentUser();
        }

        private void Logout()
        {
            string url = baseUrl + "Account/logout";
            var result = httpClient.PostAsync(url, null).Result;
            ErrorEnable = "Hidden";
            ErrorText = "";
            CurrentUser = null;
        }

        public IEnumerable<Worker> GetWorkers()
        {
            string url = baseUrl + "Workers";
            string result = httpClient.GetStringAsync(url).Result;
            var workers = JsonConvert.DeserializeObject<IEnumerable<Worker>>(result);
            return workers;
        }

        public UserWithRolesEdit GetCurrentUser()
        {
            string url = baseUrl + "Users/getCurrentUser";
            string json = httpClient.GetStringAsync(url).Result;
            var currentUsers = JsonConvert.DeserializeObject<UserWithRolesEdit>(json);
            return currentUsers;
        }

        public void UpdateWorkers()
        {
            var workersList = GetWorkers();
            ClientsData = workersList;
        }
    }
}
