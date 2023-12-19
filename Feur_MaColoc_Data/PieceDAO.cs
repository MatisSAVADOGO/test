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
    public class PieceDAO : DAO<Piece>
    {
        /// <summary>
        /// Récupère toutes les informations des pièces depuis la base de données.
        /// </summary>
        /// <returns>Un objet DataTable contenant les informations des types de tâches.</returns>
        public async Task<DataTable> getAllPiece()
        {
            try
            {
                // Vérifie si la connexion à la base de données est fermée, puis la connecte si nécessaire
                if (database.Conn.State == System.Data.ConnectionState.Closed)
                {
                    database.ConnexionBDD();
                }
                // Exécute la requête de sélection de tous les types de tâches
                return await database.Select("SELECT * FROM piece");
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
        }/// <summary>
         /// Crée un nouveau type de tâche dans la base de données.
         /// </summary>
         /// <param name="tasktype">Objet TaskType contenant les informations du type de tâche.</param>
         /// <returns>True si la création a réussi, sinon False.</returns>
        public async Task<bool> CreatePiece(Piece piece)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête d'insertion
                MySqlParameter[] mySqlParameter = new MySqlParameter[6];
                mySqlParameter[0] = new MySqlParameter("@Name", MySqlDbType.VarChar);
                mySqlParameter[1] = new MySqlParameter("@type", MySqlDbType.VarChar);
                mySqlParameter[2] = new MySqlParameter("@surface", MySqlDbType.Int32);
                mySqlParameter[3] = new MySqlParameter("@isPublic", MySqlDbType.Bool);
                mySqlParameter[4] = new MySqlParameter("@ListIdChambre", MySqlDbType.VarChar);
                mySqlParameter[5] = new MySqlParameter("@idBatiment", MySqlDbType.Int32);

                // Attribution des valeurs du type de tâche aux paramètres
                mySqlParameter[0].Value = piece.Name;
                mySqlParameter[1].Value = piece.Type;
                mySqlParameter[2].Value = piece.Surface;
                mySqlParameter[3].Value = piece.IsPublic;
                mySqlParameter[4].Value = null;
                mySqlParameter[5].Value = null;
                if (piece.Chambres != null)
                {
                    mySqlParameter[4].Value = Parseur_List_toStringOfChambre(piece.Chambres);
                }
                if(piece.Batiment != null)
                {
                    mySqlParameter[5].Value = piece.Batiment.Id;
                }

                // Exécute la requête d'insertion avec les paramètres
                result = await database.InsertWithParameter("INSERT INTO piece (name,surface,type,isPublic,ListIdChambre,idBatiment) VALUES (@Name,@type,@surface,@isPublic,@ListIdChambre,@idBatiment)", mySqlParameter);

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

        /// <summary>
        /// Supprime une pièce de la base de données.
        /// </summary>
        /// <param name="piece">Objet Piece à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public async Task<bool> DeletePiece(Piece piece)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête de suppression
                MySqlParameter[] mySqlParameter = new MySqlParameter[1];
                mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameter[0].Value = piece.Id;

                // Exécute la requête de suppression avec les paramètres
                result = await database.OneRow("DELETE FROM piece WHERE id = @id", mySqlParameter);
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

        public async Task<bool> EditPiece(Piece piece)
        {
            bool result = false;
            try
            {
                // Crée un tableau de paramètres pour la requête de modification
                MySqlParameter[] mySqlParameter = new MySqlParameter[7];
                mySqlParameter[0] = new MySqlParameter("@id", MySqlDbType.Int32);
                mySqlParameter[1] = new MySqlParameter("@Name", MySqlDbType.VarChar);
                mySqlParameter[2] = new MySqlParameter("@type", MySqlDbType.VarChar);
                mySqlParameter[3] = new MySqlParameter("@surface", MySqlDbType.Int32);
                mySqlParameter[4] = new MySqlParameter("@isPublic", MySqlDbType.Bool);
                mySqlParameter[5] = new MySqlParameter("@ListIdChambre", MySqlDbType.VarChar);
                mySqlParameter[6] = new MySqlParameter("@idBatiment", MySqlDbType.Int32);

                // Attribution des valeurs du type de tâche aux paramètres
                mySqlParameter[0].Value = piece.Id;
                mySqlParameter[1].Value = piece.Name;
                mySqlParameter[2].Value = piece.Type;
                mySqlParameter[3].Value = piece.Surface;
                mySqlParameter[4].Value = piece.IsPublic;
                mySqlParameter[5].Value = "";
                mySqlParameter[6].Value = null;
                if (piece.Chambres != null || piece.Chambres.Count > 0)
                {
                    mySqlParameter[5].Value = Parseur_List_toStringOfChambre(piece.Chambres);
                }
                if (piece.Batiment != null)
                {
                    mySqlParameter[6].Value = piece.Batiment.Id;
                }

                // Exécute la requête de modification avec les paramètres
                result = await database.OneRow("UPDATE piece SET name = @Name, type = @type, surface = @surface, isPublic = @isPublic, ListIdChambre = @ListIdChambre, idBatiment = @idBatiment WHERE id = @id", mySqlParameter);

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

        /// <summary>
        /// Recherche une pièce dans la base de données par son nom.
        /// </summary>
        /// <param name="name">Nom de la pièce à rechercher.</param>
        /// <returns>Objet Piece correspondant au nom spécifié.</returns>
        public async Task<Piece> FindPiece(string name)
        {
            BatimentDAO batimentDAO = new BatimentDAO();
            Piece piece = new Piece();
            try
            {
                // Ajoute le paramètre du nom à la commande SQL
                this.database.Cmd.Parameters.AddWithValue("@name", name);

                // Exécute la requête de sélection du type de tâche par son nom
                DataTable dataTable = await database.Select("SELECT * FROM piece WHERE name = @name");

                // Vérifie s'il y a des lignes dans le résultat
                if (dataTable.Rows.Count > 0)
                {
                    // Met à jour l'instance TaskType avec les données de la première ligne du résultat
                    DataRow row = dataTable.Rows[0];
                    piece.Id = Convert.ToInt32(row["id"]);
                    piece.Name = row["name"].ToString();

                    switch (Convert.ToInt32(row["type"].ToString()))
                    {
                        case 0:
                            piece.Type = Enum_Piece.Salon;
                            break;
                        case 1:
                            piece.Type = Enum_Piece.SDB;
                            break;
                        case 2:
                            piece.Type = Enum_Piece.Toilettes;
                            break;
                        case 3:
                            piece.Type = Enum_Piece.Buanderie;
                            break;
                        case 4:
                            piece.Type = Enum_Piece.Cuisine;
                            break;
                    }
                    
                    piece.Surface = Convert.ToInt32(row["surface"]);
                    if (Convert.ToInt32(row["isPublic"]) == 1)
                    {
                        piece.IsPublic = true;
                    }
                    else
                    {
                        piece.IsPublic = false;
                    }
                    //Pas de chambre ni batiment car propriété calculé


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
            return piece;
        }

        /// <summary>
        /// Recherche une pièce par son ID dans la base de données.
        /// </summary>
        /// <param name="id">ID d'une pièce à rechercher.</param>
        /// <returns>Objet Piece correspondant à l'ID spécifié.</returns>
        public async Task<Piece> FindPieceWithId(int id)
        {
            Piece piece = new Piece();
            try
            {
                // Ajoute un paramètre à la commande pour l'ID du bâtiment
                this.database.Cmd.Parameters.AddWithValue("@id", id);

                // Exécute une requête SELECT pour récupérer le bâtiment avec l'ID spécifié
                DataTable dataTable = await database.Select("SELECT * FROM piece WHERE id = @id");

                // Vérifie s'il y a des lignes dans le résultat
                if (dataTable.Rows.Count > 0)
                {
                    // Met à jour l'instance Batiment avec les données de la première ligne du résultat
                    DataRow row = dataTable.Rows[0];
                    piece.Id = Convert.ToInt32(row["id"]);
                    piece.Name = row["Name"].ToString();
                    
                    switch (Convert.ToInt32(row["type"].ToString()))
                    {
                        case 0:
                            piece.Type = Enum_Piece.Salon;
                            break;
                        case 1:
                            piece.Type = Enum_Piece.SDB;
                            break;
                        case 2:
                            piece.Type = Enum_Piece.Toilettes;
                            break;
                        case 3:
                            piece.Type = Enum_Piece.Buanderie;
                            break;
                        case 4:
                            piece.Type = Enum_Piece.Cuisine;
                            break;
                    }
                    piece.Surface = Convert.ToInt32(row["surface"]);
                    if (Convert.ToInt32(row["isPublic"]) == 1)
                    {
                        piece.IsPublic = true;
                    }
                    else
                    {
                        piece.IsPublic = false;
                    }
                    
                    //Pas de chambre ni batiment car propriété calculé

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
            return piece;
        }
        /// <summary>
        /// Convertit un objet DataTable en une liste d'objets TaskType.
        /// </summary>
        /// <param name="dataTable">Objet DataTable contenant les informations des types de tâches.</param>
        /// <returns>Liste d'objets TaskType.</returns>
        public override List<Piece> ConvertDataTableToList(DataTable dataTable)
        {
            BatimentDAO batimentDAO = new BatimentDAO();
            List<Piece> pieceList = new List<Piece>();
            foreach (DataRow row in dataTable.Rows)
            {
                Piece piece = new Piece();
                piece.Id = (int)row["id"];
                piece.Name = (string)row["name"];
                switch (Convert.ToInt32(row["type"].ToString()))
                {
                    case 0:
                        piece.Type = Enum_Piece.Salon;
                        break;
                    case 1:
                        piece.Type = Enum_Piece.SDB;
                        break;
                    case 2:
                        piece.Type = Enum_Piece.Toilettes;
                        break;
                    case 3:
                        piece.Type = Enum_Piece.Buanderie;
                        break;
                    case 4:
                        piece.Type = Enum_Piece.Cuisine;
                        break;
                }
                piece.Surface = (int)row["surface"];

                if (Convert.ToInt32(row["isPublic"]) == 1)
                {
                    piece.IsPublic = true;
                }
                else
                {
                    piece.IsPublic = false;
                }
                //Pas de listChambre ni Batiment car propriété calculé
                if(row["idBatiment"] != DBNull.Value)
                {
                    piece.Batiment = batimentDAO.FindBatimentWithId(Convert.ToInt32(row["idBatiment"])).Result;
                }
                if (row["ListIdChambre"] != DBNull.Value)
                {
                    piece.Chambres = Parseur_String_ToListOfChambre(row["ListIdChambre"].ToString());
                }
                pieceList.Add(piece);
            }
            
            return pieceList;
        }
        #region Parseur toList et toString

        /// <summary>
        /// Parse une chaîne de caractères représentant une liste d'identifiants de compte en une liste d'objets Account.
        /// </summary>
        /// <param name="stringOfAccounts">Chaîne de caractères représentant une liste d'identifiants de compte.</param>
        /// <returns>Liste d'objets Account.</returns>
        private List<Chambre> Parseur_String_ToListOfChambre(string stringOfChambre)
        {
            ChambreDAO chambreDAO = new ChambreDAO();

            // Initialise une liste d'objets Account
            List<Chambre> chambreList = new List<Chambre>();

            // Sépare la chaîne en un tableau d'identifiants de compte
            string[] tab_Chambre = stringOfChambre.Split(',');

            // Pour chaque identifiant de compte dans le tableau
            foreach (var chambre in tab_Chambre)
            {
                // Récupère l'objet Account correspondant à l'identifiant et l'ajoute à la liste
                chambreList.Add(chambreDAO.FindChambreWithId(Convert.ToInt32(chambre)).Result);
            }

            // Retourne la liste d'objets Account
            return chambreList;
        }

        private string Parseur_List_toStringOfChambre(List<Chambre> chambres)
        {
            if(chambres == null || chambres.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder chambreList = new StringBuilder();
            foreach (var chambre in chambres)
            {
                chambreList.Append(chambre.Id).Append(",");
            }
            if(chambreList.Length > 0)
            {
                chambreList.Length--;
            }
            return chambreList.ToString();
        }

        #endregion
    }
}

