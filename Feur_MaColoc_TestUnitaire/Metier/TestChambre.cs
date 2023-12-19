using Feur_MaColoc_Metier;
using Xunit;

namespace Feur_MaColoc_TestUnitaire.Metier
{
    public class TestChambre
    {
        [Fact]
        public void TestAddLocataire()
        {
            Account account = new Account("user1", "login1", "pass1", "user1@gmail.com", true);


            Chambre chambre = new Chambre();

            chambre.AddLocataire(account);


            Assert.Equal(account, chambre.Locataire); //le compte account est bien le locataire de la chambre
            Assert.Equal(chambre, account.Chambre);  //la chambre "chambre" est bien considéré comme la chambre de account

            Account account2 = new Account("user2", "login2", "motdepasse", "user2@gmail.com", false);
            Account account3 = new Account("user3", "login3", "modtepasse", "user3@gmail.com", true);
            Chambre chambre2 = new Chambre();

            chambre2.AddLocataire(account2);

            Assert.False(account.Chambre.Equals(account2.Chambre)); //la chambre de account est bien différente que la chambre de account2
            Assert.Null(account3.Chambre); //la chambre de account3 n'est pas encore affectée

        }
        [Fact]
        public void TestConstructeurChambreLocataire()
        {
            int id = 1;
            string name = "Chambre1";
            int surface = 20;
            Batiment batiment = new Batiment();
            Account account = new Account("user1", "login1", "pass1", "user1@gmail.com", true);

            Chambre chambre = new Chambre(id, name, surface, account, batiment);

            Assert.Equal(id, chambre.Id); //l'id est bien l'id contenu dans la chambre
            Assert.Equal(name, chambre.Name); //le nom est bien le nom contenu dans la chambre
            Assert.Equal(surface, chambre.Surface); //la surface est bien la surface contenue dans la chambre
            Assert.Equal(batiment, chambre.Batiment);   //le batiment est bien le batiment contenu dans la chambre
            Assert.Equal(account, chambre.Locataire);   //le locataire est bien le locataire contenu dans la chambre
        }

        [Fact]
        public void TestConstructeurChambreNoLocataire()
        {
            int id = 1;
            string name = "Chambre1";
            int surface = 20;
            Batiment batiment = new Batiment();

            Chambre chambre = new Chambre(id, name, surface, batiment);

            //comme les tests pour id name surface et bâtiment sont fonctionnel au dessus, ici on ne traite que le locataire
            Assert.Null(chambre.Locataire);//le locataire de la chambre est null car aucun dans le constructeur
        }

    }
}
