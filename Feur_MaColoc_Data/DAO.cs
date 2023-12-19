using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Data
{
    public abstract class DAO<T>
    {
        /// <summary>
        /// Représente l'objet qui sera utilisé pour les requêtes SQL
        /// </summary>
        protected Database database;

        /// <summary>
        /// Constructeur de la classe DAO
        /// </summary>
        public DAO()
        {
            this.database = new Database();
        }

        /// <summary>
        /// Méthode abstraite qui permet de convertir un "DataTable" en une liste d'objet modulable par rapport au DAO qui l'utilise
        /// </summary>
        /// <param name="dataTable">Représente une table SQL, récupérer lors de la méthode database.Select</param>
        /// <returns>Retourne une Liste d'objet du l'objetDAO</returns>
        public abstract List<T> ConvertDataTableToList(DataTable dataTable);
    }
}
