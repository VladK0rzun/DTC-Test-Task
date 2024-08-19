using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.ViewModels;
using WpfApp1.Views;

namespace WpfApp1.Services
{
    public class PageService
    {
        public event Action<Page> OnPageChanged;

        public void ChangePage(Page page)
        {
            OnPageChanged?.Invoke(page);
        }

        public void ShowMainWindow()
        {
            var mainWindow = new Main();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}
