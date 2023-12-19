using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Repr�sente la page de cr�ation et de suppression des types de t�ches
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
            // D�finit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            taskTypeDAO = new TaskTypeDAO();
            gestionTask = GestionTache;
            RepetitionPicker.SelectedIndexChanged += RepetitionPicker_SelectedIndexChanged;
            RepetitionPicker.SelectedItem = "Journalier";
        }

        /// <summary>
        /// M�thode qui r�cup�re la dur�e de la t�che et la convertit de string � integer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string duration_text = Duration.Text;
            int.TryParse(duration_text, out duration_task);
        }

        /// <summary>
        /// Bouton pour cr�er la t�che en fonction des valeurs rentr�es pr�c�demment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NameNewTaskEntry.Text))
                if (!string.IsNullOrEmpty(Duration.Text)) // pour g�rer si une dur�e a bien �t� rentr�e
                {
                    if (duration_task < 0)
                    {
                        await DisplayAlert("Erreur", "La dur�e doit �tre positive", "OK"); // g�re l'erreur pour un nombre n�gatif
                    }
                    else if (duration_task % 1 != 0)
                    {
                        await DisplayAlert("Erreur", "La dur�e doit �tre un entier", "OK");
                    }
                    else
                    {
                        TaskType TaskDone = new TaskType(nameTask, duration_task, repetitionTask);

                        await taskTypeDAO.CreateTaskType(TaskDone); // insertion de la t�che dans la base de donn�es

                        Duration.Text = ""; // remise � 0 de la dur�e pour une future t�che

                        await DisplayAlert("", tacheCreation, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("", "Vous n'avez pas d�fini de dur�e", "OK");
                }
            else
            {
                await DisplayAlert("", "Vous n'avez pas d�fini de nom", "OK");
            }

        }

        /// <summary>
        /// M�thode appel�e lors de la modification du nom de la t�che
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryTaskName(object sender, TextChangedEventArgs e)
        {
            nameTask = NameNewTaskEntry.Text;
            tacheCreation = "T�che cr��e : " + nameTask; // permet d'afficher le nom de la tache dans la popup
            tacheSuppression = "T�che supprim�e : " + nameTask; // permet d'afficher le nom de la tache dans la popup
        }

        /// <summary>
        /// Bouton pour supprimer une t�che existante
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
                Title = "Choisissez une t�che � supprimer",
                ItemsSource = TaskList
            };
            // on affiche les taches dans un display
            string selectedTaskName = await DisplayActionSheet("S�lectionnez une t�che � supprimer", "Annuler", null, nameOfTaskType.ToArray());

            // on v�rifie que la t�che choisie n'est pas null et n'est pas annul�e
            if (selectedTaskName != null && selectedTaskName != "Annuler")
            {
                TaskType taskToDelete;
                // choix de la tache � supprimer parmi la liste des taches pris en bdd
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
                            await DisplayAlert("", "T�che supprim�e", "OK");
                            TaskList.Remove(taskToDelete); // mettre � jour la liste des taches
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
            /// G�re le clic sur le bouton de retour
            /// </summary>
            private async void RetourButton_Clicked(object sender, System.EventArgs e)
        {
            // Retourne � la page pr�c�dente
            await Navigation.PopAsync();
        }
    }
}
