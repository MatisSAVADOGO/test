using Feur_MaColoc_Metier;

namespace Feur_MaColoc.view.Admin;

public partial class PageAjoutPieceEdition : ContentPage
{
    bool isChambre;
    PageEditionBatiment pageEditionBatiment;
    public PageAjoutPieceEdition(PageEditionBatiment pageEditionBatiment)
	{
        this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
        InitializeComponent();
        List<string> piecesList = new List<string>() { "Chambre" };
        
        // Ajouter toutes les valeurs de l'�num�ration � la liste
        foreach (Enum_Piece piece in Enum.GetValues(typeof(Enum_Piece)))
        {
            if (piece == Enum_Piece.SDB)
                piecesList.Add("Salle de bain");
            else
                piecesList.Add(piece.ToString());
            
        }
        TypePiece.ItemsSource = piecesList;
        this.pageEditionBatiment = pageEditionBatiment;
    }


    /// <summary>
    /// G�re le clic sur le bouton de retour
    /// </summary>
    private async void RetourBatiment_Clicked(object sender, EventArgs e)
    {
        // Retourne � la page pr�c�dente
        pageEditionBatiment.ListPiece();
        await Navigation.PopAsync();
    }

    private void ButtonAddPiece_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NomPiece.Text) || string.IsNullOrEmpty(Surface.Text) || TypePiece.SelectedItem == null)
        {
            DisplayAlert("", "Il manque des informations dans un ou plusieurs champs.", "OK");
            return;
        }
        else
        {


            if (isChambre)
            {
                Chambre chambre = new Chambre()
                {
                    Name = NomPiece.Text,
                    Surface = Convert.ToInt32(Surface.Text)
                };
                pageEditionBatiment.Chambres.Add(chambre);
            }
            else
            {
                Piece piece = new Piece()
                {
                    Name = NomPiece.Text,
                    IsPublic = PartageeRadioButton.IsChecked,
                    Surface = Convert.ToInt32(Surface.Text),
                    
                    
                };
                if(TypePiece.SelectedItem.ToString() == "Salle de bain")
                {
                    piece.Type = Enum_Piece.SDB;
                }
                else
                {
                    piece.Type = (Enum_Piece)Enum.Parse(typeof(Enum_Piece), TypePiece.SelectedItem.ToString());
                }
                pageEditionBatiment.Pieces.Add(piece);
            }
            NomPiece.Text = "";
            Surface.Text = "";
            DisplayAlert("", "Pi�ce ajout�e", "OK");
        }

    }

    private void TypePiece_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(TypePiece.SelectedItem.ToString() == "Chambre" || TypePiece.SelectedItem.ToString() == "Cuisine" || TypePiece.SelectedItem.ToString() == "Buanderie" || TypePiece.SelectedItem.ToString() == "Salon")
        {
            CocheIsPublic.IsVisible = false;
            PartageeRadioButton.IsChecked = true;
            NonPartageeRadioButton.IsChecked = false;
            isChambre = false;
            if(TypePiece.SelectedItem.ToString() == "Chambre")
            {
                isChambre = true;
                PartageeRadioButton.IsChecked = false;
                NonPartageeRadioButton.IsChecked = true;
            }
                
        }
        else
        {
            CocheIsPublic.IsVisible = true;
        }
        
        
    }
}