using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Metier
{
    /// <summary>
    /// Classe représentant un bâtiment de colocation
    /// </summary>
    public class Batiment
    {

        #region Attributs

        private int id;
        private string name;
        private string adresse;
        private int codePostale;
        private string ville;
        private int surface;
        private List<Account> accounts;
        private List<Chambre> chambres;
        private List<Piece> pieces;

        #endregion

        #region Propriétés

        /// <summary>
        /// Propriété pour récupérer l'id d'un bâtiment
        /// </summary>
        public int Id { get { return id; } set { id = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier le nom d'un bâtiment
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier l'adresse d'un bâtiment
        /// </summary>
        public string Adresse { get {  return adresse; } set {  adresse = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier le code postale d'un bâtiment
        /// </summary>
        public int CodePostale { get {  return codePostale; } set {  codePostale = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier l'id d'un bâtiment
        /// </summary>
        public string Ville { get {  return ville; } set {  ville = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier la surface d'un bâtiment
        /// </summary>
        public int Surface { get => surface; set => surface = value; }
        /// <summary>
        /// Propriété pour récupérer ou modifier la liste des locataires d'un bâtiment
        /// </summary>
        public List<Account> Accounts { get { return accounts; } set { accounts = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier la liste des chambres d'un bâtiment
        /// </summary>
        public List<Chambre> Chambres { get { return chambres; } set { chambres = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier la liste des pieces d'un bâtiment
        /// </summary>
        public List<Piece> Pieces { get { return pieces; } set { pieces = value; } }


        #endregion

        # region Constructeurs

        /// <summary>
        /// Constructeur d'un bâtiment en récupérant les données dans la BDD
        /// </summary>
        /// <param name="id">id du bâtiment</param>
        /// <param name="name">nom du bâtiment</param>
        /// <param name="adresse">adresse où se situe le bâtiment</param>
        /// <param name="codePostale">code postale de la ville où se situe le bâtiment</param>
        /// <param name="ville">ville où se situe le bâtiment</param>
        /// <param name="surface">surface du bâtiment</param>
        /// <param name="accounts">liste des locataires du bâtiment</param>
        /// <param name="chambres">liste des chambres du bâtiment</param>
        /// <param name="pieces">liste des pieces du bâtiment</param>
        public Batiment(int id, string name, string adresse, int codePostale, string ville, int surface, List<Account> accounts, List<Chambre> chambres, List<Piece> pieces)
        {
            this.id = id;
            this.name = name;
            this.adresse = adresse;
            this.codePostale = codePostale;
            this.ville = ville;
            this.surface = surface;
            this.accounts = accounts;
            this.chambres = chambres;
            this.pieces = pieces;
        }

        /// <summary>
        /// Constructeur d'un bâtiment en récupérant les données dans la BDD
        /// </summary>
        /// <param name="id">id du batiment</param>
        /// <param name="name">nom du batiment</param>
        /// <param name="adresse">adresse du batiment</param>
        /// <param name="codePostale">code postal du batiment</param>
        /// <param name="ville">ville du batiment</param>
        /// <param name="surface">surface du batiment</param>
        public Batiment(int id, string name, string adresse, int codePostale, string ville, int surface)
        {
            this.id = id;
            this.name = name;
            this.adresse = adresse;
            this.codePostale = codePostale;
            this.ville = ville;
            this.surface = surface;
            this.accounts = new List<Account>();
            this.chambres = new List<Chambre>();
            this.pieces = new List<Piece>();
        }
        /// <summary>
        /// Constructeur d'un bâtiment
        /// </summary>
        /// <param name="name">nom du bâtiment</param>
        /// <param name="adresse">adresse où se situe le bâtiment</param>
        /// <param name="codePostale">code postale de la ville où se situe le bâtiment</param>
        /// <param name="ville">ville où se situe le bâtiment</param>
        /// <param name="surface">surface du batiment</param>
        public Batiment(string name, string adresse, int codePostale, string ville, int surface)
        {
            this.name = name;
            this.adresse = adresse;
            this.codePostale = codePostale;
            this.ville = ville;
            this.surface = surface;
            this.accounts = new List<Account>();
            this.chambres = new List<Chambre>();
            this.pieces = new List<Piece>();
        }

        /// <summary>
        /// Constructeur d'un bâtiment vide
        /// </summary>
        public Batiment()
        {

        }

        #endregion
    }
}
