﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Dbo;
using Wpf_Medical.DataAccess;
using Wpf_Medical.Views;

namespace Wpf_Medical.ViewModels
{
    class UsersViewModel : BaseViewModel
    {
        #region variables

        private UserControl _currentView;

        private User _selectedUser;

        private ICommand _addUserButtonCommand;
        private ICommand _removeUserButtonCommand;

        #endregion

        #region constructeur
        /// <summary>
        /// Construit la vue des utilisateurs
        /// </summary>
        public UsersViewModel()
        {
            UsersClient.Instance.RefreshUsers();
        }
        #endregion

        #region methodes
        /// <summary>
        /// Charge la vue d' ajout d'utilisateur
        /// </summary>
        private void LoadAddView()
        {
            var addControl = new AddUserControl();
            var addVM = new AddUserViewModel();
            addControl.DataContext = addVM;
            CurrentView = addControl;
        }

        /// <summary>
        /// charge la vue detaille de l' utilisateur selectionne
        /// </summary>
        private void LoadDetailView()
        {
            var detailControl = new DetailUserControl();
            var detailVM = new DetailUserViewModel(_selectedUser);
            detailControl.DataContext = detailVM;
            CurrentView = detailControl;
        }

        /// <summary>
        /// supprime l' utilisateur selectionne
        /// </summary>
        private void RemoveUser()
        {
            if (_selectedUser != null)
            {
                UsersClient.Instance.RemoveUser(_selectedUser.Login);
                SelectedUser = UsersList.FirstOrDefault();
            }
        }
        #endregion

        #region getters / setters

        #region commands
        /// <summary>
        /// bouton d' ajout de l' utilisateur
        /// </summary>
        public ICommand AddUserButtonCommand
        {
            get
            {
                return _addUserButtonCommand ?? (_addUserButtonCommand = new RelayCommand(
                    param => LoadAddView(),
                    param => true
                    ));
            }
        }

        /// <summary>
        /// bouton de suppression de l' utilisateur
        /// </summary>
        public ICommand RemoveUserButtonCommand
        {
            get
            {
                return _removeUserButtonCommand ?? (_removeUserButtonCommand = new RelayCommand(
                    param => RemoveUser(),
                    param => true
                    ));
            }
        }
        #endregion


        #region attribus
        /// <summary>
        /// liste des utilisateurs
        /// </summary>
        public ObservableCollection<User> UsersList
        {
            get { return UsersClient.Instance.GetUsers(); }
        }

        /// <summary>
        /// sous-vue actuelle (vue d' ajout ou vue detaille d' un utilisateur)
        /// </summary>
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (!(Equals(_currentView, value)))
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// l' utilisateur selectionne
        /// </summary>
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged();
                    LoadDetailView();
                }
            }
        }
        #endregion

        #endregion


    }
}
