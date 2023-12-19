using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Page pour afficher la liste des locataires
    /// </summary>
    public partial class ListUsers : ContentPage
    {
        Microsoft.Maui.Controls.Grid grid;
        AccountDAO accountDAO = new AccountDAO();

        /// <summary>
        /// Initialise une nouvelle instance de la page ListUsers
        /// </summary>
        public ListUsers()
        {
            // Définit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            accountDAO = new AccountDAO();

            // Crée l'en-tête de la page
            var headerGrid = new Microsoft.Maui.Controls.Grid
            {
                Margin = new Thickness(40, 30, 60, 0),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            // Bouton de retour
            var retourButton = new Button
            {
                Text = "<",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 30,
                TextColor = Colors.White,
                BorderColor = Color.FromArgb("#3860B2"),
                BackgroundColor = Color.FromArgb("#3860B2"),
                WidthRequest = 60,
                HeightRequest = 60
            };
            retourButton.Clicked += RetourButton_Clicked;
            headerGrid.Add(retourButton, 0, 0);

            // Label "MA COLOC"
            var maColocLabel = new Label
            {
                Text = "MA COLOC",
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            headerGrid.Add(maColocLabel, 0, 1);
            Grid.SetColumnSpan(maColocLabel, 3);

            // Label "Mes locataires"
            var MesLocatairesLabel = new Label
            {
                Text = "Mes locataires",
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 60)
            };
            headerGrid.Add(MesLocatairesLabel, 0, 2);
            Grid.SetColumnSpan(MesLocatairesLabel, 3);

            // Initialise la grille principale
            grid = new Microsoft.Maui.Controls.Grid
            {
                Padding = new Thickness(25, 0, 25, 0),
                ColumnSpacing = 10,
                RowSpacing = 10,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            // Labels pour les en-têtes de colonne
            var nomHeader = new Label
            {
                Text = "Nom",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 16,
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            var loginHeader = new Label
            {
                Text = "Login",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 16,
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            var mailHeader = new Label
            {
                Text = "Mail",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 16,
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            // Ajoute les en-têtes à la grille
            grid.Add(nomHeader, 0, 0);
            grid.Add(loginHeader, 1, 0);
            grid.Add(mailHeader, 2, 0);
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });

            // Remplit la grille avec la liste des locataires
            ListAccounts();

            // Crée une ScrollView contenant la grille principale
            var scrollView = new ScrollView { Content = grid };
            var stackLayout = new StackLayout
            {
                Children = { headerGrid, scrollView }
            };

            // Définit le contenu de la page comme le StackLayout créé
            Content = stackLayout;
        }

        /// <summary>
        /// Charge la liste des locataires dans la grille
        /// </summary>
        private async void ListAccounts()
        {
            // Récupère la liste des locataires depuis la base de données
            var accounts = await accountDAO.RecupAllCompteBDD();
            List<Account> AccountList = accountDAO.ConvertDataTableToList(accounts);

            // Initialise l'index de ligne à 2
            int rowIndex = 2;

            // Parcourt la liste des locataires et les ajoute à la grille
            foreach (Account account in AccountList)
            {
                grid.AddRowDefinition(new RowDefinition());
                grid.Add(new Label { HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, Text = account.Username }, 0, rowIndex);
                grid.Add(new Label { HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, Text = account.Login }, 1, rowIndex);
                grid.Add(new Label { HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, Text = account.Mail }, 2, rowIndex);

                rowIndex++;
            }
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton de retour
        /// </summary>
        /// <param name="sender">L'objet déclenchant l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private async void RetourButton_Clicked(object sender, System.EventArgs e)
        {
            // Retourne à la page précédente
            await Navigation.PopAsync();
        }
    }
}
