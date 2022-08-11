using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfNotebook.Models;
using WpfNotebook.ViewModels;

namespace WpfNotebook.Views
{
    /// <summary>
    /// Логика взаимодействия для EditWorkerWindow.xaml
    /// </summary>
    public partial class EditWorkerWindow : Window
    {
        internal EditWorkerWindow(MainWindowVM mainWindowVM, HttpClient httpClient, string baseUrl, Worker currentWorker)
        {
            InitializeComponent();
            DataContext = new EditWorkerVM(mainWindowVM, httpClient, baseUrl, currentWorker);
        }
    }
}
