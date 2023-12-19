using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using Microsoft.Maui.Controls;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Repr�sente la page de gestion des t�ches
    /// </summary>
    public partial class PageGestionTaches : ContentPage
    {
        Microsoft.Maui.Controls.Grid grid;
        Label selectedLabel;
        TaskTypeDAO taskTypeDAO = new TaskTypeDAO();

        /// <summary>
        /// Obtient ou d�finit le label s�lectionn� repr�sentant le t�che actuellement s�lectionn�
        /// </summary>
        public Label SelectedLabel { get => selectedLabel; set => selectedLabel = value; }

        int currentRowCount;

        TaskType task;

        /// <summary>
        /// Initialise une nouvelle instance de la classe PageGestionTaches
        /// </summary>
        public PageGestionTaches()
        {
            // D�finit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            taskTypeDAO = new TaskTypeDAO();

            // Cr�e l'en-t�te de la page
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

            // Label "Mes t�ches"
            var MesTachesLabel = new Label
            {
                Text = "Mes t�ches",
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

            // Labels pour les en-t�tes de colonne
            var taskNameHeader = new Label
            {
                Text = "Nom de la t�che",
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
                Text = "Dur�e de la t�che",
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
                Text = "R�p�tition de la t�che",
                HorizontalOptions = LayoutOptions.End,
                FontSize = 16,
                FontFamily = "OpenSans",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
            };
            enTeteGrid.Add(taskRepetitionHeader, 2, 0);



            // Remplit la grille avec la liste des t�ches
            ListTache();

            // Cr�e une ScrollView contenant la grille principale
            var scrollView = new ScrollView { Content = grid };

            // Cr�e un StackLayout pour organiser les �l�ments de la page
            var stackLayout = new StackLayout
            {
                Children = { headerGrid, enTeteGrid, scrollView,  ButtonNewTask }
            };

            // D�finit le contenu de la page comme le StackLayout cr��
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
            // Pousse une nouvelle page de cr�ation de t�che
            await Navigation.PushAsync(new TaskCreationPage(this));
        }

        /// <summary>
        /// Liste tous les t�ches dans la grille
        /// </summary>
        public async void ListTache()
        {
            this.grid.Children.Clear();
            this.grid.RowDefinitions.Clear();
            

            // R�cup�re la liste des t�ches depuis la base de donn�es
            var tasks = await taskTypeDAO.getAllTaskType();
            List<TaskType> TaskList = taskTypeDAO.ConvertDataTableToList(tasks);

            // Ajoute des d�finitions de ligne en fonction du nombre de t�ches
            for (int i = 0; i < TaskList.Count; i++)
            {
                grid.AddRowDefinition(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Auto)
                });
            }
            int currentRowCount = 0;
            // Ajoute chaque t�che � la grille avec des labels et des gestes
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
        /// Ex�cute l'action associ�e � un appui long sur un t�che
        /// </summary>
        public async void ExecuteLongPress()
        {
            List<string> stringsToDo = new List<string>()
            {
                "Voir en d�tail",
                "Supprimer"
            };
            string selectedTaskName = await DisplayActionSheet("S�lectionnez une action � faire", "Annuler", null, stringsToDo.ToArray());
            string selectedTacheName = selectedLabel?.Text;

            // V�rifie si l'utilisateur a s�lectionn� un �l�ment autre que "Annuler"
            if (selectedTaskName != "Annuler")
            {
                // V�rifie si l'utilisateur a choisi "Supprimer"
                if (selectedTaskName == "Supprimer")
                {
                    // Continue avec la suppression seulement si "Supprimer" a �t� choisi
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
                                    await DisplayAlert("", "t�che supprim� !", "OK");

                                    // Actualise la Grid en retirant le Label correspondant
                                    int rowIndex = grid.Children.IndexOf(selectedLabel);
                                    if (rowIndex != -1)
                                    {
                                        //A MODIFIER 
                                        //Pour supprimer le lien entre la piece et la tache quand elle est supprim� 

                                        /*AccountDAO accountDAO = new AccountDAO();

                                        // R�initialise le lien des comptes par rapport � la t�che
                                        foreach (Account account in task.Accounts)
                                        {
                                            //account.Batiment = null;
                                            await accountDAO.EditAccount(account);
                                        }

                                        // Supprime les comptes li�s dans la t�che
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
                //permet d'aller sur la page d'�dition si voir en d�tail a �t� choisi
                /*else if (selectedTaskName == "Voir en d�tail")
                {
                    task = taskTypeDAO.FindTaskType(selectedTacheName).Result;
                    await Navigation.PushAsync(new PageEditionBatiment(this, task));
                }*/
            }
        }
    }
}
