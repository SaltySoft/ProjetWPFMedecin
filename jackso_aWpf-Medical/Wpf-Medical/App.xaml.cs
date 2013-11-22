using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf_Medical.ViewModels;
using Wpf_Medical.Views;

namespace Wpf_Medical
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoginWindow window = new LoginWindow();
            LoginViewModel vm = new LoginViewModel();
            window.DataContext = vm;
            window.Show();
        }
    }
}
