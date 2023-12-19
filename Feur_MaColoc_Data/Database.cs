using Feur_MaColoc_Metier;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Feur_MaColoc_Data
{
    /// <summary>
    /// Classe gérant la connexion à une base de données MySQL.
    /// </summary>
    public class Database
    {
        #region MySQL

        //string représentant le chemin vers base de donnée
        private readonly static string strConnexion = String.Format("server=163.5.143.66;uid=SAE;pwd=SAE2023;database=SAE");

        // Connexion MySQL
        private MySqlConnection conn; 
        /// <summary>
        /// Objet de Connexion MySQL
        /// </summary>
        public MySqlConnection Conn
        {
            get { return conn; }
            set { conn = value; }
        }
        // Commande MySQL
        private  MySqlCommand cmd;

        /// <summary>
        /// Objet de Commande MySQL
        /// </summary>
        public MySqlCommand Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
        // Lecteur de données MySQL
        private MySqlDataReader reader;

        /// <summary>
        /// Objet de Lectures de données MySQL
        /// </summary>
        public MySqlDataReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur de la classe Database
        /// </summary>
        public Database()
        {
            conn = new MySqlConnection();
            cmd = conn.CreateCommand();
        }
        #endregion

        #region Connexion BDD
        /// <summary>
        /// Établit une connexion à la base de données MySQL en utilisant la chaîne de connexion spécifiée.
        /// </summary>
        public void ConnexionBDD()
        {
            Conn.ConnectionString = Database.strConnexion;
            //Ouverture de la connexion en BDD
            conn.Open();
        }
        /// <summary>
        /// Ferme la connexion à la base de données MySQL.
        /// </summary>
        public void CloseConnection()
        {
                //Fermeture de la connexion en BDD
                conn.Close();
        }

        #endregion

        #region TableGestion

        /// <summary>
        /// Méthode Générale pour récupérer une table de la base de données contenant des données par rapport à la requête SQL donnée en paramètre
        /// </summary>
        /// <param name="select">Représente la requête SQL voulu</param>
        /// <returns>Retourne une DataTable qui représente une Table en SQL</returns>
        public async Task<DataTable> Select(string select)
        {
            //Création d'une DataTable qui sera le résultat final
            DataTable resultTable = new DataTable();

            try
            {
                if(this.Conn.State == ConnectionState.Closed)
                {
                    //On se connecte à la base de donnée si la connexion n'est pas déjà ouverte
                    this.ConnexionBDD();
                }
                //On attribue la requête SQL au cmd.commandtext soit la commande SQL qui sera executer
                cmd.CommandText = select;
                //On récupère les donnée en remplissant la DataTable avec les données de la requête SQL
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(resultTable);
            }
            catch (MySqlException ex)
            {
               throw ex;
            }
            finally
            {
                if(this.Conn.State == ConnectionState.Open)
                {
                    //On ferme la connexion à la base de donnée si la connexion est ouverte
                    await this.Conn.CloseAsync();
                    cmd.Parameters.Clear();
                }
            }
            //On retourne la DataTable remplie des données demandé
            return resultTable;
        }
        
        /// <summary>
        /// Méthode Générale pour insérer une ligne dans une table de la base de données en prenant en compte des paramètres.
        /// </summary>
        /// <param name="insert">Représente la requête SQL voulu</param>
        /// <param name="mySqlParameterCollection">Représente les différents paramètres demandé lors de la requêtes SQL</param>
        /// <returns>Retourne un booléen pour savoir si la méthode a fonctionné ou non</returns>
        public async Task<bool> InsertWithParameter(string insert, MySqlParameter[] mySqlParameterCollection)
        {
            //Création du booléen qui sera retourné pour savoir si la méthode a réussi ou non
            bool res = false;

            try
            {
                if (this.Conn.State == ConnectionState.Closed)
                {
                    //On se connecte à la base de donnée si la connexion n'est pas déjà ouverte
                    this.ConnexionBDD();
                }
                //On attribue la requête SQL au cmd.commandtext soit la commande SQL qui sera executer et on enlève les paramètres existant dans le cmd s'il y en a
                cmd.CommandText = insert;
                cmd.Parameters.Clear();

                foreach (MySqlParameter param in mySqlParameterCollection)
                {
                    //Pour chaque paramètre donnée en paramètre on l'ajoute au cmd
                    cmd.Parameters.AddWithValue(param.ParameterName, param.Value);
                }
                //on exécute la commande
                cmd.ExecuteNonQuery();
                //On met le booléen à true pour dire que la méthode a fonctionné
                res = true;
            }
            catch(MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (this.Conn.State == ConnectionState.Open)
                {
                    //On ferme la connexion à la base de donnée si la connexion est ouverte
                    await this.Conn.CloseAsync();
                }
            }

            return res;
        }

        /// <summary>
        /// Méthode Générale pour insérer une ligne dans une table de la base de données en prenant en compte des paramètres.
        /// </summary>
        /// <param name="Query">Représente la requête SQL voulu</param>
        /// <param name="mySqlParameters">Représente les différents paramètres de la requête</param>
        /// <returns>Retourne un booléen pour savoir si la méthode a fonctionné ou non</returns>
        public async Task<bool> OneRow(string Query, MySqlParameter[] mySqlParameters)
        {
            // Création du booléen qui sera retourné pour savoir si la méthode a réussi ou non
            bool res = false;
            try
            {
                if (this.Conn.State == ConnectionState.Closed)
                {
                    // On se connecte à la base de donnée si la connexion n'est pas déjà ouverte
                    this.ConnexionBDD();
                }
                // On attribue la requête SQL au cmd.commandtext soit la commande SQL qui sera executer et on enlève les paramètres existant dans le cmd s'il y en a
                cmd.CommandText = Query;
                cmd.Parameters.Clear();

                foreach (MySqlParameter param in mySqlParameters)
                {
                    // Pour chaque paramètre donnée en paramètre on l'ajoute au cmd
                    cmd.Parameters.AddWithValue(param.ParameterName, param.Value);
                }
                // on exécute la commande
                await cmd.ExecuteNonQueryAsync();
                // On met le booléen à true pour dire que la méthode a fonctionné
                res = true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (this.Conn.State == ConnectionState.Open)
                {
                    // On ferme la connexion à la base de donnée si la connexion est ouverte
                    this.Conn.Close();
                }
            }
            return res;
        }

        #endregion

        #region Méthode Général

        /// <summary>
        /// Méthode permettant d'obtenir le nombre de ligne dans une table.
        /// </summary>
        /// <param name="table">Représente le nom de la table voulu</param>
        /// <returns>Retourne un nombre integer représentant le nombre de ligne de la table SQL voulu</returns>
        private int GetNbrLigneInTable(string table)
        {
            // Requete pour compter les lignes
            string requeteSql = "SELECT COUNT(*) FROM " + table;

            //Attribution de la requete au cmd.commandtext soit la commande SQL qui sera executer
            cmd.CommandText = requeteSql;

            //On retourne une valeur integer de l'execution de la commande 
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        /// <summary>
        /// Méthode permettant d'obtenir le nombre le plus grand dans une colonne d'une table.
        /// </summary>
        /// <param name="table">Représente le nom de la table voulu</param>
        /// <param name="colonne">Représente la colonne dans la table voulu</param>
        /// <returns>Retourne la valeur maximum dans la colonne de la table voulu</returns>
        public int getHigherValueInColumn(string table, string colonne)
        {
            // Requete pour obtenir la valeur maximum dans une colonne
            string requeteSql = "SELECT MAX("+ colonne +") FROM " + table;
            //On met dans la commande la requete SQL
            cmd.CommandText = requeteSql;
            //On retourne une valeur integer de l'execution de la commande
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        #endregion
    }
}
