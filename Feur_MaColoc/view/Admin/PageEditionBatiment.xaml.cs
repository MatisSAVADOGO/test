 using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using System.Data;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Page permettant la modification d'un b�timent existant
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
        /// <param name="pageGestionBatiments">La page de gestion des b�timents associ�e</param>
        /// <param name="batiment">Le b�timent � modifier</param>
        public PageEditionBatiment(PageGestionBatiments pageGestionBatiments, Batiment batiment)
        {
            // D�finit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();

            baseGrid = gridd;
            //permet de remplir les champs � modifier avec les informations actuelles du b�timent
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
        /// G�re la modification des informations du b�timent
        /// </summary>
        private async void ModifBatiment_Clicked(object sender, EventArgs e)
        {
            // V�rifie que les champs requis sont remplis
            if (!string.IsNullOrEmpty(Nom.Text) && !string.IsNullOrEmpty(Adress.Text) && !string.IsNullOrEmpty(CodePostal.Text) && !string.IsNullOrEmpty(Ville.Text) && !string.IsNullOrEmpty(Surface.Text))
            {
                //permet de g�rer le contenu des entry (champs de texte) et leurs valeurs
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
        /// G�re le clic sur le bouton de retour
        /// </summary>
        private async void RetourButton_Clicked(object sender, EventArgs e)
        {
            // Retourne � la page pr�c�dente
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
                //grid de base sans �l�ment Piece
                gridd = baseGrid;
                currentRowCount = gridd.RowDefinitions.Count;


                //Faire le tri de, si pi�ce d�j� link...

                // Ajoute des d�finitions de ligne en fonction du nombre de b�timents
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

                    // Ajoute chaque b�timent � la grille avec des labels et des gestes
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

                    // Ajoute des d�finitions de ligne en fonction du nombre de b�timents
                    for (int i = 0; i < Pieces.Count; i++)
                    {
                        gridd.AddRowDefinition(new RowDefinition()
                        {
                            Height = new GridLength(1, GridUnitType.Auto)
                        });
                    }
                    maxLength = Pieces.Max(p => p.Name.Length);

                    // Ajoute chaque b�timent � la grille avec des labels et des gestes
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
                "Voir en d�tail",
                "Supprimer"
            };
            if (Chambres.FirstOrDefault(chambre => chambre.Name == selectedLabel?.Text) != null)
            {
                stringsToDo.Add("Ajouter des pi�ces priv�es");
            }

            string selectedTaskName = await DisplayActionSheet("S�lectionnez une action � faire", "Annuler", null, stringsToDo.ToArray());
            string selectedPieceName = selectedLabel?.Text;

            // V�rifie si l'utilisateur a s�lectionn� un �l�ment autre que "Annuler"
            if (selectedTaskName != "Annuler")
            {
                // V�rifie si l'utilisateur a choisi "Supprimer"
                if (selectedTaskName == "Supprimer")
                {
                    // Continue avec la suppression seulement si "Supprimer" a �t� choisi
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
                                        // Faire en sorte que d�s la suppression, on met � jour les diff�rents objets en rapport avec les possibles liens de la pi�ce supprim�e
                                        gridd.Children.Remove(selectedLabel);
                                        gridd.RowDefinitions.RemoveAt(rowIndex);
                                        ListPiece();
                                        await DisplayAlert("", "Chambre supprim� !", "OK");
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
                                        // Faire en sorte que d�s la suppression, on met � jour les diff�rents objets en rapport avec les possibles liens de la pi�ce supprim�e
                                        gridd.Children.Remove(selectedLabel);
                                        gridd.RowDefinitions.RemoveAt(rowIndex);
                                        ListPiece();
                                        await DisplayAlert("", "Pi�ce supprim� !", "OK");
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
                //permet d'aller sur la page d'�dition si voir en d�tail a �t� choisi
                else if (selectedTaskName == "Voir en d�tail")
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
                else if (selectedTaskName == "Ajouter des pi�ces priv�es")
                {
                    // R�cup�re les pi�ces priv�es
                    var piecesPrivees = pieces.Where(p => p.IsPublic == false && p.Chambres == null ).ToList();

                    var piecesNames = piecesPrivees.Select(p => p.Name).ToArray();

                    string selectedPrivatePiece = await DisplayActionSheet("S�lectionnez une pi�ce � ajouter", "Valider", null, piecesNames);
                    if (selectedPrivatePiece != "Valider")
                    {
                        //plus que la logique � impl�menter
                    }
                }
            }
        }
    }
}
