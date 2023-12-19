using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using Microsoft.Maui.Controls;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Repr�sente la page de gestion des b�timents
    /// </summary>
    public partial class PageGestionBatiments : ContentPage
    {
        Microsoft.Maui.Controls.Grid grid;
        Label selectedLabel;
        BatimentDAO batimentDAO = new BatimentDAO();

        /// <summary>
        /// Obtient ou d�finit la grille utilis�e pour organiser et afficher les b�timents
        /// </summary>
        public Grid Grid { get => grid; set => grid = value; }

        /// <summary>
        /// Obtient ou d�finit le label s�lectionn� repr�sentant le b�timent actuellement s�lectionn�
        /// </summary>
        public Label SelectedLabel { get => selectedLabel; set => selectedLabel = value; }

        int currentRowCount;

        Batiment batiment;

        /// <summary>
        /// Initialise une nouvelle instance de la classe PageGestionBatiments
        /// </summary>
        public PageGestionBatiments()
        {
            // D�finit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            //initialise une grille principale
            grid = new Microsoft.Maui.Controls.Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };

            ListBatiment();
            //initialise des scrollview et des stacklayout

            ScrollView scrollView = new ScrollView
            {
                Content = xamlGrid
            };
            ScrollView scrollView1 = new ScrollView
            {
                Content = grid
            };

            StackLayout stackLayout = new StackLayout
            {
                Children = { scrollView, scrollView1 }
            };

            Content = stackLayout;
        }

        /// <summary>
        /// G�re l'�v�nement du bouton de retour
        /// </summary>
        private async void RetourButton_Clicked(object sender, System.EventArgs e)
        {
            // Retourne � la page pr�c�dente
            await Navigation.PopAsync();
        }

        /// <summary>
        /// �v�nement du bouton de cr�ation pour envoyer sur la page de cr�ation de b�timent
        /// </summary>
        private async void BoutonCreation_Clicked(object sender, System.EventArgs e)
        {
            // Pousse une nouvelle page de cr�ation de b�timent
            await Navigation.PushAsync(new PageCreationBatiment(this));
        }

        /// <summary>
        /// Liste tous les b�timents dans la grille
        /// </summary>
        public async void ListBatiment()
        {
            this.grid.Children.Clear();
            this.grid.RowDefinitions.Clear();
            currentRowCount = 0;

            var batiments = await batimentDAO.RecupAllBatimentBDD();
            List<Batiment> listbatiment = batimentDAO.ConvertDataTableToList(batiments);

            // Ajoute des d�finitions de ligne en fonction du nombre de b�timents
            for (int i = 0; i < listbatiment.Count; i++)
            {
                grid.AddRowDefinition(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Auto)
                });
            }

            // Ajoute chaque b�timent � la grille avec des labels et des gestes
            foreach (Batiment batiment in listbatiment)
            {
                Label label = new Label { VerticalOptions = LayoutOptions.Center, Text = batiment.Name, Margin = 14, FontSize = 21 };
                grid.Add(label, 2, currentRowCount);
                grid.ColumnDefinitions[2].Width = new GridLength(2, GridUnitType.Auto);
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

        /// <summary>
        /// Ex�cute l'action associ�e � un appui long sur un b�timent
        /// </summary>
        public async void ExecuteLongPress()
        {
            List<string> stringsToDo = new List<string>()
            {
                "Voir en d�tail",
                "Supprimer"
            };
            string selectedTaskName = await DisplayActionSheet("S�lectionnez une action � faire", "Annuler", null, stringsToDo.ToArray());
            string selectedBatimentName = selectedLabel?.Text;

            // V�rifie si l'utilisateur a s�lectionn� un �l�ment autre que "Annuler"
            if (selectedTaskName != "Annuler")
            {
                // V�rifie si l'utilisateur a choisi "Supprimer"
                if (selectedTaskName == "Supprimer")
                {
                    // Continue avec la suppression seulement si "Supprimer" a �t� choisi
                    if (selectedBatimentName != null)
                    {
                        try
                        {
                            batiment = batimentDAO.FindBatiment(selectedBatimentName).Result;
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                        if (batiment != null)
                        {
                            try
                            {
                                if (await batimentDAO.DeleteBatiment(batiment))
                                {
                                    await DisplayAlert("", "B�timent supprim� !", "OK");

                                    // Actualise la Grid en retirant le Label correspondant
                                    int rowIndex = grid.Children.IndexOf(selectedLabel);
                                    if (rowIndex != -1)
                                    {
                                        AccountDAO accountDAO = new AccountDAO();

                                        // R�initialise le lien des comptes par rapport au b�timent
                                        foreach (Account account in batiment.Accounts)
                                        {
                                            //account.Batiment = null;
                                            await accountDAO.EditAccount(account);
                                        }

                                        // Supprime les comptes li�s dans le b�timent
                                        batiment.Accounts.Clear();
                                        await batimentDAO.EditBatiment(batiment);

                                        grid.Children.RemoveAt(rowIndex);
                                        grid.RowDefinitions.RemoveAt(rowIndex);
                                        ListBatiment();
                                    }
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
                    batiment = batimentDAO.FindBatiment(selectedBatimentName).Result;
                    await Navigation.PushAsync(new PageEditionBatiment(this, batiment));
                }
            }
        }
    }
}
