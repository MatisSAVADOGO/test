using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Metier
{
    /// <summary>
    /// Classe représentant une piece d'un batiment de colocation
    /// </summary>
    public class Piece
    {

        #region Attributs

        private int id;
        private string name;
        private int surface;
        private Enum_Piece type;
        private bool isPublic; //caractere public ou privé d'une pièce car une salle de bain ou des toillettes peuvent être privées
        private List<Chambre> chambres;
        private Batiment batiment;

        #endregion

        #region Propriétés

        /// <summary>
        /// Propriété pour récupérer l'id d'une pièce
        /// </summary>
        public int Id { get { return id; } set { id = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier le nom d'une pièce
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier la surface d'une pièce
        /// </summary>
        public int Surface { get => surface; set => surface = value; }
        /// <summary>
        /// Propriété pour récupérer ou modifier le type d'une pièce
        /// </summary>
        public Enum_Piece Type { get => type; set => type = value; }
        /// <summary>
        /// Propriété pour récupérer ou modifier le caractere public d'une pièce
        /// </summary>
        public bool IsPublic { get => isPublic; set => isPublic = value; }
        /// <summary>
        /// Propriété pour récupérer ou modifier la liste des chambres d'une pièce
        /// </summary>
        public List<Chambre> Chambres { get { return chambres; } set { chambres = value; } }
        /// <summary>
        /// Propriété pour récupérer ou modifier le bâtiment d'une pièce
        /// </summary>
        public Batiment Batiment { get => batiment; set => batiment = value; }
        #endregion

        # region Constructeurs

        /// <summary>
        /// Constructeur d'une pièce en récupérant les données dans la BDD
        /// </summary>
        /// <param name="id">id de la pièce</param>
        /// <param name="name">nom de la pièce</param>
        /// <param name="surface">surface de la pièce</param>
        /// <param name="type">type de la pièce</param>
        /// <param name="isPublic">caractere public ou privé d'une piece</param>
        /// <param name="chambres">liste des chambres de la pièce</param>
        /// <param name="batiment">batiment d'une piece</param>
        public Piece(int id, string name, int surface, Enum_Piece type, bool isPublic, List<Chambre> chambres, Batiment batiment)
        {
            this.id = id;
            this.name = name;
            this.surface = surface;
            this.type = type;
            this.isPublic = isPublic;
            this.chambres = chambres;
            this.batiment = batiment;
        }

        /// <summary>
        /// Constructeur d'une piece en récupérant les données dans la BDD
        /// </summary>
        /// <param name="id">id de la pièce</param>
        /// <param name="name">nom de la pièce</param>
        /// <param name="surface">surface de la pièce</param>
        /// <param name="type">type de la pièce</param>
        /// <param name="isPublic">caractere public ou privé d'une piece</param>
        /// <param name="batiment">batiment d'une piece</param>
        public Piece(int id, string name, int surface, Enum_Piece type, bool isPublic, Batiment batiment)
        {
            this.id = id;
            this.name = name;
            this.surface = surface;
            this.type = type;
            this.isPublic = isPublic;
            this.chambres = new List<Chambre>();
            this.batiment = batiment;
        }
        /// <summary>
        /// Constructeur d'une pièce
        /// </summary>
        /// <param name="name">nom de la pièce</param>
        /// <param name="surface">surface de la pièce</param>
        /// <param name="type">type de la pièce</param>
        /// <param name="isPublic">caractere public ou privé d'une piece</param>
        /// <param name="batiment">batiment d'une piece</param>
        public Piece(string name, int surface, Enum_Piece type, bool isPublic, Batiment batiment)
        {
            this.name = name;
            this.surface = surface;
            this.type = type;
            this.isPublic = isPublic;
            this.chambres = new List<Chambre>();
            this.batiment = batiment;
        }

        /// <summary>
        /// Constructeur d'une pièce vide
        /// </summary>
        public Piece()
        {

        }

        #endregion
    }
}
