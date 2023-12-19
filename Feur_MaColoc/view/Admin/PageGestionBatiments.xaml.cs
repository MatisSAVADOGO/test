using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using Microsoft.Maui.Controls;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Représente la page de gestion des bâtiments
    /// </summary>
    public partial class PageGestionBatiments : ContentPage
    {
        Microsoft.Maui.Controls.Grid grid;
        Label selectedLabel;
        BatimentDAO batimentDAO = new BatimentDAO();

        /// <summary>
        /// Obtient ou définit la grille utilisée pour organiser et afficher les bâtiments
        /// </summary>
        public Grid Grid { get => grid; set => grid = value; }

        /// <summary>
        /// Obtient ou définit le label sélectionné représentant le bâtiment actuellement sélectionné
        /// </summary>
        public Label SelectedLabel { get => selectedLabel; set => selectedLabel = value; }

        int currentRowCount;

        Batiment batiment;

        /// <summary>
        /// Initialise une nouvelle instance de la classe PageGestionBatiments
        /// </summary>
        public PageGestionBatiments()
        {
            // Définit la couleur de fond de la page
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
        /// Gère l'événement du bouton de retour
        /// </summary>
        private async void RetourButton_Clicked(object sender, System.EventArgs e)
        {
            // Retourne à la page précédente
            await Navigation.PopAsync();
        }

        /// <summary>
        /// événement du bouton de création pour envoyer sur la page de création de bâtiment
        /// </summary>
        private async void BoutonCreation_Clicked(object sender, System.EventArgs e)
        {
            // Pousse une nouvelle page de création de bâtiment
            await Navigation.PushAsync(new PageCreationBatiment(this));
        }

        /// <summary>
        /// Liste tous les bâtiments dans la grille
        /// </summary>
        public async void ListBatiment()
        {
            this.grid.Children.Clear();
            this.grid.RowDefinitions.Clear();
            currentRowCount = 0;

            var batiments = await batimentDAO.RecupAllBatimentBDD();
            List<Batiment> listbatiment = batimentDAO.ConvertDataTableToList(batiments);

            // Ajoute des définitions de ligne en fonction du nombre de bâtiments
            for (int i = 0; i < listbatiment.Count; i++)
            {
                grid.AddRowDefinition(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Auto)
                });
            }

            // Ajoute chaque bâtiment à la grille avec des labels et des gestes
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
        /// Exécute l'action associée à un appui long sur un bâtiment
        /// </summary>
        public async void ExecuteLongPress()
        {
            List<string> stringsToDo = new List<string>()
            {
                "Voir en détail",
                "Supprimer"
            };
            string selectedTaskName = await DisplayActionSheet("Sélectionnez une action à faire", "Annuler", null, stringsToDo.ToArray());
            string selectedBatimentName = selectedLabel?.Text;

            // Vérifie si l'utilisateur a sélectionné un élément autre que "Annuler"
            if (selectedTaskName != "Annuler")
            {
                // Vérifie si l'utilisateur a choisi "Supprimer"
                if (selectedTaskName == "Supprimer")
                {
                    // Continue avec la suppression seulement si "Supprimer" a été choisi
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
                                    await DisplayAlert("", "Bâtiment supprimé !", "OK");

                                    // Actualise la Grid en retirant le Label correspondant
                                    int rowIndex = grid.Children.IndexOf(selectedLabel);
                                    if (rowIndex != -1)
                                    {
                                        AccountDAO accountDAO = new AccountDAO();

                                        // Réinitialise le lien des comptes par rapport au bâtiment
                                        foreach (Account account in batiment.Accounts)
                                        {
                                            //account.Batiment = null;
                                            await accountDAO.EditAccount(account);
                                        }

                                        // Supprime les comptes liés dans le bâtiment
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
                //permet d'aller sur la page d'édition si voir en détail a été choisi
                else if (selectedTaskName == "Voir en détail")
                {
                    batiment = batimentDAO.FindBatiment(selectedBatimentName).Result;
                    await Navigation.PushAsync(new PageEditionBatiment(this, batiment));
                }
            }
        }
    }
}
