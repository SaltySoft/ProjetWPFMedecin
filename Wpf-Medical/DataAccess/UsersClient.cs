using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using Wpf_Medical.ServiceUser;
using User = Dbo.User;

namespace Wpf_Medical.DataAccess
{
    /// <summary>
    /// Pattern Singleton
    /// </summary>
    public class UsersClient
    {
        /// <summary>
        ///  liste des utilisateurs
        /// </summary>
        private ObservableCollection<User> _userList;

        private static UsersClient _instance;

        /// <summary>
        /// Thread-safe
        /// </summary>
        private static readonly object Padlock = new object();


        public static UsersClient Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new UsersClient());
                }
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        private UsersClient()
        {
        }


        /// <summary>
        /// Charge les users depuis le web-service.
        /// </summary>
        public ObservableCollection<User> LoadUsers()
        {
            var client = new ServiceUserClient();
            var userList = new ObservableCollection<User>();

            try
            {
                ServiceUser.User[] users = client.GetListUser();
                foreach (ServiceUser.User t in users)
                {
                    userList.Add(new User
                    {
                        Login = t.Login,
                        Pwd = t.Pwd,
                        Name = t.Name,
                        Firstname = t.Firstname,
                        Role = t.Role,
                        Connected = t.Connected,
                        Picture = t.Picture
                    });
                }
            }
            catch (CommunicationException)
            {
                MessageBox.Show("Erreur d'acces aux web-services");
            }
            return userList;
        }

        public ObservableCollection<User> GetUsers()
        {
            if (_userList == null)
            {
                _userList = new ObservableCollection<User>();
                _userList = LoadUsers();
            }
            return _userList;
        }

        public void SetUsers(ObservableCollection<User> users)
        {
            _userList = users;
        }

        public async void AddUser(User user)
        {
            var client = new ServiceUserClient();
            try
            {
                if (UserExists(user.Login))
                {
                    MessageBox.Show("Cet utilisateur existe deja !");
                    return;
                }
                var userToAdd = new ServiceUser.User
                {
                    Login = user.Login,
                    Pwd = user.Pwd,
                    Connected = false,
                    Firstname = user.Firstname,
                    Name = user.Name,
                    Role = user.Role,
                    Picture = user.Picture
                };
                Task<bool> didAddUser = client.AddUserAsync(userToAdd);
                if (await didAddUser)
                {   
                    _userList.Add(user);
                }

            }
             catch (CommunicationException c)
             {
                 Console.WriteLine(c.StackTrace);
                 MessageBox.Show("Erreur d'acces aux web-services");
             }
        }

        public bool UserExists(string login)
        {
            return _userList.Any(u => u.Login == login);
        }

        /// <summary>
        /// verifie que l'user est valide
        /// </summary>
        /// <param name="login">Login de l'user</param>
        /// <param name="password">Password de l' user</param>
        /// <returns></returns>
        public Task<bool> TestUser(String login, String password)
        {
            var client = new ServiceUserClient();
            return client.ConnectAsync(login, password);
        }

        public Task DisconnectUser(String login)
        {
            var client = new ServiceUserClient();
            return client.DisconnectAsync(login);
        }

        public async void RemoveUser(String login)
        {
            var client = new ServiceUserClient();
            Task<bool> res = client.DeleteUserAsync(login);
            if (await res)
            {
                _userList.Remove(_userList.First(x => x.Login == login));   
            }
        }
    }
}
