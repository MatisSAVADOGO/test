using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using Microsoft.Maui.Controls;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Représente la page de gestion des tâches
    /// </summary>
    public partial class PageGestionTaches : ContentPage
    {
        Microsoft.Maui.Controls.Grid grid;
        Label selectedLabel;
        TaskTypeDAO taskTypeDAO = new TaskTypeDAO();

        /// <summary>
        /// Obtient ou définit le label sélectionné représentant le tâche actuellement sélectionné
        /// </summary>
        public Label SelectedLabel { get => selectedLabel; set => selectedLabel = value; }

        int currentRowCount;

        TaskType task;

        /// <summary>
        /// Initialise une nouvelle instance de la classe PageGestionTaches
        /// </summary>
        public PageGestionTaches()
        {
            // Définit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            taskTypeDAO = new TaskTypeDAO();

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
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
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

            // Label "Mes tâches"
            var MesTachesLabel = new Label
            {
                Text = "Mes tâches",
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 60)
            };
            headerGrid.Add(MesTachesLabel, 0, 2);
            Grid.SetColumnSpan(MesTachesLabel, 3);

            // Initialise la grille principale
            grid = new Microsoft.Maui.Controls.Grid
            {
                Padding = new Thickness(45, 0, 45, 45),
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

            var enTeteGrid = new Microsoft.Maui.Controls.Grid
            {
                Margin = new Thickness(45, 0, 45, 45),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }

                }
            };

            // Labels pour les en-têtes de colonne
            var taskNameHeader = new Label
            {
                Text = "Nom de la tâche",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 16,
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
            };
            enTeteGrid.Add(taskNameHeader, 0, 0);

            var taskDurationHeader = new Label
            {
                Text = "Durée de la tâche",
                HorizontalOptions = LayoutOptions.End,
                FontSize = 16,
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
            };
            enTeteGrid.Add(taskDurationHeader, 1, 0);

            var taskRepetitionHeader = new Label
            {
                Text = "Répétition de la tâche",
                HorizontalOptions = LayoutOptions.End,
                FontSize = 16,
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
            };
            enTeteGrid.Add(taskRepetitionHeader, 2, 0);



            // Remplit la grille avec la liste des tâches
            ListTache();

            // Crée une ScrollView contenant la grille principale
            var scrollView = new ScrollView { Content = grid };

            // Crée un StackLayout pour organiser les éléments de la page
            var stackLayout = new StackLayout
            {
                Children = { headerGrid, enTeteGrid, scrollView,  ButtonNewTask }
            };

            // Définit le contenu de la page comme le StackLayout créé
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
            // Pousse une nouvelle page de création de tâche
            await Navigation.PushAsync(new TaskCreationPage(this));
        }

        /// <summary>
        /// Liste tous les tâches dans la grille
        /// </summary>
        public async void ListTache()
        {
            this.grid.Children.Clear();
            this.grid.RowDefinitions.Clear();
            

            // Récupère la liste des tâches depuis la base de données
            var tasks = await taskTypeDAO.getAllTaskType();
            List<TaskType> TaskList = taskTypeDAO.ConvertDataTableToList(tasks);

            // Ajoute des définitions de ligne en fonction du nombre de tâches
            for (int i = 0; i < TaskList.Count; i++)
            {
                grid.AddRowDefinition(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Auto)
                });
            }
            int currentRowCount = 0;
            // Ajoute chaque tâche à la grille avec des labels et des gestes
            foreach (TaskType task in TaskList)
            {
                Label label = new Label { HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, Text = task.Name };
                Label label2 = new Label { HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, Text = task.Duration.ToString() };
                Label label3 = new Label { HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, Text = task.Repetition }; 
                grid.Add(label, 0, currentRowCount);
                grid.Add(label2, 1, currentRowCount);
                grid.Add(label3, 2, currentRowCount); 

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
        /// Exécute l'action associée à un appui long sur un tâche
        /// </summary>
        public async void ExecuteLongPress()
        {
            List<string> stringsToDo = new List<string>()
            {
                "Voir en détail",
                "Supprimer"
            };
            string selectedTaskName = await DisplayActionSheet("Sélectionnez une action à faire", "Annuler", null, stringsToDo.ToArray());
            string selectedTacheName = selectedLabel?.Text;

            // Vérifie si l'utilisateur a sélectionné un élément autre que "Annuler"
            if (selectedTaskName != "Annuler")
            {
                // Vérifie si l'utilisateur a choisi "Supprimer"
                if (selectedTaskName == "Supprimer")
                {
                    // Continue avec la suppression seulement si "Supprimer" a été choisi
                    if (selectedTacheName != null)
                    {
                        try
                        {
                            task = taskTypeDAO.FindTaskType(selectedTacheName).Result;
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                        if (task != null)
                        {
                            try
                            {
                                if (await taskTypeDAO.DeleteTaskType(task))
                                {
                                    await DisplayAlert("", "tâche supprimé !", "OK");

                                    // Actualise la Grid en retirant le Label correspondant
                                    int rowIndex = grid.Children.IndexOf(selectedLabel);
                                    if (rowIndex != -1)
                                    {
                                        //A MODIFIER 
                                        //Pour supprimer le lien entre la piece et la tache quand elle est supprimé 

                                        /*AccountDAO accountDAO = new AccountDAO();

                                        // Réinitialise le lien des comptes par rapport à la tâche
                                        foreach (Account account in task.Accounts)
                                        {
                                            //account.Batiment = null;
                                            await accountDAO.EditAccount(account);
                                        }

                                        // Supprime les comptes liés dans la tâche
                                        task.Accounts.Clear();
                                        await taskTypeDAO.EditBatiment(task);*/ 

                                        grid.Children.RemoveAt(rowIndex);
                                        grid.RowDefinitions.RemoveAt(rowIndex);
                                        ListTache();
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
                /*else if (selectedTaskName == "Voir en détail")
                {
                    task = taskTypeDAO.FindTaskType(selectedTacheName).Result;
                    await Navigation.PushAsync(new PageEditionBatiment(this, task));
                }*/
            }
        }
    }
}
