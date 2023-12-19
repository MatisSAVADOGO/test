using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Représente la page de création et de suppression des types de tâches
    /// </summary>
    public partial class TaskCreationPage : ContentPage
    {
        private string nameTask;
        private string repetitionTask;
        private int duration_task;
        private TaskTypeDAO taskTypeDAO;
        private string tacheCreation;
        private string tacheSuppression;
        PageGestionTaches gestionTask;
        /// <summary>
        /// Initialise une nouvelle instance de la classe TaskCreationPage
        /// </summary>
        public TaskCreationPage(PageGestionTaches GestionTache)
        {
            // Définit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            taskTypeDAO = new TaskTypeDAO();
            gestionTask = GestionTache;
            RepetitionPicker.SelectedIndexChanged += RepetitionPicker_SelectedIndexChanged;
            RepetitionPicker.SelectedItem = "Journalier";
        }

        /// <summary>
        /// Méthode qui récupère la durée de la tâche et la convertit de string à integer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string duration_text = Duration.Text;
            int.TryParse(duration_text, out duration_task);
        }

        /// <summary>
        /// Bouton pour créer la tâche en fonction des valeurs rentrées précédemment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NameNewTaskEntry.Text))
                if (!string.IsNullOrEmpty(Duration.Text)) // pour gérer si une durée a bien été rentrée
                {
                    if (duration_task < 0)
                    {
                        await DisplayAlert("Erreur", "La durée doit être positive", "OK"); // gère l'erreur pour un nombre négatif
                    }
                    else if (duration_task % 1 != 0)
                    {
                        await DisplayAlert("Erreur", "La durée doit être un entier", "OK");
                    }
                    else
                    {
                        TaskType TaskDone = new TaskType(nameTask, duration_task, repetitionTask);

                        await taskTypeDAO.CreateTaskType(TaskDone); // insertion de la tâche dans la base de données

                        Duration.Text = ""; // remise à 0 de la durée pour une future tâche

                        await DisplayAlert("", tacheCreation, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("", "Vous n'avez pas défini de durée", "OK");
                }
            else
            {
                await DisplayAlert("", "Vous n'avez pas défini de nom", "OK");
            }

        }

        /// <summary>
        /// Méthode appelée lors de la modification du nom de la tâche
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryTaskName(object sender, TextChangedEventArgs e)
        {
            nameTask = NameNewTaskEntry.Text;
            tacheCreation = "Tâche créée : " + nameTask; // permet d'afficher le nom de la tache dans la popup
            tacheSuppression = "Tâche supprimée : " + nameTask; // permet d'afficher le nom de la tache dans la popup
        }

        /// <summary>
        /// Bouton pour supprimer une tâche existante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonDeleteTask(object sender, EventArgs e)
        {
            var dataTable_Tasklist = await taskTypeDAO.getAllTaskType();
            List<TaskType> TaskList = taskTypeDAO.ConvertDataTableToList(dataTable_Tasklist);
            List<string> nameOfTaskType = new List<string>();
            foreach (TaskType task in TaskList)
            {
                nameOfTaskType.Add(task.Name);
            }

            Picker picker = new Picker
            {
                Title = "Choisissez une tâche à supprimer",
                ItemsSource = TaskList
            };
            // on affiche les taches dans un display
            string selectedTaskName = await DisplayActionSheet("Sélectionnez une tâche à supprimer", "Annuler", null, nameOfTaskType.ToArray());

            // on vérifie que la tâche choisie n'est pas null et n'est pas annulée
            if (selectedTaskName != null && selectedTaskName != "Annuler")
            {
                TaskType taskToDelete;
                // choix de la tache à supprimer parmi la liste des taches pris en bdd
                try
                {
                    taskToDelete = taskTypeDAO.FindTaskType(selectedTaskName).Result;
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
                if (taskToDelete != null)
                {
                    try
                    {
                        if (await taskTypeDAO.DeleteTaskType(taskToDelete))
                        {
                            await DisplayAlert("", "Tâche supprimée", "OK");
                            TaskList.Remove(taskToDelete); // mettre à jour la liste des taches
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

        private void RepetitionPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedRepetition = RepetitionPicker.SelectedItem as string;

            switch (selectedRepetition)
            {
                case "Journalier": repetitionTask = "Journalier";
                    break;
                case "Hebdomadaire": repetitionTask = "Hebdomadaire";
                    break;
                case "Mensuel": repetitionTask = "Mensuel";
                    break;
                default:
                    break;
            }
        }


            /// <summary>
            /// Gère le clic sur le bouton de retour
            /// </summary>
            private async void RetourButton_Clicked(object sender, System.EventArgs e)
        {
            // Retourne à la page précédente
            await Navigation.PopAsync();
        }
    }
}
