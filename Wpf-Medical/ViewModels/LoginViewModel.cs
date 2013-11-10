using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf_Medical.ServiceUser;
using Wpf_Medical.Views;

namespace Wpf_Medical.ViewModels
{
    class LoginViewModel : BaseViewModel
    {

        #region variables
        private DataAccess.Users _dataUsers = null;
        private bool _closeSignal;

        private String _login;
        private String _password;

        private ICommand _loginCommand;
        #endregion

        #region constructeur
        /// <summary>
        /// Initialise la vue avec les champs vides
        /// </summary>
        public LoginViewModel()
        {
            _login = "";
            _password = "";
            _dataUsers = new DataAccess.Users();

            _loginCommand = new RelayCommand(param => LoginAccess(), param => true);
        }
        #endregion

        #region methodes

        /// <summary>
        /// Login de l'utilisateur avec le DataAccess
        /// </summary>
        /// 
        private void LoginAccess()
        {
            if (_dataUsers.TestUser(_login, _password))
            {
                MainWindow window = new MainWindow();
                MainWindowViewModel vm = new MainWindowViewModel();
                window.DataContext = vm;
                window.Show();
                CloseSignal = true;
            }
            else
            {
                ServiceUser.ServiceUserClient client = new ServiceUserClient();
                client.AddUser(new ServiceUser.User()
                {
                    Login = "root",
                    Pwd = "root"
                });
                MessageBox.Show("login nok");
  
            }
        }
        #endregion

        #region getters / setters
        /// <summary>
        /// Close signal
        /// </summary>
        public bool CloseSignal
        {
            get { return _closeSignal; }
            set
            {
                if (_closeSignal != value)
                {
                    _closeSignal = value;
                    OnPropertyChanged("CloseSignal");
                }
            }
        }

        /// <summary>
        /// pour le bouton de login
        /// </summary>
        public ICommand LoginCommand
        {
            get { return _loginCommand; }
            set { _loginCommand = value; }
        }

        /// <summary>
        /// la textbox de login
        /// </summary>
        public String Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged("Login");
                }
            }
        }

        /// <summary>
        /// la passbox de login
        /// </summary>
        public String Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        #endregion
    }
}
