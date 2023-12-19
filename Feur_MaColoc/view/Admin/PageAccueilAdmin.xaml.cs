using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using System.Numerics;

namespace Feur_MaColoc.view.Admin
{
    /// <summary>
    /// Page d'accueil pour un admin
    /// </summary>
    public partial class PageAccueilAdmin : ContentPage
    {
        /// <summary>
        /// Initialise une nouvelle instance de la PageAccueilAdmin
        /// </summary>
        public PageAccueilAdmin()
        {
            // Définit la couleur de fond de la page
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
        }

        /// <summary>
        /// événement de click de bouton qui envoie à la page de gestion des taches
        /// </summary>
        private async void MesTaches_Clicked(object sender, System.EventArgs e)
        {
            // Navigue vers la page de gestion des tâches
            await Navigation.PushAsync(new PageGestionTaches());
        }

        /// <summary>
        /// événement de click de bouton qui envoie à la page de listing des utilisateurs
        /// </summary>
        private async void ListUsersButton_Clicked(object sender, EventArgs e)
        {
            // Navigue vers la page de liste des utilisateurs
            await Navigation.PushAsync(new ListUsers());
        }

        /// <summary>
        /// événement de click de bouton qui envoie à la page de gestion des bâtiments
        /// </summary>
        private async void MesBatiments_Clicked(object sender, EventArgs e)
        {
            // Navigue vers la page de gestion des bâtiments
            await Navigation.PushAsync(new PageGestionBatiments());
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton de retour
        /// </summary>
        private async void RetourButton_Clicked(object sender, System.EventArgs e)
        {
            // Retourne à la page précédente
            await Navigation.PopToRootAsync();
        }
    }
}
