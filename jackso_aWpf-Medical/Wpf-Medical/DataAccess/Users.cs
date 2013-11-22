using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf_Medical.ServiceUser;
using User = Dbo.User;

namespace Wpf_Medical.DataAccess
{
    public class Users
    {
        /// <summary>
        ///  liste des utilisateurs
        /// </summary>
        private List<User> _userList = null;

        /// <summary>
        /// Constructeur
        /// </summary>
        public Users()
        {
            _userList = new List<User>();
            this.LoadUsers();
        }


        /// <summary>
        /// Charge les users depuis le web-service.
        /// </summary>
        public void LoadUsers()
        {
            ServiceUser.ServiceUserClient client = new ServiceUserClient();
            /*
            client.AddUser(new ServiceUser.User()
            {
                Login = "root",
                Pwd = "root"
            }); */

            try
            {
                ServiceUser.User[] users = client.GetListUser();
                for (int i = 0; i < users.Length; i++)
                {
                    _userList.Add(new Dbo.User()
                    {
                        Login = users[i].Login,
                        Pwd = users[i].Pwd
                    });
                }

            }
            catch (CommunicationException)
            {
                MessageBox.Show(
                    "Une erreur s' est produite lors de la connexion au Web-Service, merci de reessayer plus tard");
            }
        }
       
        /// <summary>
        /// verifie que l'user est valide
        /// </summary>
        /// <param name="login">Login de l'user</param>
        /// <param name="password">Password de l' user</param>
        /// <returns></returns>
        public bool TestUser(String login, String password)
        {
            return _userList.Where(x => x.Login == login && x.Pwd == password).Any();
        }

    }
}
