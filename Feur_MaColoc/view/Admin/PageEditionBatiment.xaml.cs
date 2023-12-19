 using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using System.Data;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Page permettant la modification d'un bâtiment existant
    /// </summary>
    public partial class PageEditionBatiment : ContentPage
    {
        AccountDAO accountDAO = new AccountDAO();
        BatimentDAO batimentDAO = new BatimentDAO();
        Batiment batiment_A_Modifier;
        PageGestionBatiments pageGestionBatiments;
        List<Account> accountBatimentModifier;
        Account accountToAdd;
        Account accountToRemove;
        PieceDAO pieceDAO;
        ChambreDAO chambreDAO;
        Piece piece;
        List<Piece> pieces;
        Chambre chambre;
        List<Chambre> chambres;
        Grid baseGrid;
        int currentRowCount;
        Label selectedLabel;
        bool isChambre;
        public Piece Piece
        {
            get { return piece; }
            set { piece = value; }
        }

        public List<Piece> Pieces
        {
            get { return pieces; }
            set { pieces = value; }
        }
        public Chambre Chambre
        {
            get { return chambre; }
            set { chambre = value; }
        }
        public List<Chambre> Chambres
        {
            get { return chambres; }
            set { chambres = value; }
        }
        public Batiment Batiment { get => batiment_A_Modifier; set => batiment_A_Modifier = value; }



        /// <summary>
        /// Initialise une nouvelle instance de la PageEditionBatiment
        /// </summary>
        /// <param name="pageGestionBatiments">La page de gestion des bâtiments associée</param>
        /// <param name="batiment">Le bâtiment à modifier</param>
        public PageEditionBatiment(PageGestionBatiments pageGestionBatiments, Batiment batiment)
        {
            // Définit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();

            baseGrid = gridd;
            //permet de remplir les champs à modifier avec les informations actuelles du bâtiment
            batiment_A_Modifier = batiment;
            accountBatimentModifier = new List<Account>();
            BatimentModifier.Text = "Modification du batiment : " + batiment.Name;
            Nom.Text = batiment_A_Modifier.Name;
            Adress.Text = batiment_A_Modifier.Adresse;
            CodePostal.Text = batiment_A_Modifier.CodePostale.ToString();
            Ville.Text = batiment_A_Modifier.Ville;
            Surface.Text = batiment_A_Modifier.Surface.ToString();
            FrameOfAllEntry.Margin = new Thickness(25, 25, 25, 25);

            Nom.Margin = new Thickness(0, 10, 0, 10);
            Adress.Margin = new Thickness(0, 0, 0, 10);
            CodePostal.Margin = new Thickness(0, 0, 0, 10);
            Ville.Margin = new Thickness(0, 0, 0, 10);
            Surface.Margin = new Thickness(0, 0, 0, 10);
            this.pageGestionBatiments = pageGestionBatiments;

            ListPiece();
            ScrollView scrollView = new ScrollView
            {
                Content = FinalView
            };

            Content = scrollView;



        }


        /// <summary>
        /// Gère la modification des informations du bâtiment
        /// </summary>
        private async void ModifBatiment_Clicked(object sender, EventArgs e)
        {
            // Vérifie que les champs requis sont remplis
            if (!string.IsNullOrEmpty(Nom.Text) && !string.IsNullOrEmpty(Adress.Text) && !string.IsNullOrEmpty(CodePostal.Text) && !string.IsNullOrEmpty(Ville.Text) && !string.IsNullOrEmpty(Surface.Text))
            {
                //permet de gèrer le contenu des entry (champs de texte) et leurs valeurs
                batiment_A_Modifier.Name = Nom.Text;
                batiment_A_Modifier.Adresse = Adress.Text;
                batiment_A_Modifier.CodePostale = Convert.ToInt32(CodePostal.Text);
                batiment_A_Modifier.Ville = Ville.Text;
                batiment_A_Modifier.Surface = Convert.ToInt32(Surface.Text);

                await batimentDAO.EditBatiment(batiment_A_Modifier);

                await Navigation.PopAsync();
                pageGestionBatiments.ListBatiment();
            }
            else
            {
                // Affiche une alerte indiquant que des informations manquent dans un ou plusieurs champs
                await DisplayAlert("", "Il manque des informations dans un ou plusieurs champs.", "OK");
            }
        }

        /// <summary>
        /// Gère le clic sur le bouton de retour
        /// </summary>
        private async void RetourButton_Clicked(object sender, EventArgs e)
        {
            // Retourne à la page précédente
            await Navigation.PopAsync();
        }

        private async void ButtonAddPiece_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new PageAjoutPieceEdition(this));
        }

        public async void ListPiece()
        {
            pieceDAO = new PieceDAO();
            chambreDAO = new ChambreDAO();
            var piecesDataTable = pieceDAO.getAllPiece().Result;
            List<Piece> listpieces = pieceDAO.ConvertDataTableToList(piecesDataTable);
            var chambresDataTable = chambreDAO.RecupAllChambreBDD().Result;
            List<Chambre> listchambres = chambreDAO.ConvertDataTableToList(chambresDataTable);
            
            List<Piece> triListPieces = new List<Piece>();
            List<Chambre> triListChambres = new List<Chambre>();
            foreach(var piece in listpieces)
            {
                if (piece.Batiment != null)
                {
                    if (piece.Batiment.Id != null)
                    {
                        triListPieces.Add(piece);
                    }
                }
            }
            foreach (var chambre in listchambres)
            {
                if (chambre.Batiment != null)
                {
                    if (chambre.Batiment.Id != null)
                    {
                        triListChambres.Add(chambre);
                    }
                }
            }
            
            
            Pieces = triListPieces.Where(p => p.Batiment.Id == batiment_A_Modifier.Id).ToList();
            Chambres = triListChambres.Where(p => p.Batiment.Id == batiment_A_Modifier.Id).ToList();

            double maxLength;
            if (Pieces.Count != 0 || Chambres.Count != 0)
            {
                //grid de base sans élément Piece
                gridd = baseGrid;
                currentRowCount = gridd.RowDefinitions.Count;


                //Faire le tri de, si pièce déjà link...

                // Ajoute des définitions de ligne en fonction du nombre de bâtiments
                if (Chambres.Count != 0)
                {


                    for (int i = 0; i < Chambres.Count; i++)
                    {
                        gridd.AddRowDefinition(new RowDefinition()
                        {
                            Height = new GridLength(1, GridUnitType.Auto)
                        });
                    }
                    maxLength = Chambres.Max(p => p.Name.Length);

                    // Ajoute chaque bâtiment à la grille avec des labels et des gestes
                    foreach (Chambre chambre in Chambres)
                    {

                        Label label = new Label { VerticalOptions = LayoutOptions.Center, Text = chambre.Name, Margin = 14, FontSize = 21 };
                        gridd.Add(label, 1, currentRowCount);
                        gridd.ColumnDefinitions[1].Width = new GridLength(maxLength, GridUnitType.Auto);
                        var longPressure = new TapGestureRecognizer();
                        longPressure.NumberOfTapsRequired = 1;
                        longPressure.Command = new Command(() =>
                        {
                            selectedLabel = label;
                            ExecuteLongPress();
                        });
                        label.GestureRecognizers.Add(longPressure);
                        currentRowCount++;
                    }


                }
                if (Pieces.Count != 0)
                {

                    // Ajoute des définitions de ligne en fonction du nombre de bâtiments
                    for (int i = 0; i < Pieces.Count; i++)
                    {
                        gridd.AddRowDefinition(new RowDefinition()
                        {
                            Height = new GridLength(1, GridUnitType.Auto)
                        });
                    }
                    maxLength = Pieces.Max(p => p.Name.Length);

                    // Ajoute chaque bâtiment à la grille avec des labels et des gestes
                    foreach (Piece piece in Pieces)
                    {

                        Label label = new Label { VerticalOptions = LayoutOptions.Center, Text = piece.Name, Margin = 14, FontSize = 21 };
                        gridd.Add(label, 1, currentRowCount);
                        gridd.ColumnDefinitions[1].Width = new GridLength(maxLength, GridUnitType.Auto);
                        var longPressure = new TapGestureRecognizer();
                        longPressure.NumberOfTapsRequired = 1;
                        longPressure.Command = new Command(() =>
                        {
                            selectedLabel = label;
                            ExecuteLongPress();
                        });
                        label.GestureRecognizers.Add(longPressure);
                        currentRowCount++;
                    }
                }
            }
            else
            {

                gridd.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Star);

            }
        }
        public async void ExecuteLongPress()
        {
            List<string> stringsToDo = new List<string>()
            {
                "Voir en détail",
                "Supprimer"
            };
            if (Chambres.FirstOrDefault(chambre => chambre.Name == selectedLabel?.Text) != null)
            {
                stringsToDo.Add("Ajouter des pièces privées");
            }

            string selectedTaskName = await DisplayActionSheet("Sélectionnez une action à faire", "Annuler", null, stringsToDo.ToArray());
            string selectedPieceName = selectedLabel?.Text;

            // Vérifie si l'utilisateur a sélectionné un élément autre que "Annuler"
            if (selectedTaskName != "Annuler")
            {
                // Vérifie si l'utilisateur a choisi "Supprimer"
                if (selectedTaskName == "Supprimer")
                {
                    // Continue avec la suppression seulement si "Supprimer" a été choisi
                    if (selectedPieceName != null)
                    {
                        try
                        {
                            if (chambreDAO.FindChambre(selectedPieceName).Result != null)
                            {
                                chambre = chambreDAO.FindChambre(selectedPieceName).Result;
                            }
                            if (pieceDAO.FindPiece(selectedPieceName).Result != null)
                            {
                                piece = pieceDAO.FindPiece(selectedPieceName).Result;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                        if (chambre != null)
                        {
                            try
                            {
                                if (await chambreDAO.DeleteChambre(chambre))
                                {

                                    // Actualise la Grid en retirant le Label correspondant
                                    int rowIndex = gridd.GetRow(selectedLabel);

                                    if (rowIndex != -1 && rowIndex < gridd.RowDefinitions.Count)
                                    {
                                        // Faire en sorte que dès la suppression, on met à jour les différents objets en rapport avec les possibles liens de la pièce supprimée
                                        gridd.Children.Remove(selectedLabel);
                                        gridd.RowDefinitions.RemoveAt(rowIndex);
                                        ListPiece();
                                        await DisplayAlert("", "Chambre supprimé !", "OK");
                                    }
                                    else
                                    {
                                        await DisplayAlert("", "Erreur lors de la suppression", "OK");
                                    }
                                }


                                if (await pieceDAO.DeletePiece(piece))
                                {
                                    // Actualise la Grid en retirant le Label correspondant
                                    int rowIndex = gridd.GetRow(selectedLabel);

                                    if (rowIndex != -1 && rowIndex < gridd.RowDefinitions.Count)
                                    {
                                        // Faire en sorte que dès la suppression, on met à jour les différents objets en rapport avec les possibles liens de la pièce supprimée
                                        gridd.Children.Remove(selectedLabel);
                                        gridd.RowDefinitions.RemoveAt(rowIndex);
                                        ListPiece();
                                        await DisplayAlert("", "Pièce supprimé !", "OK");
                                    }
                                    else
                                    {
                                        await DisplayAlert("", "Erreur lors de la suppression", "OK");
                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                throw ex.InnerException;
                            }
                        }
                    }
                }
                //permet d'aller sur la page d'édition si voir en détail a été choisi
                else if (selectedTaskName == "Voir en détail")
                {
                    if (chambreDAO.FindChambre(selectedPieceName).Result != null && chambreDAO.FindChambre(selectedPieceName).Result.Id != 0 && chambreDAO.FindChambre(selectedPieceName).Result.Id != null)
                    {
                        chambre = chambreDAO.FindChambre(selectedPieceName).Result;
                        isChambre = true;
                        await Navigation.PushAsync(new PageEditionPiece(this, isChambre, chambre));
                    }

                    if (pieceDAO.FindPiece(selectedPieceName).Result != null && pieceDAO.FindPiece(selectedPieceName).Result.Id != 0 && pieceDAO.FindPiece(selectedPieceName).Result.Id != null)
                    {
                        piece = pieceDAO.FindPiece(selectedPieceName).Result;
                        isChambre = false;
                        await Navigation.PushAsync(new PageEditionPiece(this, isChambre, piece));
                    }

                    
                }
                else if (selectedTaskName == "Ajouter des pièces privées")
                {
                    // Récupère les pièces privées
                    var piecesPrivees = pieces.Where(p => p.IsPublic == false && p.Chambres == null ).ToList();

                    var piecesNames = piecesPrivees.Select(p => p.Name).ToArray();

                    string selectedPrivatePiece = await DisplayActionSheet("Sélectionnez une pièce à ajouter", "Valider", null, piecesNames);
                    if (selectedPrivatePiece != "Valider")
                    {
                        //plus que la logique à implémenter
                    }
                }
            }
        }
    }
}
