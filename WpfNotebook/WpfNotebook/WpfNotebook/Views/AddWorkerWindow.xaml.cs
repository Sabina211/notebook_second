using MahApps.Metro.Controls;
using System.Net.Http;
using WpfNotebook.ViewModels;

namespace WpfNotebook.Views
{
    /// <summary>
    /// Логика взаимодействия для AddWorkerWindow.xaml
    /// </summary>
    public partial class AddWorkerWindow : MetroWindow
    {
        internal AddWorkerWindow(MainWindowVM mainWindowVM, HttpClient httpClient, string baseUrl)
        {
            InitializeComponent();
            DataContext = new AddWorkerVM(mainWindowVM, httpClient, baseUrl);
        }
    }
}
