using MahApps.Metro.Controls;
using System.Net.Http;
using WpfNotebook.Models;
using WpfNotebook.ViewModels;

namespace WpfNotebook.Views
{
    /// <summary>
    /// Логика взаимодействия для EditWorkerWindow.xaml
    /// </summary>
    public partial class EditWorkerWindow : MetroWindow
    {
        internal EditWorkerWindow(MainWindowVM mainWindowVM, HttpClient httpClient, string baseUrl, Worker currentWorker)
        {
            InitializeComponent();
            DataContext = new EditWorkerVM(mainWindowVM, httpClient, baseUrl, currentWorker);
        }
    }
}
