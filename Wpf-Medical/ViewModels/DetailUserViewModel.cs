

using System;
using Dbo;

namespace Wpf_Medical.ViewModels
{
    class DetailUserViewModel : BaseViewModel
    {
        private readonly User _selectedUser;
        private byte[] _connectedPicture;

        public DetailUserViewModel(User u)
        {
            _selectedUser = u;
            if (_selectedUser.Connected)
            {

            }
            else
            {
                
            }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
        }

        public String FullName
        {
            get { return _selectedUser.Firstname + " " + _selectedUser.Name; }
        }
    }
}
