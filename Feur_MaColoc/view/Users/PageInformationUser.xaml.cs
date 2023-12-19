using Feur_MaColoc_Metier;
using Feur_MaColoc_Data;
using Microsoft.Maui.Controls;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc.view.Users
{
    public partial class PageInformationUser : ContentPage
    {
        private AccountDAO accountDAO;
        private Account currentAccount;
        private Entry usernameEntry;
        private Entry loginEntry;
        private Entry mailEntry;
        private Entry currentPasswordEntry;
        private Entry newPasswordEntry;
        private Entry confirmNewPasswordEntry;

        public PageInformationUser(Account account)
        {
            this.BackgroundColor = new Color((float)(224 / 255.0), (float)(255 / 255.0), (float)(255 / 255.0));
            InitializeComponent();
            this.currentAccount = account;
            this.accountDAO = new AccountDAO();

            BuildUserInformationInterface();
        }

        private void BuildUserInformationInterface()
        {

            var BoutonRetour = new Button
            {
                Text = "<",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 30,
                TextColor = Colors.White,
                BackgroundColor = Color.FromArgb("#3860B2"),
                WidthRequest = 60,
                HeightRequest = 60,
                HorizontalOptions = LayoutOptions.Start
            };
            BoutonRetour.Clicked += async (sender, e) => { await Navigation.PopAsync(); };

            var LabelTitre = new Label
            {
                Text = "Mes Informations",
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };

            var gridInformations = new Grid
            {
                Margin = new Thickness(20),
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
                }
            };

            usernameEntry = new Entry { Text = currentAccount.Username };
            loginEntry = new Entry { Text = currentAccount.Login };
            mailEntry = new Entry { Text = currentAccount.Mail };

            gridInformations.Add(new Label { Text = "Nom :" }, 0, 0);
            gridInformations.Add(usernameEntry, 1, 0);
            gridInformations.Add(new Label { Text = "Login :" }, 0, 1);
            gridInformations.Add(loginEntry, 1, 1);
            gridInformations.Add(new Label { Text = "Mail :" }, 0, 2);
            gridInformations.Add(mailEntry, 1, 2);

            var updateInfoButton = new Button
            {
                Text = "Mettre à jour les informations",
                BackgroundColor = Color.FromArgb("#3860B2"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            updateInfoButton.Clicked += UpdateInfoButton_Clicked;

            var gridPassword = new Grid
            {
                Margin = new Thickness(20),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                }
            };

            currentPasswordEntry = new Entry { IsPassword = true };
            newPasswordEntry = new Entry { IsPassword = true };
            confirmNewPasswordEntry = new Entry { IsPassword = true };

            gridPassword.Add(new Label { Text = "Mot de passe actuel :" }, 0, 0);
            gridPassword.Add(currentPasswordEntry, 0, 1);
            gridPassword.Add(new Label { Text = "Nouveau mot de passe :" }, 0, 2);
            gridPassword.Add(newPasswordEntry, 0, 3);
            gridPassword.Add(new Label { Text = "Confirmez le mot de passe :" }, 0, 4);
            gridPassword.Add(confirmNewPasswordEntry, 0, 5);

            var updatePasswordButton = new Button
            {
                Text = "Mettre à jour le mot de passe",
                BackgroundColor = Color.FromArgb("#3860B2"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            updatePasswordButton.Clicked += UpdatePasswordButton_Clicked;

            var stackLayout = new StackLayout
            {
                Children =
                {
                    BoutonRetour,
                    LabelTitre,
                    gridInformations,
                    updateInfoButton, 
                    gridPassword,
                    updatePasswordButton 
                }
            };

            Content = stackLayout;
        }

        private async void UpdateInfoButton_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(usernameEntry.Text) ||
                string.IsNullOrEmpty(loginEntry.Text) ||
                string.IsNullOrEmpty(mailEntry.Text))
            {
                await DisplayAlert("Erreur", "Les champs ne doivent pas être vides", "OK");
                return;
            }
            else 
            {
                this.currentAccount.Username = usernameEntry.Text;
                this.currentAccount.Login = loginEntry.Text;
                this.currentAccount.Mail = mailEntry.Text;
            };


            bool updateResult = await accountDAO.EditAccount(currentAccount);
            if (updateResult)
            {
                await DisplayAlert("Succès", "Informations mises à jour avec succès", "OK");
            }
            else
            {
                await DisplayAlert("Erreur", "La mise à jour des informations a échoué", "OK");
            }
        }

        private async void UpdatePasswordButton_Clicked(object sender, EventArgs e)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string hashedCurrentPassword = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(currentPasswordEntry.Text))).Replace("-", "").ToLower();
                if (currentAccount.Password != hashedCurrentPassword)
                {
                    await DisplayAlert("Erreur", "Le mot de passe actuel est incorrect", "OK");
                    currentPasswordEntry.Text = "";
                    return;
                }
            }

            if (newPasswordEntry.Text != confirmNewPasswordEntry.Text)
            {
                await DisplayAlert("Erreur", "le nouveau mot de passe n'est pas le même que sa validation", "OK");
                newPasswordEntry.Text = "";
                confirmNewPasswordEntry.Text =  "";
                return;

            }
            using (SHA256 sha256 = SHA256.Create())
            {
                string hashedNewPassword = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(newPasswordEntry.Text))).Replace("-", "").ToLower();
                currentAccount.Password = hashedNewPassword;
            }

            bool updatePasswordResult = await accountDAO.EditAccount(currentAccount);
            if (updatePasswordResult)
            {
                await DisplayAlert("Succès", "Mot de passe mis à jour avec succès", "OK");
                newPasswordEntry.Text = "";
                confirmNewPasswordEntry.Text = "";
                currentPasswordEntry.Text = "";
            }
            else
            {
                await DisplayAlert("Erreur", "La mise à jour du mot de passe a échoué", "OK");
                newPasswordEntry.Text = "";
                confirmNewPasswordEntry.Text = "";
                currentPasswordEntry.Text = "";
            }

        }
    }
}
