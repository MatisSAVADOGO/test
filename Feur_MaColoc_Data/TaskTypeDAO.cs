using Feur_MaColoc_Metier;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Data
{
    public class TaskTypeDAO : DAO<TaskType>
    {
        public TaskTypeDAO()
        {

        }

        /// <summary>
        /// Récupère toutes les informations des types de tâches depuis la base de données.
        /// </summary>
        /// <returns>Un objet DataTable contenant les informations des types de tâches.</returns>
        public async Task<DataTable> getAllTaskType()
        {
            try
            {
                // Vérifie si la connexion à la base de données est fermée, puis la connecte si nécessaire
                if (database.Conn.State == System.Data.ConnectionState.Closed)
                {
                    database.ConnexionBDD();
                }
                // Exécute la requête de sélection de tous les types de tâches
                return await database.Select("SELECT * FROM tasktype");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, lance une exception avec le message d'erreur
                throw new Exception(ex.Message);
            }
            finally
            {
                // Vérifie si la connexion à la base de données est ouverte, puis la ferme si nécessaire
                if (database.Conn.State == System.Data.ConnectionState.Open)
                {
                    database.CloseConnection();
                }
            }
        }
        #region Création Tâches
        /// <summary>
        /// Crée un nouveau type de tâche dans la base de données.
        /// </summary>
        /// <param name="tasktype">Objet TaskType contenant les informations du type de tâche.</param>
        /// <returns>True si la création a réussi, sinon False.</returns>
        public async Task<bool> CreateTaskType(TaskType tasktype)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête d'insertion
                MySqlParameter[] mySqlParameter = new MySqlParameter[4];
                mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameter[1] = new MySqlParameter("@Name", MySqlDbType.VarChar);
                mySqlParameter[2] = new MySqlParameter("@duration", MySqlDbType.Int32);
                mySqlParameter[3] = new MySqlParameter("@repetition", MySqlDbType.VarChar);

                // Attribution des valeurs du type de tâche aux paramètres
                mySqlParameter[0].Value = tasktype.Id;
                mySqlParameter[1].Value = tasktype.Name;
                mySqlParameter[2].Value = tasktype.Duration;
                mySqlParameter[3].Value = tasktype.Repetition;

                // Exécute la requête d'insertion avec les paramètres
                result = await database.InsertWithParameter("INSERT INTO tasktype (id,Name,duration,repetition) VALUES (@id,@Name,@duration,@repetition)", mySqlParameter);

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

        #region Suppression Tâches
        /// <summary>
        /// Supprime un type de tâche de la base de données.
        /// </summary>
        /// <param name="taskType">Objet TaskType à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public async Task<bool> DeleteTaskType(TaskType taskType)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête de suppression
                MySqlParameter[] mySqlParameter = new MySqlParameter[1];
                mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameter[0].Value = taskType.Id;

                // Exécute la requête de suppression avec les paramètres
                result = await database.OneRow("DELETE FROM tasktype WHERE id = @id", mySqlParameter);
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

        #region Find Tâche
        /// <summary>
        /// Recherche un type de tâche dans la base de données par son nom.
        /// </summary>
        /// <param name="name">Nom du type de tâche à rechercher.</param>
        /// <returns>Objet TaskType correspondant au nom spécifié.</returns>
        public async Task<TaskType> FindTaskType(string name)
        {
            TaskType taskType = new TaskType();
            try
            {
                // Ajoute le paramètre du nom à la commande SQL
                this.database.Cmd.Parameters.AddWithValue("@name", name);

                // Exécute la requête de sélection du type de tâche par son nom
                DataTable dataTable = await database.Select("SELECT * FROM tasktype WHERE Name = @Name");

                // Vérifie s'il y a des lignes dans le résultat
                if (dataTable.Rows.Count > 0)
                {
                    // Met à jour l'instance TaskType avec les données de la première ligne du résultat
                    DataRow row = dataTable.Rows[0];
                    taskType.Id = Convert.ToInt32(row["id"]);
                    taskType.Name = row["Name"].ToString();
                    taskType.Duration = Convert.ToInt32(row["duration"]);

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
            return taskType;
        }
        #endregion

        #region Convert to List<TaskType>
        /// <summary>
        /// Convertit un objet DataTable en une liste d'objets TaskType.
        /// </summary>
        /// <param name="dataTable">Objet DataTable contenant les informations des types de tâches.</param>
        /// <returns>Liste d'objets TaskType.</returns>
        public override List<TaskType> ConvertDataTableToList(DataTable dataTable)
        {
            List<TaskType> taskTypes = new List<TaskType>();
            foreach (DataRow row in dataTable.Rows)
            {
                TaskType taskType = new TaskType();
                taskType.Id = (int)row["id"];
                taskType.Name = (string)row["name"];
                taskType.Duration = (int)row["duration"];
                taskType.Repetition = (string)row["repetition"];
                taskTypes.Add(taskType);
            }
            return taskTypes;
        }
        #endregion

        #region Edition Tâche 
        public async Task<bool> EditTaskType(TaskType tasktype)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête de mise à jour
                MySqlParameter[] mySqlParameters = new MySqlParameter[3];
                mySqlParameters[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameters[1] = new MySqlParameter("@Name", MySqlDbType.VarChar);
                mySqlParameters[2] = new MySqlParameter("@duration", MySqlDbType.Int32);
                mySqlParameters[3] = new MySqlParameter("@repetition", MySqlDbType.VarChar);

                // Attribution des valeurs du type de tâche aux paramètres
                mySqlParameters[0].Value = tasktype.Id;
                mySqlParameters[1].Value = tasktype.Name;
                mySqlParameters[2].Value = tasktype.Duration;
                mySqlParameters[3].Value = tasktype.Repetition;

                // Exécute la requête de mise à jour avec les paramètres
                result = await database.OneRow("UPDATE tasktype SET Name = @Name, duration = @duration WHERE id = @id", mySqlParameters);
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
    }
}
