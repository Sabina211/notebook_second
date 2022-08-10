using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotebook.Models;

namespace WpfNotebook.ViewModels
{
    class MainWindowVM : Bindable
    {
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

        public MainWindowVM()
        {
            List<Worker> workers = new List<Worker>();
            workers.Add(new Worker { 
                Name="Mark",
                Surname="Markov", 
                Id=Guid.Empty, 
                Address="Марковая улица", 
                Description = "описание", 
                Patronymic="Маркович",
                PhoneNumber="8952132654987"});
            ClientsData = workers;
        }
    }
}
