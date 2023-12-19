using System;
using System.Collections.Generic;
using System.Linq;
using Feur_MaColoc_Metier;
using Microsoft.Maui.Controls;

namespace Feur_MaColoc.view.Users
{
    public partial class PageAccueilUser : ContentPage
    {
        private Microsoft.Maui.Controls.Grid grid;
        private Dictionary<DateTime, List<Tache>> tachesParDate = new Dictionary<DateTime, List<Tache>>();
        private Account account;
        public PageAccueilUser(Account account)
        {
            InitializeComponent();
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            this.account = account;
            var headerGrid = new Microsoft.Maui.Controls.Grid
            {
                Margin = new Thickness(40, 0, 40, 0),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var mainGrid = new Microsoft.Maui.Controls.Grid
            {
                Margin = new Thickness(00, 30, 60, 30),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var BoutonInfoUser = new Button
            {
                Text = "Mes Informations",
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 30,
                TextColor = Colors.White,
                BorderColor = Color.FromArgb("#3860B2"),
                BackgroundColor = Color.FromArgb("#3860B2"),
                HeightRequest = 60,
                Margin = new Thickness(10),
                HorizontalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };
            BoutonInfoUser.Clicked += BoutonInfoUser_Cliked ;
            headerGrid.Add(BoutonInfoUser, 0, 0);

            var BoutonTachesValide = new Button
            {
                Text = "Mes tâches validées",
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 30,
                TextColor = Colors.White,
                BorderColor = Color.FromArgb("#3860B2"),
                BackgroundColor = Color.FromArgb("#3860B2"),
                HeightRequest = 60,
                Margin = new Thickness(10),
                HorizontalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };
            BoutonTachesValide.Clicked += BoutonTacheValide_Clicked;
            headerGrid.Add(BoutonTachesValide, 2, 0);

            var LabelTitre = new Label
            {
                Text = "Mes Tâches du jour",
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 15, 0, 10),
            };
            mainGrid.Add(LabelTitre, 1, 0);
            Grid.SetColumnSpan(LabelTitre, 3);

            grid = new Microsoft.Maui.Controls.Grid
            {
                Padding = new Thickness(25, 0, 25, 0),
                ColumnSpacing = 10,
                RowSpacing = 10,
                RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            AddTaches();

            var scrollView = new ScrollView { Content = grid };
            var stackLayout = new StackLayout
            {
                Children = { headerGrid,mainGrid, scrollView }
            };

            Content = stackLayout;
        }

        private void AddTaches()
        {
            for (int i = 1; i <= 3; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                /*
                var checkBox = new CheckBox { VerticalOptions = LayoutOptions.Center };
                checkBox.CheckedChanged += CheckBox_CheckedChanged;
                grid.Add(checkBox, 0, i);

                var pieceLabel = new Label
                {
                    Text = $"Pièce {i}", 
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                };
                var tacheLabel = new Label
                {
                    Text = $"Tâche {i}",
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                };
                */

                var checkBox1 = new CheckBox { VerticalOptions = LayoutOptions.Center };
                checkBox1.CheckedChanged += CheckBox_CheckedChanged;

                var checkBox2 = new CheckBox { VerticalOptions = LayoutOptions.Center };
                checkBox2.CheckedChanged += CheckBox_CheckedChanged;

                var checkBox3 = new CheckBox { VerticalOptions = LayoutOptions.Center };
                checkBox3.CheckedChanged += CheckBox_CheckedChanged;

                var piece1 = new Label
                {
                    Text = "Salon",
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                };
                var piece2 = new Label
                {
                    Text = "Chambre",
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                };
                var piece3 = new Label
                {
                    Text = "SDB",
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                };
                var tache1 = new Label
                {
                    Text = "Aspirer sol",
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                }; 
                var tache2 = new Label
                {
                    Text = "Vider poubelle",
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                }; 
                var tache3 = new Label
                {
                    Text = "nettoyer mirroir",
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                };
                grid.Add(piece1, 1, 1);
                grid.Add(piece2, 1, 2);
                grid.Add(piece3, 1, 3);
                grid.Add(tache1, 2, 1);
                grid.Add(tache2, 2, 2);
                grid.Add(tache3, 2, 3);
                grid.Add(checkBox1, 0, i);
                grid.Add(checkBox2, 0, i);
                grid.Add(checkBox3, 0, i);

                checkBox1.BindingContext = new Tache { Description = tache1.Text, Piece = piece1.Text };
                checkBox2.BindingContext = new Tache { Description = tache2.Text, Piece = piece2.Text };
                checkBox3.BindingContext = new Tache { Description = tache3.Text, Piece = piece3.Text };

                /*
                grid.Add(pieceLabel, 1, i);
                grid.Add(tacheLabel, 2, i);
                
                checkBox.BindingContext = new Tache { Description = $"Tâche {i}" };
                */
            }
        }


        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                var checkBox = (CheckBox)sender;
                var tache = (Tache)checkBox.BindingContext;

                // Trouvez le label de la pièce correspondant
                var rowIndex = grid.GetRow(checkBox);
                var pieceLabel = grid.Children.OfType<Label>()
                                    .FirstOrDefault(c => grid.GetRow(c) == rowIndex && c.Text != tache.Description);

                if (pieceLabel != null)
                {
                    tache.Piece = pieceLabel.Text; // Assignez le nom de la pièce à la tâche
                }

                bool answer = await DisplayAlert("Confirmation", $"Valider la tâche {tache.Description}?", "Oui", "Non");
                if (answer)
                {
                    tache.DateValidation = DateTime.Today;
                    if (!tachesParDate.ContainsKey(tache.DateValidation))
                    {
                        tachesParDate[tache.DateValidation] = new List<Tache>();
                    }
                    tachesParDate[tache.DateValidation].Add(tache);

                    // Supprimez les éléments de la ligne
                    var childrenToRemove = grid.Children.Where(c => grid.GetRow(c) == rowIndex).ToList();
                    foreach (var child in childrenToRemove)
                    {
                        grid.Children.Remove(child);
                    }

                    // Mettez à jour les RowIndex des éléments restants
                    foreach (var child in grid.Children)
                    {
                        var currentRow = grid.GetRow(child);
                        if (currentRow > rowIndex)
                        {
                            grid.SetRow(child, currentRow - 1);
                        }
                    }
                    grid.RowDefinitions.RemoveAt(rowIndex);
                }
                else
                {
                    checkBox.IsChecked = false;
                }
            }
        }

        /*
        private void ReorganiserTaches()
        {
            // Obtenez toutes les checkboxes et labels restants
            var allCheckBoxes = grid.Children.OfType<CheckBox>().ToList();
            var allLabels = grid.Children.OfType<Label>().ToList();

            // Nettoyez la grille avant de la reconstruire
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            int rowCount = 0;

            // Pour chaque checkbox, trouvez le label correspondant et ajoutez-les à la grille
            foreach (var checkBox in allCheckBoxes)
            {
                ((Layout)checkBox.Parent)?.Children.Remove(checkBox);
                // Ajoutez une nouvelle ligne pour chaque tâche
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // Trouvez les labels correspondants à cette checkbox
                var labelsForCheckBox = allLabels.Where(label => grid.GetRow(label) == grid.GetRow(checkBox)).ToList();

                // Ajoutez la checkbox et ses labels à la nouvelle ligne
                grid.Add(checkBox, 0, rowCount);
                int colCount = 1;
                foreach (var label in labelsForCheckBox)
                {
                    ((Layout)label.Parent)?.Children.Remove(label);
                    grid.Add(label, colCount, rowCount);
                    colCount++;
                }

                rowCount++;
            }
        }
        */


        private async void BoutonTacheValide_Clicked(object sender, System.EventArgs e)
        {
            var pageTachesFinies = new PageAffichageTachesFinies(tachesParDate);
            await Navigation.PushAsync(pageTachesFinies);
        }

        private async void BoutonInfoUser_Cliked(object sender, System.EventArgs e)
        {
            if(account != null)
            {
                Navigation.PushAsync(new PageInformationUser(account));
            }
        }
    }

    public class Tache
    {
        public string Description { get; set; }
        public DateTime DateValidation { get; set; }
        public string Piece { get; set; }
    }
}
