using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;

namespace Wpf_Medical.ViewModels
{
    class AddUserViewModel : BaseViewModel
    {
        #region variables

        private ObservableCollection<string> _roles;
        private String _avatarPath;
        private ICommand _addUserCommand;


        private String _login;
        private String _firstName;
        private String _lastName;
        private String _password;
        private String _roleSelected;
        private byte[] _image;

        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur
        /// </summary>
        public AddUserViewModel()
        {
            LoadDefaultImage();
        }
        #endregion

        #region methodes
        /// <summary>
        /// Charge l'image par defaut
        /// </summary>
        private void LoadDefaultImage()
        {
            _roles = new ObservableCollection<string> {"Infirmière" , "Medecin", "Radiologue", "Chirurgien" };
            Role = _roles.FirstOrDefault();
        }

        private void AddUser()
        {
            var user = new Dbo.User
            {
                Login = _login,
                Pwd = _password,
                Firstname = _firstName,
                Name = _lastName,
                Role = _roleSelected,
                Picture = _image,
                Connected = false
            };
            DataAccess.UsersClient.Instance.AddUser(user);
        }
        #endregion

        #region getters / setters

        #region attribus
        /// <summary>
        /// Le chemin de l'image de l'utilisateur
        /// </summary>
        public String AvatarPath
        {
            get { return _avatarPath; }
            set
            {
                if (_avatarPath != value)
                {
                    _avatarPath = value;
                    OnPropertyChanged();
                }
            }
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

        /// <summary>
        /// le textbox du prenom
        /// </summary>
        public String FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// le textbox du nom
        /// </summary>
        public String LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// la liste des roles
        /// </summary>
        public ObservableCollection<string> Roles
        {
            get { return _roles; }
        }

        /// <summary>
        /// selection l'image qui va bien en cas de changement de role dans le menu deroulant
        /// </summary>
        public String Role
        {
            get { return _roleSelected; }
            set
            {
                if (_roleSelected != value && value != null)
                {
                    _roleSelected = value;
                    var fileImg = "Resources/Images/";
                    if (_roleSelected.Equals("Medecin"))
                    {
                        fileImg += "medecin";
                    }
                    else if (_roleSelected.Equals("Radiologue"))
                    {
                        fileImg += "radiologue";
                    }
                    else if (_roleSelected.Equals("Chirurgien"))
                    {
                        fileImg += "chirurgien";
                    }
                    else if (_roleSelected.Equals("Infirmière"))
                    {
                        fileImg += "infirmiere";
                    }
                    else
                    {
                        fileImg += "no_image";
                    }
                    fileImg += ".jpg";

                    var info = new FileInfo(fileImg);
                    AvatarPath = info.FullName;
                    var sr = new StreamReader(AvatarPath);
                    var read = new BinaryReader(sr.BaseStream);
                    _image = read.ReadBytes((int) sr.BaseStream.Length);
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region commands
        /// <summary>
        /// Bouton d' ajout de l' utilisateur
        /// </summary>
        public ICommand AddUserCommand
        {
            get
            {
                return _addUserCommand ?? (_addUserCommand = new RelayCommand(
                    param => AddUser(),
                    param => true
                    ));
            }
        }
        #endregion

        #endregion
    }
}
