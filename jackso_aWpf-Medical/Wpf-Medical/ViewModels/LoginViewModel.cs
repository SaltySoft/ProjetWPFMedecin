using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf_Medical.DataAccess;
using Wpf_Medical.ServiceUser;
using Wpf_Medical.Views;

namespace Wpf_Medical.ViewModels
{
    class LoginViewModel : BaseViewModel
    {

        #region variables
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
            _loginCommand = new RelayCommand(param => LoginAccess(), param => true);
        }
        #endregion

        #region methodes

        /// <summary>
        /// Login de l'utilisateur avec le DataAccess
        /// </summary>
        /// 
        private async void LoginAccess()
        {
            try
            {
                Task<bool> isConnected = UsersClient.Instance.TestUser(_login, _password);
                if (await isConnected)
                {

                    ServiceUser.User u = UsersClient.Instance.GetUser(_login);

                    var window = new MainWindow();
                    var vm = new MainWindowViewModel(u);
                    window.DataContext = vm;
                    window.Show();
                    CloseSignal = true;
                }
                else
                {
                    MessageBox.Show("Cet utilisateur n'existe pas !");
                }
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Impossible d' acceder aux web Services");
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
