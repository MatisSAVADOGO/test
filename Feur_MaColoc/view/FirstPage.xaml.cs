using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using System.Numerics;

namespace Feur_MaColoc.view
{

    public partial class FirstPage : ContentPage
    {

        public FirstPage()
        {
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
        }
        /// <summary>
        /// évènement de redirection vers la page de connexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ConnexionButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PageConnexion());
        }

        /// <summary>
        /// évènement de redirection vers la page d'inscription
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InscriptionButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PageInscription());
        }
    }
}