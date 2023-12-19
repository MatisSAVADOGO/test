using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using Feur_MaColoc.view.Admin;
using Feur_MaColoc.view.Users;
using System.Data;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Feur_MaColoc.view
{

    public partial class PageConnexion : ContentPage
    {
        //Liste des comptes en BDD récupéré plus tard
        List<Account> comptes = new List<Account>();
        //Futur compte connecté a l'application
        Account compteValided;
        AccountDAO accountDAO;
        public PageConnexion()
        {
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            accountDAO = new AccountDAO();
        }

        /// <summary>
        /// Bouton de connexion à un compte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Login.Text) || string.IsNullOrWhiteSpace(Password.Text))
                {
                    await DisplayAlert("", "Veuillez saisir un login et un mot de passe.", "OK");
                    return;
                }

                bool compteValide = false;
                DataTable comptesDataTable = await accountDAO.RecupAllCompteBDD();
                comptes = accountDAO.ConvertDataTableToList(comptesDataTable);

                foreach (Account compte in comptes)
                {
                    // Utilisation de l'algorithme de hachage SHA-256 pour comparer les mots de passe
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hashedPasswordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password.Text));
                        string hashedPasswordInput = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();

                        if (compte.Login == Login.Text && compte.Password == hashedPasswordInput)
                        {
                            compteValide = true;
                            compteValided = compte;
                            
                            break;
                        }
                    }
                }

                if (compteValide)
                {
                    await DisplayAlert("", "Connexion au compte réussi.", "OK");
                    Login.Text = "";
                    Password.Text = "";

                    // Simple condition pour vérifier si c'est un admin et envoyer sur la page adaptée
                    if (compteValided.IsAdmin == true)
                    {
                        await Navigation.PushAsync(new PageAccueilAdmin());
                    }
                    else
                    {
                        await Navigation.PushAsync(new PageAccueilUser(compteValided));
                    }
                }
                else
                {
                    await DisplayAlert("", "Login ou mot de passe incorrect.", "OK");
                }
            }
            catch
            {
                await DisplayAlert("", "Connexion au serveur échoué", "OK");
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
