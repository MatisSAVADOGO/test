using Feur_MaColoc_Metier;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Feur_MaColoc_Data
{
    /// <summary>
    /// Classe responsable de la gestion des opérations liées aux bâtiments dans la base de données.
    /// </summary>
    public class BatimentDAO : DAO<Batiment>
    {
        private AccountDAO accountDAO;

        /// <summary>
        /// Constructeur par défaut qui initialise une nouvelle instance de la classe <see cref="BatimentDAO"/>.
        /// </summary>
        public BatimentDAO()
        {

        }

        /// <summary>
        /// Récupère tous les bâtiments de la base de données et les retourne sous forme de DataTable.
        /// </summary>
        /// <returns>DataTable contenant les bâtiments.</returns>
        public async Task<DataTable> RecupAllBatimentBDD()
        {
            try
            {
                // Ouvre la connexion à la base de données si elle est fermée
                if (database.Conn.State == ConnectionState.Closed)
                {
                    database.ConnexionBDD();
                }

                // Exécute une requête SELECT pour récupérer tous les bâtiments
                return await database.Select("SELECT * FROM batiment");
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

        #region Create Batiment

        /// <summary>
        /// Crée un nouveau bâtiment dans la base de données.
        /// </summary>
        /// <param name="batiment">Objet Batiment représentant le nouveau bâtiment.</param>
        /// <returns>True si la création a réussi, sinon False.</returns>
        public async Task<bool> CreateBatiment(Batiment batiment)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête d'insertion
                MySqlParameter[] mySqlParameter = new MySqlParameter[5];
                mySqlParameter[0] = new MySqlParameter("@Name", MySqlDbType.VarChar);
                mySqlParameter[1] = new MySqlParameter("@Adress", MySqlDbType.VarChar);
                mySqlParameter[2] = new MySqlParameter("@Code_Postal", MySqlDbType.Int32);
                mySqlParameter[3] = new MySqlParameter("@Ville", MySqlDbType.VarChar);
                mySqlParameter[4] = new MySqlParameter("@Surface", MySqlDbType.Int32);


                // Attribution des valeurs du bâtiment aux paramètres
                mySqlParameter[0].Value = batiment.Name;
                mySqlParameter[1].Value = batiment.Adresse;
                mySqlParameter[2].Value = batiment.CodePostale;
                mySqlParameter[3].Value = batiment.Ville;
                mySqlParameter[4].Value = batiment.Surface;

                // Exécute la requête d'insertion avec les paramètres
                result = await database.InsertWithParameter("INSERT INTO batiment(Name,Adress,Code_Postal,Ville,Surface) VALUES (@Name,@Adress,@Code_Postal,@Ville,@Surface)", mySqlParameter);

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

        #region Delete Batiment

        /// <summary>
        /// Supprime un bâtiment de la base de données.
        /// </summary>
        /// <param name="batiment">Objet Batiment à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public async Task<bool> DeleteBatiment(Batiment batiment)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête de suppression
                MySqlParameter[] mySqlParameter = new MySqlParameter[1];
                mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameter[0].Value = batiment.Id;

                // Exécute la requête de suppression avec les paramètres
                result = await database.OneRow("DELETE FROM batiment WHERE id = @id", mySqlParameter);
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

        #region Find Batiment

        /// <summary>
        /// Recherche un bâtiment par son nom dans la base de données.
        /// </summary>
        /// <param name="name">Nom du bâtiment à rechercher.</param>
        /// <returns>Objet Batiment correspondant au nom spécifié.</returns>
        public async Task<Batiment> FindBatiment(string name)
        {
            Batiment batiment = new Batiment();
            try
            {
                var dataTable = database.Select($"SELECT * FROM batiment WHERE Name = '{name}'").Result;
                var data = ConvertDataTableToList(dataTable);
                batiment = data[0];
                
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
            return batiment;
        }

        /// <summary>
        /// Recherche un bâtiment par son ID dans la base de données.
        /// </summary>
        /// <param name="id">ID du bâtiment à rechercher.</param>
        /// <returns>Objet Batiment correspondant à l'ID spécifié.</returns>
        public async Task<Batiment> FindBatimentWithId(int id)
        {
            Batiment batiment = new Batiment();
            try
            {
                var dataTable = database.Select("SELECT * FROM batiment WHERE id = " + id).Result;
                var data = ConvertDataTableToList(dataTable);
                batiment = data[0];

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
            return batiment;
        }

        #endregion

        #region Edition Bâtiment

        /// <summary>
        /// Modifie les informations d'un bâtiment dans la base de données.
        /// </summary>
        /// <param name="batiment">Objet Batiment contenant les nouvelles informations.</param>
        /// <returns>True si la modification a réussi, sinon False.</returns>
        public async Task<bool> EditBatiment(Batiment batiment)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête de mise à jour
                MySqlParameter[] mySqlParameter = new MySqlParameter[7];
                mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameter[1] = new MySqlParameter("@Name", MySqlDbType.VarChar);
                mySqlParameter[2] = new MySqlParameter("@Adress", MySqlDbType.VarChar);
                mySqlParameter[3] = new MySqlParameter("@Code_Postal", MySqlDbType.Int32);
                mySqlParameter[4] = new MySqlParameter("@Ville", MySqlDbType.VarChar);
                mySqlParameter[5] = new MySqlParameter("@Surface", MySqlDbType.Int32);
                mySqlParameter[6] = new MySqlParameter("@List_Account_User", MySqlDbType.VarChar);

                mySqlParameter[0].Value = batiment.Id;
                mySqlParameter[1].Value = batiment.Name;
                mySqlParameter[2].Value = batiment.Adresse;
                mySqlParameter[3].Value = batiment.CodePostale;
                mySqlParameter[4].Value = batiment.Ville;
                mySqlParameter[5].Value = batiment.Surface;
                mySqlParameter[6].Value = null;

                result = await database.OneRow("UPDATE batiment SET Name = @Name, Adress = @Adress, Code_Postal = @Code_Postal, Ville = @Ville, Surface = @Surface WHERE id = @id", mySqlParameter);
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
        

        #region Convert to List<Batiment>

        /// <summary>
        /// Convertit un DataTable en une liste d'objets Batiment.
        /// </summary>
        /// <param name="dataTable">DataTable contenant les données à convertir.</param>
        /// <returns>Liste d'objets Batiment.</returns>
        public override List<Batiment> ConvertDataTableToList(DataTable dataTable)
        {
            List<Batiment> batimentList = new List<Batiment>();
            foreach (DataRow bat in dataTable.Rows)
            {
                Batiment batiment = new Batiment();
                batiment.Id = Convert.ToInt32(bat["id"]);
                batiment.Name = bat["Name"].ToString();
                batiment.CodePostale = Convert.ToInt32(bat["Code_Postal"]);
                batiment.Adresse = bat["Adress"].ToString();
                batiment.Ville = bat["Ville"].ToString();
                batiment.Surface = Convert.ToInt32(bat["Surface"]);
                //Pas les listes car elles se sont des propriétés calculés
                batimentList.Add(batiment);
            }

            return batimentList;
        }


        #endregion

        #region Parseur String_Account_ToList

        /// <summary>
        /// Parse une chaîne de caractères représentant une liste d'identifiants de compte en une liste d'objets Account.
        /// </summary>
        /// <param name="stringOfAccounts">Chaîne de caractères représentant une liste d'identifiants de compte.</param>
        /// <returns>Liste d'objets Account.</returns>
        private List<Account> Parseur_String_ToList(string stringOfAccounts)
        {
            // Initialise un nouvel objet AccountDAO
            accountDAO = new AccountDAO();
            // Initialise une liste d'objets Account
            List<Account> accounts = new List<Account>();

            // Sépare la chaîne en un tableau d'identifiants de compte
            string[] tab_Account = stringOfAccounts.Split(',');

            // Pour chaque identifiant de compte dans le tableau
            foreach (var account in tab_Account)
            {
                // Récupère l'objet Account correspondant à l'identifiant et l'ajoute à la liste
                Account account1 = accountDAO.GetAccountById(Convert.ToInt32(account)).Result;
                //account1.Chambre faire l'affectation chambre 
                accounts.Add(account1);

            }

            // Retourne la liste d'objets Account
            return accounts;
        }

        #endregion


        #region Parseur toList et toString

        /// <summary>
        /// Parse une chaîne de caractères représentant une liste d'identifiants de compte en une liste d'objets Account.
        /// </summary>
        /// <param name="stringOfAccounts">Chaîne de caractères représentant une liste d'identifiants de compte.</param>
        /// <returns>Liste d'objets Account.</returns>
        private List<Chambre> Parseur_String_ToListOfChambre(string stringOfChambre)
        {
            ChambreDAO chambreDAO = new ChambreDAO();

            List<Chambre> chambreList = new List<Chambre>();

            string[] tab_Chambre = stringOfChambre.Split(',');
            foreach (var chambre in tab_Chambre)
            {
                chambreList.Add(chambreDAO.FindChambreWithId(Convert.ToInt32(chambre)).Result);
            }

            return chambreList;
        }

        private string Parseur_List_toStringOfChambre(List<Chambre> chambres)
        {
            if (chambres == null || chambres.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder chambreList = new StringBuilder();

            foreach (var chambre in chambres)
            {
                chambreList.Append(chambre.Id).Append(",");
            }

            if (chambreList.Length > 0)
            {
                chambreList.Length--;
            }

            return chambreList.ToString();
        }

        #endregion



    }
}
