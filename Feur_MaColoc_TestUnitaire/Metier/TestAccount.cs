using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Feur_MaColoc_TestUnitaire.Metier
{
    public class TestAccount
    {
        [Fact]
        public void TestAccountEquality()
        {
            
            Account account1 = new Account("user1", "login1", "pass1", "user1@gmail.com", true);
            Account account2 = new Account("user1", "login1", "pass1", "user1@gmail.com", true);
            Account account3 = new Account("user2", "login2", "pass2", "user2@gmail.com", false);

            Assert.Equal(account1, account2);  // Les comptes devraient être égaux car ils ont le même ID
            Assert.NotEqual(account1, account3);  // Les comptes ne devraient pas être égaux car ils ont des ID différents
        }

        [Fact]
        public void TestAccountHashCode()
        {
            
            Account account1 = new Account("user1", "login1", "pass1", "user1@gmail.com", true);
            Account account2 = new Account("user1", "login1", "pass1", "user1@gmail.com", true);

            Assert.Equal(account1.GetHashCode(), account2.GetHashCode());  // Les hashs devraient être égaux car les comptes sont égaux
        }
    }
}
