using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_Medical.DataAccess;
using Wpf_Medical.Views;

namespace Wpf_Medical.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {

        #region variables
        /// <summary>
        /// UserControls
        /// </summary>
        private UserControl _currentView;

        private String _userLogin;

        /// <summary>
        /// Commands
        /// </summary>
        private ICommand _patientsButtonCommand;
        private ICommand _usersButtonCommand;
        private ICommand _logoutCommand;

        private bool _closeSignal;


        #endregion
        

        #region constructeur
        
        public MainWindowViewModel(String userLogin)
        {
            _userLogin = userLogin;
            LoadUsersView();
        }

        #endregion

        #region methodes

        /// <summary>
        /// charge la vue des utilisateurs
        /// </summary>
        private void LoadUsersView()
        {
            var userControl = new UsersViewControl();
            var userVM = new UsersViewModel();
            userControl.DataContext = userVM;
            CurrentView = userControl;
        }

        /// <summary>
        /// charge la vue des patients
        /// </summary>
        private void LoadPatientsView()
        {
            var patientControl = new PatientsViewControl();
            var patientsVm = new PatientsViewModel();
            patientControl.DataContext = patientsVm;
            CurrentView = patientControl;
        }
        
        /// <summary>
        /// Deconnexion de l' utilisateur
        /// </summary>
        private void Logout()
        {
            Task dec = UsersClient.Instance.DisconnectUser(_userLogin);
            if (dec != null)
            {
                var loginVM = new LoginViewModel();
                var loginWin = new LoginWindow {DataContext = loginVM};
                loginWin.Show();
                CloseSignal = true;   
            }
        }


        #endregion

        #region getter / setters
        /// <summary>
        /// Bouton pour affichier les utilisateurs
        /// </summary>
        public ICommand UserButtonCommand
        {
            get
            {
                return _usersButtonCommand ?? (_usersButtonCommand = new RelayCommand(
                    param => LoadUsersView(),
                    param => true
                    ));
            }
        }
        /// <summary>
        /// Bouton pour se deconnecter
        /// </summary>
        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ?? (_logoutCommand = new RelayCommand(
                    param => Logout(),
                    param  => true
                    ));
            }
        }

        /// <summary>
        /// Bouton pour afficher les patients
        /// </summary>
        public ICommand PatientButtonCommand
        {
            get
            {
                if (_patientsButtonCommand == null)
                {
                    _patientsButtonCommand = new RelayCommand(
                        param => LoadPatientsView(),
                        param => true
                    );
                }
                return _patientsButtonCommand;
            }
        }

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
        /// Getter pour la vue principale (user / patient)
        /// </summary>
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (!Equals(_currentView, value))
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }


        #endregion

    }
}
