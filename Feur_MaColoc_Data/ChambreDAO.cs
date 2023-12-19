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
    public class ChambreDAO : DAO<Chambre>
    {
        /// <summary>
        /// Récupère tous les bâtiments de la base de données et les retourne sous forme de DataTable.
        /// </summary>
        /// <returns>DataTable contenant les bâtiments.</returns>
        public async Task<DataTable> RecupAllChambreBDD()
        {
            try
            {
                // Ouvre la connexion à la base de données si elle est fermée
                if (database.Conn.State == ConnectionState.Closed)
                {
                    database.ConnexionBDD();
                }

                // Exécute une requête SELECT pour récupérer tous les bâtiments
                return await database.Select("SELECT * FROM chambre");
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
        public async Task<bool> CreateChambre(Chambre chambre)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête d'insertion
                MySqlParameter[] mySqlParameter = new MySqlParameter[3];
                mySqlParameter[0] = new MySqlParameter("@Name", MySqlDbType.VarChar);
                mySqlParameter[1] = new MySqlParameter("@Surface", MySqlDbType.VarChar);
                mySqlParameter[2] = new MySqlParameter("@idBatiment", MySqlDbType.Int32);

                // Attribution des valeurs du bâtiment aux paramètres
                mySqlParameter[0].Value = chambre.Name;
                mySqlParameter[1].Value = chambre.Surface;
                mySqlParameter[2].Value = null;
                if(chambre.Batiment != null)
                {
                    mySqlParameter[2].Value = chambre.Batiment.Id;
                }

                // Exécute la requête d'insertion avec les paramètres // Vérifier si envoyer null value en bdd fais crash ou non
                result = await database.InsertWithParameter("INSERT INTO chambre(name,surface,idBatiment) VALUES (@Name,@Surface,@idBatiment)", mySqlParameter);

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

        #region Delete chambre

        /// <summary>
        /// Supprime un bâtiment de la base de données.
        /// </summary>
        /// <param name="chambre">Objet Batiment à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public async Task<bool> DeleteChambre(Chambre chambre)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête de suppression
                MySqlParameter[] mySqlParameter = new MySqlParameter[1];
                mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameter[0].Value = chambre.Id;

                // Exécute la requête de suppression avec les paramètres
                result = await database.OneRow("DELETE FROM chambre WHERE id = @id", mySqlParameter);
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
        public async Task<Chambre> FindChambre(string name)
        {
            AccountDAO accountDAO = new AccountDAO();
            BatimentDAO batimentDAO = new BatimentDAO();
            Chambre chambre= new Chambre();
            try
            {
                // Ajoute un paramètre à la commande pour le nom du bâtiment
                this.database.Cmd.Parameters.AddWithValue("@name", name);

                // Exécute une requête SELECT pour récupérer le bâtiment avec le nom spécifié
                DataTable dataTable = await database.Select("SELECT * FROM chambre WHERE name = @name");

                // Vérifie s'il y a des lignes dans le résultat
                if (dataTable.Rows.Count > 0)
                {
                    // Met à jour l'instance Batiment avec les données de la première ligne du résultat
                    DataRow row = dataTable.Rows[0];
                    chambre.Id = Convert.ToInt32(row["id"]);
                    chambre.Name = row["name"].ToString();
                    chambre.Surface = Convert.ToInt32(row["surface"]);
                    //Pas de locataire ni batiment car propriété calculé
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
            return chambre;
        }

        /// <summary>
        /// Recherche un bâtiment par son ID dans la base de données.
        /// </summary>
        /// <param name="id">ID du bâtiment à rechercher.</param>
        /// <returns>Objet Batiment correspondant à l'ID spécifié.</returns>
        public async Task<Chambre> FindChambreWithId(int id)
        {
            AccountDAO accountDAO = new AccountDAO();
            BatimentDAO batimentDAO = new BatimentDAO();
            Chambre chambre = new Chambre();
            try
            {
                // Ajoute un paramètre à la commande pour l'ID du bâtiment
                this.database.Cmd.Parameters.AddWithValue("@id", id);

                // Exécute une requête SELECT pour récupérer le bâtiment avec l'ID spécifié
                DataTable dataTable = await database.Select("SELECT * FROM chambre WHERE id = @id");

                // Vérifie s'il y a des lignes dans le résultat
                if (dataTable.Rows.Count > 0)
                {
                    // Met à jour l'instance Batiment avec les données de la première ligne du résultat
                    DataRow row = dataTable.Rows[0];
                    chambre.Id = Convert.ToInt32(row["id"]);
                    chambre.Name = row["name"].ToString();
                    chambre.Surface = Convert.ToInt32(row["surface"]);

                    if (!string.IsNullOrEmpty(row["idBatiment"].ToString()))
                    {
                        DataTable data = batimentDAO.RecupAllBatimentBDD().Result;
                        List<Batiment> list = batimentDAO.ConvertDataTableToList(data);
                        list = list.Where(bat => bat.Id == Convert.ToInt32(row["idBatiment"].ToString())).ToList();
                        chambre.Batiment = list[0];
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
            return chambre;
        }

        #endregion

        public List<Chambre> ChambreSelect(string requete)
        {
            return ConvertDataTableToList(database.Select(requete).Result);
        }

        #region Edition Pièces

        /// <summary>
        /// Modifie les informations d'un bâtiment dans la base de données.
        /// </summary>
        /// <param name="batiment">Objet Batiment contenant les nouvelles informations.</param>
        /// <returns>True si la modification a réussi, sinon False.</returns>
        public async Task<bool> EditChambre(Chambre chambre)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête de mise à jour
                MySqlParameter[] mySqlParameter = new MySqlParameter[3];
                mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameter[1] = new MySqlParameter("@name", MySqlDbType.VarChar);
                mySqlParameter[2] = new MySqlParameter("@surface", MySqlDbType.Int32);
                
                mySqlParameter[0].Value = chambre.Id;
                mySqlParameter[1].Value = chambre.Name;
                mySqlParameter[2].Value = chambre.Surface;

                if (chambre.Batiment != null)
                {
                    // Exécute la requête de mise à jour avec les paramètres
                    result = await database.OneRow($"UPDATE chambre SET name = @name, surface = @surface, idBatiment = {chambre.Batiment.Id} WHERE id = @id", mySqlParameter);

                }
                else
                    //result = await database.OneRow("UPDATE chambre SET name = @name, surface = @surface WHERE id = @id", mySqlParameter);
                    result = await database.OneRow($"UPDATE chambre SET name = '{chambre.Name}', surface = {chambre.Surface} WHERE id = {chambre.Id};", mySqlParameter);
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
        public override List<Chambre> ConvertDataTableToList(DataTable dataTable)
        {
            AccountDAO accountDAO = new AccountDAO();
            BatimentDAO batimentDAO = new BatimentDAO();
            List<Chambre> chambreList = new List<Chambre>();
            

            foreach (DataRow row in dataTable.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string name = row["name"].ToString();
                int surface = Convert.ToInt32(row["surface"]);
                Account account = null;
                Batiment batiment = new Batiment();

                DataTable data = database.Select($"SELECT * FROM account WHERE idChambre = {id}").Result;
                if(data.Rows.Count > 0)
                {
                    account = accountDAO.ConvertDataTableToList(data)[0];
                }
                
                
                if (!string.IsNullOrEmpty(row["idBatiment"].ToString()))
                {
                    batiment = batimentDAO.FindBatimentWithId(Convert.ToInt32(row["idBatiment"])).Result;
                }
                // Crée un objet Batiment avec les données de la ligne
                Chambre chambre = new Chambre();
                chambre.Id = id;
                chambre.Name = name;
                chambre.Surface = surface;
                if(account != null)
                {
                    chambre.Locataire = account;
                }
                if(batiment != null)
                {
                    chambre.Batiment = batiment;
                }

                chambreList.Add(chambre);
            }

            return chambreList;
        }

        #endregion
    }
}
