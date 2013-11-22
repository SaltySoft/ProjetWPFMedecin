using System;
using System.IO;


namespace Wpf_Medical.ViewModels
{
    class DetailUserViewModel : BaseViewModel
    {
        #region variables

        private readonly ServiceUser.User _selectedUser;
        private String _connectedPicture;

        #endregion

        #region constructeur
        public DetailUserViewModel(ServiceUser.User u)
        {
            _selectedUser = u;
            string filePath;
            if (_selectedUser.Connected == true)
            {
                filePath = "Resources/Images/check.png";
            }
            else
            {
                filePath = "Resources/Images/uncheck.png";
            }
  

            var info = new FileInfo(filePath);
            ConnectedPicture = info.FullName;
        }
        #endregion

        #region getters / setters
        /// <summary>
        /// Utilisateur selectionne
        /// </summary>
        public ServiceUser.User SelectedUser
        {
            get { return _selectedUser; }
        }

        /// <summary>
        /// Nom complet de l' utilisateur
        /// </summary>
        public String FullName
        {
            get { return _selectedUser.Firstname + " " + _selectedUser.Name; }
        }

        public String ConnectedPicture
        {
            get { return _connectedPicture; }
            set
            {
                if (_connectedPicture != value )
                {
                    _connectedPicture = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
