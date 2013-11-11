﻿using System.Windows.Controls;
using System.Windows.Input;
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

        /// <summary>
        /// Commands
        /// </summary>
        private ICommand _patientsButtonCommand;
        private ICommand _usersButtonCommand;
        #endregion
        

        #region constructeur
        
        public MainWindowViewModel()
        {
            LoadUsersView();
        }

        #endregion

        #region methodes

        private void LoadUsersView()
        {
            var userControl = new UsersViewControl();
            var userVM = new UsersViewModel();
            userControl.DataContext = userVM;
            CurrentView = userControl;
        }

        private void LoadPatientsView()
        {
            var patientControl = new PatientsViewControl();
            var patientsVm = new PatientsViewModel();
            patientControl.DataContext = patientsVm;
            CurrentView = patientControl;
        }

        /*
        private void LoadUserDetailedControl()
        {
            // load detail user view
            Views.DetailUserControl detailUserView = new Views.DetailUserControl();
            ViewModels.DetailUserViewModel vm = new DetailUserViewModel();
            detailUserView.DataContext = vm;
            CurrentView = detailUserView;
        } */

        #endregion

        #region getter / setters

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

        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (!(_currentView == value))
                {
                    _currentView = value;
                    OnPropertyChanged("CurrentView");
                }
            }
        }


        #endregion

    }
}