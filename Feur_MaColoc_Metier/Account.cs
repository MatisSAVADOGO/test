using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Metier
{
    /// <summary>
    /// Classe représentant un compte
    /// </summary>
    public class Account
    {
        #region Attributs

        private int id;
        private string username;
        private string login;
        private string password;
        private string mail;
        private bool isAdmin = false;

        private Chambre chambre;

        #endregion

        #region Propriétés

        /// <summary>
        /// ID du compte
        /// </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// pseudo du compte
        /// </summary>
        public string Username { get => username; set => username = value; }
        
        /// <summary>
        /// nom de compte, du compte
        /// </summary>
        public string Login { get => login; set => login = value; }
        
        /// <summary>
        /// mot de passe du compte
        /// </summary>
        public string Password { get => password; set => password = value; }
        /// <summary>
        /// mail du compte
        /// </summary>
        public string Mail { get => mail; set => mail = value; }
        /// <summary>
        /// vérification du droit d'admin au compte
        /// </summary>
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }

        /// <summary>
        /// Propriété pour récupérer ou modifier la chambre où habite le locataire
        /// </summary>
        public Chambre Chambre { get => chambre; set => chambre = value; }

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur de la classe Account représentant un compte dans l'application MaColoc
        /// </summary>
        /// <param name="username"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="mail"></param>
        /// <param name="isAdmin"></param>
        public Account(string username, string login, string password, string mail, bool isAdmin)
        {
            this.login = login;
            this.password = password;
            this.isAdmin = isAdmin;
            this.mail = mail;
            this.username = username;
        }

        /// <summary>
        /// Constructeur de la classe Account représentant un compte dans l'application MaColoc servant à la récupération de données de la BDD
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="mail"></param>
        /// <param name="isAdmin"></param>
        public Account(int id, string username, string login, string password, string mail, bool isAdmin)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.isAdmin = isAdmin;
            this.mail = mail;
            this.username = username;
        }

        /// <summary>
        /// Constructeur de la classe Account représentant un compte dans l'application MaColoc servant à la récupération de données de la BDD avec chambre
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="mail"></param>
        /// <param name="isAdmin"></param>
        public Account(int id, string username, string login, string password, string mail, bool isAdmin, Chambre chambre)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.isAdmin = isAdmin;
            this.mail = mail;
            this.username = username;
            this.chambre = chambre;
        }

        #endregion

        /// <summary>
        /// méthode equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Account other = (Account)obj;
            return Id == other.Id &&
                   Username == other.Username &&
                   Login == other.Login &&
                   Password == other.Password &&
                   Mail == other.Mail &&
                   IsAdmin == other.IsAdmin &&
                   Equals(Chambre, other.Chambre);  
        }
        /// <summary>
        /// méthode gethashcode
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Username, Login, Password, Mail, IsAdmin, Chambre);
        }
    }
}
