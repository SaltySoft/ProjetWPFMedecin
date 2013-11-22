﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using Wpf_Medical.ServiceUser;

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
        private ObservableCollection<ServiceUser.User> _userList;

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

        /// <summary>
        /// recupere la liste des utilisateurs
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ServiceUser.User> GetUsers()
        {
            if (_userList == null)
            {
                _userList = new ObservableCollection<ServiceUser.User>();
                _userList = LoadUsers();
            }
            return _userList;
        }

        /// <summary>
        /// get a user from his username
        /// </summary>
        /// <param name="username">the username to get</param>
        /// <returns></returns>
        public ServiceUser.User GetUser(String username)
        {
            var client = new ServiceUserClient();
            ServiceUser.User user = null;
            try
            {
                user = client.GetUser(username);
            }
            catch (CommunicationException)
            {
                MessageBox.Show("Erreur d'acces aux web-services");
            }
            return user;
        }

        /// <summary>
        /// Rafrachie la liste des utilisateurs
        /// </summary>
        public void RefreshUsers()
        {
            _userList = LoadUsers();
        }

        /// <summary>
        /// ajoute un nouvel utilisateur
        /// </summary>
        /// <param name="user">Login de l' user</param>
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
        /// <summary>
        /// verifie l' existence de l'user
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deconnecte l'user
        /// </summary>
        /// <param name="login">Login de l'user</param>
        /// <returns></returns>
        public Task DisconnectUser(String login)
        {
            var client = new ServiceUserClient();
            return client.DisconnectAsync(login);
        }

        /// <summary>
        /// supprime l'user
        /// </summary>
        /// <param name="login">Login de l'user</param>
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