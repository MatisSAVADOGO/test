using Feur_MaColoc.view.Users;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Feur_MaColoc.view.Users
{
    public partial class PageAffichageTachesFinies : ContentPage
    {
        private Dictionary<DateTime, List<Tache>> tachesParDate;
        private Grid gridTaches;
        private Grid headerGrid;
        private DatePicker datePicker;

        public PageAffichageTachesFinies(Dictionary<DateTime, List<Tache>> tachesParDate)
        {
            InitializeComponent();
            this.tachesParDate = tachesParDate;
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));

            headerGrid = new Microsoft.Maui.Controls.Grid
            {
                Margin = new Thickness(40, 30, 40, 0),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var labelChoisirJour = new Label
            {
                Text = "Choisir un jour :",
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            headerGrid.Add(labelChoisirJour, 0, 1);

            datePicker = new DatePicker
            {
                Format = "dd/MM/yyyy",
                MinimumDate = new DateTime(2023, 1, 1), 
                MaximumDate = DateTime.Today,
                Date = DateTime.Today
            };
            datePicker.DateSelected += DatePicker_DateSelected;
            headerGrid.Add(datePicker, 1, 1);
            Grid.SetColumnSpan(datePicker, 2);

            var BoutonRetour = new Button
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
            BoutonRetour.Clicked += Bouton_Home_Clicked;
            headerGrid.Add(BoutonRetour, 2, 0);

            var LabelTitre = new Label
            {
                Text = "Mes Tâches validées aujourd'hui",
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 30, 0, 10),
            };
            headerGrid.Add(LabelTitre, 1, 2);
            Grid.SetColumnSpan(LabelTitre, 2);

            gridTaches = new Grid
            {
                RowSpacing = 10,
                BackgroundColor = Colors.White,
                Margin = new Thickness(20, 5),
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            AfficherTaches(datePicker.Date);
            var scrollViewer = new ScrollView { Content = gridTaches };
            var stackLayout = new StackLayout
            {
                Children = { headerGrid, scrollViewer }
            };

            this.Content = stackLayout;
        }

        private async void Bouton_Home_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void AfficherTaches(DateTime date)
        {
            gridTaches.Children.Clear();
            gridTaches.RowDefinitions.Clear();
            if (tachesParDate.ContainsKey(date))
            {
                int row = 0;
                foreach (var tache in tachesParDate[date])
                {
                    gridTaches.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    var labelTache = new Label
                    {
                        Text = tache.Description,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 18
                    };
                    gridTaches.Add(labelTache, 0, row);

                    var labelPiece = new Label
                    {
                        Text = tache.Piece,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 18
                    };
                    gridTaches.Add(labelPiece, 1, row);

                    row++;
                }
            }
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            AfficherTaches(e.NewDate);
        }
    }
}
