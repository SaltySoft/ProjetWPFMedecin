

using System;
using Dbo;

namespace Wpf_Medical.ViewModels
{
    class DetailUserViewModel : BaseViewModel
    {
        #region variables
        
        private readonly User _selectedUser;
        private byte[] _connectedPicture;

        #endregion

        #region constructeur
        public DetailUserViewModel(User u)
        {
            _selectedUser = u;
            if (_selectedUser.Connected)
            {
                // fixme ajouter l'image appriorie en fonction de la connection de l'utilisateur ou non
            }
            else
            {
                
            }
        }
        #endregion

        #region getters / setters
        /// <summary>
        /// Utilisateur selectionne
        /// </summary>
        public User SelectedUser
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
        #endregion
    }
}
