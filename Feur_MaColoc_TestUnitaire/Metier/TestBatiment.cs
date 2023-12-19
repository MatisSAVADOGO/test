using System.Collections.Generic;
using Xunit;

namespace Feur_MaColoc_Metier.Tests
{
    public class TestBatiment
    {
        [Fact]
        public void TestConstructeurCompteChambrePiece()
        {
            int id = 1;
            string name = "Batiment1";
            string adresse = "Adresse1";
            int codePostale = 12345;
            string ville = "Ville1";
            int surface = 100;
            List<Account> accounts = new List<Account>();
            List<Chambre> chambres = new List<Chambre>();
            List<Piece> pieces = new List<Piece>();

            Batiment batiment = new Batiment(id, name, adresse, codePostale, ville, surface, accounts, chambres, pieces);

            Assert.NotNull(batiment);
            Assert.Equal(id, batiment.Id);
            Assert.Equal(name, batiment.Name);
            Assert.Equal(adresse, batiment.Adresse);
            Assert.Equal(codePostale, batiment.CodePostale);
            Assert.Equal(ville, batiment.Ville);
            Assert.Equal(surface, batiment.Surface);
            Assert.Same(accounts, batiment.Accounts);
            Assert.Same(chambres, batiment.Chambres);
            Assert.Same(pieces, batiment.Pieces);
        }
        [Fact]
        public void TestConstructeur()
        {
            int id = 1;
            string name = "Batiment1";
            string adresse = "Adresse1";
            int codePostale = 12345;
            string ville = "Ville1";
            int surface = 100;

            Batiment batiment = new Batiment(id, name, adresse, codePostale, ville, surface);

            Assert.NotNull(batiment);
            Assert.Equal(id, batiment.Id);
            Assert.Equal(name, batiment.Name);
            Assert.Equal(adresse, batiment.Adresse);
            Assert.Equal(codePostale, batiment.CodePostale);
            Assert.Equal(ville, batiment.Ville);
            Assert.Equal(surface, batiment.Surface);
            Assert.Empty(batiment.Accounts);
        }
    }
}
