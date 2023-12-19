using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using System.Numerics;

namespace Feur_MaColoc.view
{

    public partial class PageInscription : ContentPage
    {
        AccountDAO accountDao;

        public PageInscription()
        {
            //couleur du background
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            accountDao = new AccountDAO();
        }
        /// <summary>
        /// Méthode de création de compte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnRegisterClicked(object sender, System.EventArgs e)
        {
           
            if (Password.Text == PasswordConfirmation.Text)
            {
                try
                {
                    Account newAccount;
                    //conditions multiple pour vérifier la validité du compte en train d'être créé
                    if (!string.IsNullOrEmpty(NomComplet.Text) && !string.IsNullOrEmpty(Login.Text) && !string.IsNullOrEmpty(Password.Text) && !string.IsNullOrEmpty(Mail.Text))
                    {
                        newAccount = new Account(NomComplet.Text, Login.Text, Password.Text, Mail.Text, false);
                        await accountDao.CreateAccount(newAccount);
                        await DisplayAlert("", "Création de Compte Réussi.", "OK");
                        await Shell.Current.GoToAsync("//PageConnexion");
                    }
                    else
                    {
                        await DisplayAlert("", "Il manque des informations dans un ou plusieurs champs.", "OK"); 
                    }
                }
                catch(Exception ex)
                {
                    await DisplayAlert("", ex.Message, "OK");
                }   
            }
            else
            {
                await DisplayAlert("","La confirmation du mot de passe n'est pas égal au mot de passe", "OK");
           
            }
        }
        /// <summary>
        /// bouton de retour 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RetourButton_Clicked(object sender, System.EventArgs e)
        {
            //renvoie à la page précédente
            await Navigation.PopAsync();
        }
    }
}