using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Metier
{
    /// <summary>
    /// Classe représentant une chambre d'un bâtiment de colocation
    /// </summary>
    public class Chambre
    {
        #region Attributs

        private int id;
        private string name;
        private int surface;
        private Account locataire;
        private Batiment batiment;

        #endregion

        #region Propriétés

        /// <summary>
        /// Propriété pour récupérer l'id d'une chambre
        /// </summary>
        public int Id { get { return id; } set { id = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier le nom d'une chambre
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier la surface d'une chambre
        /// </summary>
        public int Surface { get => surface; set => surface = value; }
        /// <summary>
        /// Propriété pour récupérer ou modifier le locataire d'une chambre
        /// </summary>
        public Account Locataire { get => locataire; set => locataire = value; }
        /// <summary>
        /// Propriété pour récupérer ou modifier le bâtiment d'une chambre
        /// </summary>
        public Batiment Batiment { get => batiment; set => batiment = value; }
        #endregion

        # region Constructeurs

        /// <summary>
        /// Constructeur d'une chambre en récupérant les données dans la BDD
        /// </summary>
        /// <param name="id">id de la chambre</param>
        /// <param name="name">nom de la chambre</param>
        /// <param name="surface">surface de la chambre</param>
        /// <param name="locataire">liste des chambres de la chambre</param>
        /// <param name="batiment">batiment d'une piece</param>
        public Chambre(int id, string name, int surface, Account locataire, Batiment batiment)
        {
            this.id = id;
            this.name = name;
            this.surface = surface;
            this.Locataire = locataire;
            this.batiment = batiment;
        }

        /// <summary>
        /// Constructeur d'une chambre en récupérant les données dans la BDD
        /// </summary>
        /// <param name="id">id de la chambre</param>
        /// <param name="name">nom de la chambre</param>
        /// <param name="surface">surface de la chambre</param>
        /// <param name="batiment">batiment d'une piece</param>
        public Chambre(int id, string name, int surface, Batiment batiment)
        {
            this.id = id;
            this.name = name;
            this.surface = surface;
            this.locataire = null;
            this.batiment = batiment;
        }
        /// <summary>
        /// Constructeur d'une chambre
        /// </summary>
        /// <param name="name">nom de la chambre</param>
        /// <param name="surface">surface de la chambre</param>
        /// <param name="locataire">type de la chambre</param>
        /// <param name="batiment">batiment d'une piece</param>
        public Chambre(string name, int surface, Account locataire, Batiment batiment)
        {
            this.name = name;
            this.surface = surface;
            AddLocataire(locataire);
            this.batiment = batiment;
        }

        /// <summary>
        /// Constructeur d'une chambre vide
        /// </summary>
        public Chambre()
        {

        }

        #endregion


        /// <summary>
        /// Ajoute un locataire a la chambre et ajoute la chambre au locataire
        /// </summary>
        /// <param name="locataire">locataire habitant notre chambre</param>
        public void AddLocataire(Account locataire)
        {
            this.Locataire = locataire;
            locataire.Chambre = this;
        }
    }
}
