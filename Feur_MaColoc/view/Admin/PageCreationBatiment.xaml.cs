using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Page de cr�ation d'un nouveau b�timent
    /// </summary>
    public partial class PageCreationBatiment : ContentPage
    {
        Grid baseGrid;
        BatimentDAO batimentDao;
        PieceDAO pieceDAO;
        PageGestionBatiments gestionBatiment;
        Label selectedLabel;
        Piece piece;
        List<Piece> pieces;
        int currentRowCount;
        public List<Piece> Pieces
        {
            get { return pieces; }
            set { pieces = value; }
        }
        List<Chambre> chambres;
        public List<Chambre> Chambres
        {
            get { return chambres; }
            set { chambres = value; }
        }

        /// <summary>
        /// Initialise une nouvelle instance de la PageCreationBatiment
        /// </summary>
        /// <param name="GestionBatiment">La page de gestion des b�timents associ�e</param>
        public PageCreationBatiment(PageGestionBatiments GestionBatiment)
        {
            Pieces = new List<Piece>();
            Chambres = new List<Chambre>();
            
            // D�finit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            batimentDao = new BatimentDAO();
            gestionBatiment = GestionBatiment;

            baseGrid = ListPieceGrid;

            ListPiece();
            ScrollView scrollView = new ScrollView
            {
                Content = FinalView
            };
            
            Content = scrollView;
        }

        /// <summary>
        /// G�re l'�v�nement de clic sur le bouton de retour
        /// </summary>
        private async void RetourButton_Clicked(object sender, System.EventArgs e)
        {
            // Retourne � la page pr�c�dente
            await Navigation.PopAsync();
        }

        /// <summary>
        /// �v�nement de click de bouton qui envoie � la page de cr�ation des b�timents
        /// </summary>
        private async void ButtonCreateBatiment_Clicked(object sender, EventArgs e)
        {
            try
            {
                Batiment batiment;

                // V�rifie que les champs requis sont remplis
                if (!string.IsNullOrEmpty(Nom.Text) && !string.IsNullOrEmpty(Adress.Text) && !string.IsNullOrEmpty(CodePostal.Text) && !string.IsNullOrEmpty(Ville.Text) && !string.IsNullOrEmpty(Surface.Text))
                {
                    // Cr�e un objet Batiment avec les donn�es saisies
                    batiment = new Batiment(Nom.Text, Adress.Text, Convert.ToInt32(CodePostal.Text), Ville.Text, Convert.ToInt32(Surface.Text));

                    // Ajoute le nouveau b�timent � la base de donn�es
                    await batimentDao.CreateBatiment(batiment);
                    await DisplayAlert("", "Cr�ation de B�timent R�ussi.", "OK");

                    // Obtient le nombre actuel de b�timents dans la Grid de la page de gestion des b�timents
                    int currentRowCount = gestionBatiment.Grid.RowDefinitions.Count;

                    // Ajoute une nouvelle RowDefinition pour le nouveau b�timent
                    gestionBatiment.Grid.AddRowDefinition(new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Auto)
                    });

                    // Obtient la derni�re RowDefinition existante dans la Grid
                    RowDefinition lastRow = gestionBatiment.Grid.RowDefinitions.LastOrDefault();

                    // Ajoute le label du nouveau b�timent � la suite de la derni�re RowDefinition existante ou � la premi�re si la liste est vide
                    Label label = new Label { VerticalOptions = LayoutOptions.Center, Text = batiment.Name, Margin = 14, FontSize = 21 };
                    gestionBatiment.Grid.Add(label, 1, currentRowCount == 0 ? 0 : currentRowCount + 1);

                    // Ajoute le GestureRecognizer au label
                    var longPressure = new TapGestureRecognizer();
                    longPressure.NumberOfTapsRequired = 1;
                    longPressure.Command = new Command(() =>
                    {
                        gestionBatiment.SelectedLabel = label;
                        gestionBatiment.ExecuteLongPress();
                    });
                    label.GestureRecognizers.Add(longPressure);
                    ChambreDAO chambreDAO = new ChambreDAO();
                    Batiment BatimentWithID = await batimentDao.FindBatiment(batiment.Name);
                    foreach (Piece piece in Pieces)
                    {
                        piece.Batiment = BatimentWithID;
                        pieceDAO.CreatePiece(piece);
                    }
                    foreach (Chambre chambre in Chambres)
                    {
                        chambre.Batiment = BatimentWithID;
                        chambreDAO.CreateChambre(chambre);
                    }

                    // Retourne � la page de gestion des b�timents
                    await Navigation.PopAsync();
                    gestionBatiment.ListBatiment();
                }
                else
                {
                    // Affiche une alerte indiquant que des informations manquent dans un ou plusieurs champs
                    await DisplayAlert("", "Il manque des informations dans un ou plusieurs champs.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Affiche une alerte en cas d'erreur
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        private async void ButtonAddPiece_Clicked(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new PageAjoutPiece(this));
        }
        public async void ListPiece()
        {
            ListPieceGrid.Children.Clear();
            ListPieceGrid.RowDefinitions.Clear();
            double maxLength;
            if (Pieces.Count != 0 || Chambres.Count != 0)
            {


                pieceDAO = new PieceDAO();
                //grid de base sans �l�ment Piece
                ListPieceGrid = baseGrid;
                currentRowCount = ListPieceGrid.RowDefinitions.Count;

                /*var pieces = pieceDAO.getAllPiece().Result;
                List<Piece> listPiece = pieceDAO.ConvertDataTableToList(pieces);*/
                //Faire le tri de, si pi�ce d�j� link...

                // Ajoute des d�finitions de ligne en fonction du nombre de b�timents
                if (Chambres.Count != 0)
                {


                    for (int i = 0; i < Chambres.Count; i++)
                    {
                        ListPieceGrid.AddRowDefinition(new RowDefinition()
                        {
                            Height = new GridLength(1, GridUnitType.Auto)
                        });
                    }
                    maxLength = Chambres.Max(p => p.Name.Length);

                    // Ajoute chaque b�timent � la grille avec des labels et des gestes
                    foreach (Chambre chambre in Chambres)
                    {

                        Label label = new Label { VerticalOptions = LayoutOptions.Center, Text = chambre.Name, Margin = 14, FontSize = 21 };
                        ListPieceGrid.Add(label, 1, currentRowCount);
                        ListPieceGrid.ColumnDefinitions[1].Width = new GridLength(maxLength, GridUnitType.Auto);
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
                        ListPieceGrid.AddRowDefinition(new RowDefinition()
                        {
                            Height = new GridLength(1, GridUnitType.Auto)
                        });
                    }
                    maxLength = Pieces.Max(p => p.Name.Length);

                    // Ajoute chaque b�timent � la grille avec des labels et des gestes
                    foreach (Piece piece in Pieces)
                    {

                        Label label = new Label { VerticalOptions = LayoutOptions.Center, Text = piece.Name, Margin = 14, FontSize = 21 };
                        ListPieceGrid.Add(label, 1, currentRowCount);
                        ListPieceGrid.ColumnDefinitions[1].Width = new GridLength(maxLength, GridUnitType.Auto);
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
            
        }
        public async void ExecuteLongPress()
        {
            List<string> stringsToDo = new List<string>()
            {
                "Voir en d�tail",
                "Supprimer"
            };
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
                            bool isSupprimer = false;
                            if (Pieces.FirstOrDefault(piece => piece.Name == selectedPieceName) != null)
                            {
                                Pieces.Remove(Pieces.FirstOrDefault(piece => piece.Name == selectedPieceName));
                                isSupprimer = true;
                            }
                            if (Chambres.FirstOrDefault(piece => piece.Name == selectedPieceName) != null)
                            {
                                Chambres.Remove(Chambres.FirstOrDefault(chambre => chambre.Name == selectedPieceName));
                                isSupprimer = true;
                            }

                            if (isSupprimer)
                            {
                                // Actualise la Grid en retirant le Label correspondant
                                int rowIndex = ListPieceGrid.GetRow(selectedLabel);

                                if (rowIndex != -1 && rowIndex < ListPieceGrid.RowDefinitions.Count)
                                {
                                    // Faire en sorte que d�s la suppression, on met � jour les diff�rents objets en rapport avec les possibles liens de la pi�ce supprim�e
                                    ListPieceGrid.Children.Remove(selectedLabel);
                                    ListPieceGrid.RowDefinitions.RemoveAt(rowIndex);
                                    ListPiece();
                                }
                                await DisplayAlert("", "Pi�ce supprim� !", "OK");
                            }

                            else
                            {
                                await DisplayAlert("", "Erreur lors de la suppression", "OK");
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
                piece = pieceDAO.FindPiece(selectedPieceName).Result;
                //await Navigation.PushAsync(new PageEditionPiece(this, piece)); a mettre lorsque la page d'edition de piece sera cr��e 
            }
        }
    }
}

