using Feur_MaColoc_Data;
using Feur_MaColoc_Metier;
using Syncfusion.Maui.Core.Internals;
using System.IO.Pipelines;

namespace Feur_MaColoc.view.Admin;

public partial class PageEditionPiece : ContentPage
{
    private string typePiece;
    AccountDAO accountDAO = new AccountDAO();
    ChambreDAO chambreDAO = new ChambreDAO();
    Batiment batiment_A_Modifier;
    List<Account> accountBatimentModifierForChambre;
    List<Piece> accountBatimentModifierForPiecePrivee;
    PageEditionBatiment pageEditionBatiment;
    List<Piece> piecesRemoved;
    Account accountToAdd;
    Account ancienLocataire;
    Chambre chambreToModif;
    Piece pieceToModif;

    public PageEditionPiece(PageEditionBatiment pageEditionBatiment, bool isChambre, object Piece)
    {
        this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
        InitializeComponent();
        this.pageEditionBatiment = pageEditionBatiment;
        batiment_A_Modifier = pageEditionBatiment.Batiment;

        if(Piece is Chambre)
        {
            chambreToModif = (Chambre)Piece;
            if(chambreToModif.Locataire != null)
            {
                ancienLocataire = chambreToModif.Locataire;
            }
        }
        else if(Piece is Piece)
        {
            pieceToModif = (Piece)Piece;
        }

        if (isChambre)
        {
            TypePiece.Text = "Chambre";
            NomPiece.Text = pageEditionBatiment.Chambre.Name;
            Surface.Text = pageEditionBatiment.Chambre.Surface.ToString();
        }
        else
        {
            TypePiece.Text = pageEditionBatiment.Piece.Type.ToString();
            if (pageEditionBatiment.Piece.Type.ToString() == "SDB")
            {
                TypePiece.Text = "Salle de Bain";
            }
            NomPiece.Text = pageEditionBatiment.Piece.Name;
            Surface.Text = pageEditionBatiment.Piece.Surface.ToString();
        }
        

        accountBatimentModifierForChambre = new List<Account>();
        accountBatimentModifierForPiecePrivee = new List<Piece>();
        piecesRemoved = new List<Piece> ();

        // Stockez le type de pièce dans le champ
        typePiece = TypePiece.Text;

        // Vérifiez le type de pièce
        if (typePiece == "Chambre")
        {
            CocheIsPublic.IsVisible = false;
            // Ajoutez les boutons dynamiquement
            Button ajouterColocButton = new Button
            {
                Text = "Ajouter un Locataire",
                BackgroundColor = Color.FromArgb("#3860B2"),
                Margin = new Thickness(20, 0, 20, 5)
            };
            ajouterColocButton.Clicked += AjoutColocChambre;
            stackLayout.Children.Add(ajouterColocButton);

            Button supprimerColocButton = new Button
            {
                Text = "Supprimer un colocataire",
                BackgroundColor = Color.FromArgb("#3860B2"),
                Margin = new Thickness(20, 0, 20, 10)
            };
            supprimerColocButton.Clicked += SupprColocChambre;
            stackLayout.Children.Add(supprimerColocButton);

/*            Button ajouterPiecePriveeButton = new Button
            {
                Text = "Ajouter une pièce privée",
                BackgroundColor = Color.FromArgb("#3860B2"),
                Margin = new Thickness(20, 0, 20, 5)
            };
            ajouterPiecePriveeButton.Clicked += AjoutPiecePriveePourChambre;
            stackLayout.Children.Add(ajouterPiecePriveeButton);
            Button supprimerPiecePriveeButton = new Button
            {
                Text = "Supprimer une pièce privée",
                BackgroundColor = Color.FromArgb("#3860B2"),
                Margin = new Thickness(20, 0, 20, 5)
            };
            supprimerPiecePriveeButton.Clicked += SupprPiecePriveePourChambre;
            stackLayout.Children.Add(supprimerPiecePriveeButton);*/

        }
        else
        {
            if(pageEditionBatiment.Piece.IsPublic)
            {
                PartageeRadioButton.IsChecked = true;
                NonPartageeRadioButton.IsChecked = false;
            }
            else
            {
                PartageeRadioButton.IsChecked = false;
                NonPartageeRadioButton.IsChecked = true;
            }
            // Ajoutez les boutons dynamiquement
            Button ajouterColocButton = new Button
            {
                Text = "Ajouter un colocataire",
                BackgroundColor = Color.FromArgb("#3860B2"),
                Margin = new Thickness(20, 0, 20, 5)
            };
            ajouterColocButton.Clicked += AjoutColocPiece;
            stackLayout.Children.Add(ajouterColocButton);

            Button supprimerColocButton = new Button
            {
                Text = "Supprimer un colocataire",
                BackgroundColor = Color.FromArgb("#3860B2"),
                Margin = new Thickness(20, 0, 20, 5)
            };
            supprimerColocButton.Clicked += SupprColocPiece;
            stackLayout.Children.Add(supprimerColocButton);
        }
        Button validationButton = new Button()
        {
            Text = "Valider les modification",
            BackgroundColor = Color.FromArgb("#3860B2"),
            Margin = new Thickness(20, 10, 20, 5),

        };
        validationButton.Clicked += ValiderModification_Clicked;
        stackLayout.Children.Add(validationButton);
    }
    /// <summary>
    /// Gère la suppression d'un colocataire du bâtiment
    /// </summary>
    private async void SupprColocChambre(object sender, EventArgs e)
    {
        
        List<Account> listAccountInBatiment = accountBatimentModifierForChambre;
        List<Account> listAccount = accountDAO.ConvertDataTableToList(await accountDAO.RecupAllCompteBDD());
        listAccount = listAccount.Where(account => account.Chambre == chambreToModif).ToList();
        foreach(Account account in listAccount)
        {
            listAccountInBatiment.Add(account);
        }
        List<string> listAccountString = new List<string>();
        foreach (Account account in listAccountInBatiment)
        {
            listAccountString.Add(account.Username);
        }

        string selectedAccount = await DisplayActionSheet("Choisissez un colocataire", "Annuler", null, listAccountString.ToArray());
        if (selectedAccount != null && selectedAccount != "Annuler")
        {
            BatimentDAO batimentDAO = new BatimentDAO();
            Account accountToRemove = listAccountInBatiment.FirstOrDefault(account => account.Username == selectedAccount);

            accountToRemove.Chambre = null;
            pageEditionBatiment.Chambre.Locataire = null;

            accountBatimentModifierForChambre.Remove(accountToRemove);

            await DisplayAlert("", "Locataire supprimé de la chambre", "OK");
        }
    }


    /// <summary>
    /// Gère l'ajout d'un colocataire au bâtiment
    /// </summary>
    private async void AjoutColocChambre(object sender, EventArgs e)
    {
        var listAccountDataTable = await accountDAO.RecupAllCompteBDD();
        var listAccount = accountDAO.ConvertDataTableToList(listAccountDataTable);
        List<string> listAccountString = new List<string>();
        
        //On prend que les comptes qui n'ont pas de chambre et qui ne viennent pas déjà d'être ajouté pendant la modification
        List<Account> listAccountWithChambre = listAccount.Where(account => account.Chambre == null 
        && !accountBatimentModifierForChambre.Any(existingAccount => existingAccount.Username == account.Username)).ToList();

        
        foreach (Account account in listAccountWithChambre)
        {
            listAccountString.Add(account.Username);
        }
        string selectedAccount = await DisplayActionSheet("Choisissez un colocataire", "Annuler", null, listAccountString.ToArray());

        //Si il y a déjà un locataire dans la chambre on ne peut pas en ajouter un autre
        if(accountBatimentModifierForChambre.Count > 0)
        {
            await DisplayAlert("", "Vous ne pouvez mettre qu'un locataire a une chambre", "OK");
        }
        else
        {
            //Si on a bien choisi un compte et qu'on a pas cliqué sur annuler
            if (selectedAccount != null && selectedAccount != "Annuler")
            {
                // On créer une personne et on l'ajoute à la liste des personnes qui vont être ajouté au batiment
                BatimentDAO batimentDAO = new BatimentDAO();
                accountToAdd = listAccountWithChambre.Where(account => account.Username == selectedAccount).FirstOrDefault();
                accountToAdd.Chambre = pageEditionBatiment.Chambre;
                pageEditionBatiment.Chambre.Locataire = accountToAdd;
                accountBatimentModifierForChambre.Add(accountToAdd);
                await DisplayAlert("", "Locataire ajouté à la chambre", "OK");
            }
        }
        

    }

    /// <summary>
    /// Gère la suppression d'un colocataire du bâtiment
    /// </summary>
    private async void SupprPiecePriveePourChambre(object sender, EventArgs e)
    {
        List<Piece> listPieceInBatiment = accountBatimentModifierForPiecePrivee;
        List<string> listAccountString = new List<string>();
        foreach (Piece piece in listPieceInBatiment)
        {
            listAccountString.Add(piece.Name);
        }

        string selectedPiece = await DisplayActionSheet("Choisissez une pièce privée", "Annuler", null, listAccountString.ToArray());
        if (selectedPiece != null && selectedPiece != "Annuler")
        {
            BatimentDAO batimentDAO = new BatimentDAO();
            Piece pieceToRemove = listPieceInBatiment.FirstOrDefault(piece => piece.Name == selectedPiece);


            accountBatimentModifierForPiecePrivee.Remove(pieceToRemove);
            piecesRemoved.Add(pieceToRemove);
            await DisplayAlert("", "Locataire supprimé de la chambre", "OK");
        }
    }
    /// <summary>
    /// Gère l'ajout d'un colocataire au bâtiment
    /// </summary>
    private async void AjoutPiecePriveePourChambre(object sender, EventArgs e)
    {

        //pour chambre => toute les pièces qui sont privé, pas déjà attribué a une autre chambre (vu que c'est privé) et ce sont forcément SDB ou toilette
        
            PieceDAO pieceDAO = new PieceDAO();
            List<Piece> listPiece = pieceDAO.ConvertDataTableToList(await pieceDAO.getAllPiece());
            int u = 0;
            //On prend que les pièces qui sont des WC ou Salle de bain ET qui ne sont pas publique ET qui n'ont pas déjà une chambre (car privé = 1 chambre -> 1 locataire)
            List<Piece> listPiecePrivee = listPiece.Where(piece => piece.IsPublic == false).ToList();
            listPiecePrivee = listPiecePrivee.Where(piece => piece.Type == Enum_Piece.SDB || piece.Type == Enum_Piece.Toilettes).ToList();

            //On veut afficher les pièces privée sans chambre

            //S'il y a au moins une pièce privée  sans chambre
            if(listPiecePrivee.Any(piece => piece.Chambres == null))
            {
                //On prend juste les pièces privée sans chambre
                listPiecePrivee = listPiecePrivee.Where(piece => piece.Chambres == null).ToList();
            }
            else
            {
                //Dans le cas où toutes les pièces privée ont une chambre
                DisplayAlert("", "Toutes les pièces privée ont chambre ", "OK");
            }
            //On prend toutes les pièces privée qui ne sont pas déjà dans la variable accountBatimentModifierForPiecePrivee (variable temporaire de modification des pièces)
            //accountBatimentModifierForPiecePrivee -> variable qu'on utilisera lors de la validation des modifications avec envoie BDD
            listPiecePrivee = listPiecePrivee.Where(piece => !accountBatimentModifierForPiecePrivee.Any(existingPiece => existingPiece.Name == piece.Name)).ToList();
            List<string> listPieceString = new List<string>();
            foreach (Piece piece in listPiecePrivee)
            {
                listPieceString.Add(piece.Name);
            }
            string selectedPiece = await DisplayActionSheet("Choisissez une pièce privée", "Annuler", null, listPieceString.ToArray());
            if (selectedPiece != null && selectedPiece != "Annuler")
            {
                Piece pieceToAdd = listPiecePrivee.Where(piece => piece.Name == selectedPiece).FirstOrDefault();
                accountBatimentModifierForPiecePrivee.Add(pieceToAdd);

                await DisplayAlert("", "Pièce privée ajoutée à la chambre", "OK");
            }
        

    }
    /// <summary>
    /// Gère la suppression d'un colocataire du bâtiment
    /// </summary>
    private async void SupprColocPiece(object sender, EventArgs e)
    {
        
    }
    /// <summary>
    /// Gère l'ajout d'un colocataire au bâtiment
    /// </summary>
    private async void AjoutColocPiece(object sender, EventArgs e)
    {

        // Pour modif pièce, mettre des chambres dans la pièces que t'edit
    }
    private async void ValiderModification_Clicked(object sender, EventArgs e)
    {
        AccountDAO accountDAO = new AccountDAO();

        //Si l'objet qu'on récupère et qu'on doit modifier est une chambre...
        if(chambreToModif != null)
        {
            PieceDAO pieceDAO = new PieceDAO();
            if (ancienLocataire != null)
            {
                //S'il y avait un ancien locataire, on le supprime de la chambre et on met a jour son profil
                ancienLocataire.Chambre = null;
                await accountDAO.EditAccount(ancienLocataire);
            }
            if(accountBatimentModifierForPiecePrivee.Count > 0)
            {
                
                Piece piecePrivee = accountBatimentModifierForPiecePrivee[0];
                piecePrivee.Chambres = new List<Chambre>();
                piecePrivee.Chambres.Add(chambreToModif);
                await pieceDAO.EditPiece(piecePrivee);
            }
            if(piecesRemoved.Count > 0)
            {
                foreach(Piece piece in piecesRemoved)
                {
                    piece.Chambres = null;
                    await pieceDAO.EditPiece(piece);
                }
            }
            if (accountBatimentModifierForChambre.Count > 0)
            {
                //On prend le locataire, on modifie ses valeurs en fonction de la chambre et on l'ajoute à la chambre
                Account account = accountBatimentModifierForChambre[0];
                account.Chambre = chambreToModif;
                chambreToModif.Locataire = account;
                
                await accountDAO.EditAccount(account);
                
                
            }
            if (pieceToModif != null)
            {
                await pieceDAO.EditPiece(pieceToModif);
            }
            else if (chambreToModif != null)
            {
                await chambreDAO.EditChambre(chambreToModif);
            }
            DisplayAlert("", "Modification effectué !", "OK");
            await Navigation.PopAsync();
        }
    }
}