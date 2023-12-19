using Feur_MaColoc_Metier;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Data
{
    /// <summary>
    /// Classe responsable de la gestion des opérations liées aux comptes dans la base de données.
    /// </summary>
    public class AccountDAO : DAO<Account>
    {
        private BatimentDAO batimentDAO;

        /// <summary>
        /// Constructeur par défaut qui initialise une nouvelle instance de la classe <see cref="AccountDAO"/>.
        /// </summary>
        public AccountDAO()
        {
            batimentDAO = new BatimentDAO();
        }

        #region Gestion Compte

        /// <summary>
        /// Récupère tous les comptes de la base de données et les retourne sous forme de DataTable.
        /// </summary>
        /// <returns>DataTable contenant les comptes.</returns>
        public async Task<DataTable> RecupAllCompteBDD()
        {
            try
            {
                // Ouvre la connexion à la base de données si elle est fermée
                if (database.Conn.State == ConnectionState.Closed)
                {
                    database.ConnexionBDD();
                }

                // Exécute une requête SELECT pour récupérer tous les comptes
                return await database.Select("SELECT * FROM account");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, lance une exception avec le message d'erreur
                throw new Exception(ex.Message);
            }
            finally
            {
                // Ferme la connexion à la base de données si elle est ouverte
                if (database.Conn.State == ConnectionState.Open)
                {
                    database.CloseConnection();
                }
            }
        }

        /// <summary>
        /// Crée un nouveau compte dans la base de données.
        /// </summary>
        /// <param name="account">Objet Account représentant le nouveau compte.</param>
        /// <returns>True si la création a réussi, sinon False.</returns>
        public async Task<bool> CreateAccount(Account account)
        {
            bool result = false;
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    // Convertit le mot de passe en tableau de bytes
                    byte[] hashedPasswordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(account.Password));

                    // Convertit les bytes hachés en une chaîne hexadécimale
                    string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();

                    // Crée un tableau de paramètres pour la requête d'insertion
                    MySqlParameter[] mySqlParameter = new MySqlParameter[4];
                    mySqlParameter[0] = new MySqlParameter("@username", MySqlDbType.VarChar);
                    mySqlParameter[1] = new MySqlParameter("@login", MySqlDbType.VarChar);
                    mySqlParameter[2] = new MySqlParameter("@password", MySqlDbType.VarChar);
                    mySqlParameter[3] = new MySqlParameter("@mail", MySqlDbType.VarChar);

                    // Attribution des valeurs du compte aux paramètres
                    mySqlParameter[0].Value = account.Username;
                    mySqlParameter[1].Value = account.Login;
                    mySqlParameter[2].Value = hashedPassword;

                    // Spécification du type et de la taille pour le paramètre du courriel
                    mySqlParameter[3].DbType = DbType.String;
                    mySqlParameter[3].Size = 255;
                    mySqlParameter[3].Value = account.Mail;

                    // Exécute la requête d'insertion avec les paramètres
                    result = await database.InsertWithParameter("INSERT INTO account (username,login,password,mail) VALUES (@username,@login,@password,@mail)", mySqlParameter);
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, lance une exception avec le message d'erreur interne
                throw ex.InnerException;
            }
            finally
            {
                // Nettoie les paramètres de la commande après l'exécution
                database.Cmd.Parameters.Clear();
            }

            return result;
        }

        #endregion

        #region Conversion DataTable to List

        /// <summary>
        /// Convertit un DataTable en une liste d'objets Account.
        /// </summary>
        /// <param name="dataTable">DataTable à convertir.</param>
        /// <returns>Liste d'objets Account.</returns>
        public override List<Account> ConvertDataTableToList(DataTable dataTable)
        {
            ChambreDAO chambreDAO = new ChambreDAO();
            Account account;
            List<Account> accountList = new List<Account>();

            foreach (DataRow row in dataTable.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string username = row["username"].ToString();
                string login = row["login"].ToString();
                string password = row["password"].ToString();
                string mail = row["mail"].ToString();
                bool isAdmin = Convert.ToBoolean(row["Admin"]);
                account = new Account(id, username, login, password, mail, isAdmin);
                //Pas de chambre car propriété calculé
                if (row["idChambre"] != DBNull.Value && !string.IsNullOrEmpty(row["idChambre"].ToString()))
                {
                    account.Chambre = chambreDAO.FindChambreWithId(Convert.ToInt32(row["idChambre"])).Result;
                }

                accountList.Add(account);
            }

            return accountList;
        }

        /// <summary>
        /// Récupère un compte par son ID dans la base de données.
        /// </summary>
        /// <param name="id">ID du compte à récupérer.</param>
        /// <returns>Objet Account correspondant à l'ID spécifié.</returns>
        public async Task<Account> GetAccountById(int id)
        {
            ChambreDAO chambreDAO = new ChambreDAO();   
            Account account = null;
            try
            {
                // Ajoute un paramètre à la commande pour l'ID du compte
                this.database.Cmd.Parameters.AddWithValue("@id", id);

                // Exécute une requête SELECT pour récupérer le compte avec l'ID spécifié
                DataTable dataTable = await database.Select("SELECT * FROM account WHERE id = @id");
                
                // Vérifie s'il y a des lignes dans le résultat
                if (dataTable.Rows.Count > 0)
                {
                    // Met à jour l'instance Account avec les données de la première ligne du résultat
                    DataRow row = dataTable.Rows[0];
                    account = new Account(Convert.ToInt32(row["id"]), row["username"].ToString(), row["login"].ToString(), row["password"].ToString(), row["mail"].ToString(), Convert.ToBoolean(row["Admin"]), chambreDAO.FindChambreWithId(Convert.ToInt32(row["idChambre"])).Result);
                    //On affecte une chambre autre part                     
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, lance une exception avec le message d'erreur interne
                throw ex.InnerException;
            }
            finally
            {
                // Nettoie les paramètres de la commande après l'exécution
                database.Cmd.Parameters.Clear();
            }
            return account;
        }

        /// <summary>
        /// Récupère un compte par son nom d'utilisateur dans la base de données.
        /// </summary>
        /// <param name="name">Nom d'utilisateur du compte à récupérer.</param>
        /// <returns>Objet Account correspondant au nom d'utilisateur spécifié.</returns>
        public async Task<Account> GetAccountByName(string name)
        {
            Account account = null;
            try
            {
                // Ajoute un paramètre à la commande pour le nom d'utilisateur
                this.database.Cmd.Parameters.AddWithValue("@name", name);

                // Exécute une requête SELECT pour récupérer le compte avec le nom d'utilisateur spécifié
                DataTable dataTable = await database.Select("SELECT * FROM account WHERE username = @name");

                // Vérifie s'il y a des lignes dans le résultat
                if (dataTable.Rows.Count > 0)
                {
                    // Met à jour l'instance Account avec les données de la première ligne du résultat
                    DataRow row = dataTable.Rows[0];
                    account = new Account(Convert.ToInt32(row["id"]), row["username"].ToString(), row["login"].ToString(), row["password"].ToString(), row["mail"].ToString(), Convert.ToBoolean(row["Admin"]));
                    //On affecte une chambre autre part                     
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, lance une exception avec le message d'erreur interne
                throw ex.InnerException;
            }
            finally
            {
                // Nettoie les paramètres de la commande après l'exécution
                database.Cmd.Parameters.Clear();
            }
            return account;
        }

        #endregion

        /// <summary>
        /// Modifie les informations d'un compte dans la base de données.
        /// </summary>
        /// <param name="account">Objet Account contenant les nouvelles informations.</param>
        /// <returns>True si la modification a réussi, sinon False.</returns>
        public async Task<bool> EditAccount(Account account)
        {
            bool result = false;
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {

                    // Convertit le mot de passe en tableau de bytes
                    byte[] hashedPasswordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(account.Password));

                    // Convertit les bytes hachés en une chaîne hexadécimale
                    string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();

                    // Crée un tableau de paramètres pour la requête de mise à jour
                    MySqlParameter[] mySqlParameter = new MySqlParameter[7];
                    mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                    mySqlParameter[1] = new MySqlParameter("@username", MySqlDbType.VarChar);
                    mySqlParameter[2] = new MySqlParameter("@login", MySqlDbType.VarChar);
                    mySqlParameter[3] = new MySqlParameter("@password", MySqlDbType.VarChar);
                    mySqlParameter[4] = new MySqlParameter("@mail", MySqlDbType.VarChar);
                    mySqlParameter[5] = new MySqlParameter("@isAdmin", MySqlDbType.Bit);

                    // Attribution des valeurs du compte aux paramètres
                    mySqlParameter[0].Value = account.Id;
                    mySqlParameter[1].Value = account.Username;
                    mySqlParameter[2].Value = account.Login;
                    if(account.Password.Length != 64) // 64 correspond au nombre de caractère que le SHA256 génère, on vérifie donc si le password est déjà hasher ou non
                    {
                        mySqlParameter[3].Value = hashedPassword;
                    }
                    else
                    {
                        mySqlParameter[3].Value = account.Password;
                    }
                    mySqlParameter[4].Value = account.Mail;
                    mySqlParameter[5].Value = account.IsAdmin;

                    // Ajoute un paramètre pour l'ID du bâtiment (si disponible)
                    if (account.Chambre != null)
                    {
                        mySqlParameter[6] = new MySqlParameter("@chambre", MySqlDbType.Int32);
                        mySqlParameter[6].Value = account.Chambre.Id;
                        result = await database.OneRow("UPDATE account SET username = @username, login = @login, password = @password, mail = @mail, Admin = @isAdmin,idChambre = @chambre WHERE id = @id", mySqlParameter);
                    }
                    else
                    {
                        mySqlParameter[6] = new MySqlParameter("@chambre", MySqlDbType.Int32);
                        mySqlParameter[6].Value = "";
                        result = await database.OneRow("UPDATE account SET username = @username, login = @login, password = @password, mail = @mail, Admin = @isAdmin,idChambre = @chambre WHERE id = @id", mySqlParameter);


                    }
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, lance une exception avec le message d'erreur interne
                throw ex.InnerException;
            }
            finally
            {
                // Nettoie les paramètres de la commande après l'exécution
                database.Cmd.Parameters.Clear();
            }
            return result;
        }
    }
}
