using System;
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

        public UsersViewModel()
        {
            UsersClient.Instance.RefreshUsers();
        }

        private void LoadAddView()
        {
            var addControl = new AddUserControl();
            var addVM = new AddUserViewModel();
            addControl.DataContext = addVM;
            CurrentView = addControl;
        }

        private void LoadDetailView()
        {
            var detailControl = new DetailUserControl();
            var detailVM = new DetailUserViewModel(_selectedUser);
            detailControl.DataContext = detailVM;
            CurrentView = detailControl;
        }

        private void RemoveUser()
        {
            if (_selectedUser != null)
            {
                UsersClient.Instance.RemoveUser(_selectedUser.Login);
                SelectedUser = UsersList.FirstOrDefault();
            }
        }

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


        public ObservableCollection<User> UsersList
        {
            get { return UsersClient.Instance.GetUsers(); }
        }


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

        
    }
}
