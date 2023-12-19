using Feur_MaColoc_Metier;
using Xunit;

namespace Feur_MaColoc_TestUnitaire.Metier
{
    public class TestTaskType
    {
        [Fact]
        public void TestConstructeurID()
        {
            int id = 1;
            string name = "Task1";
            int duration = 20;
            string repetition = "Journalier";

            TaskType taskType = new TaskType(id, name, duration, repetition);

            Assert.NotNull(taskType); // test que l'objet taskType n'est pas null
            Assert.Equal(id, taskType.Id); // Vérifie que l'id est bien intialisé
            Assert.Equal(name, taskType.Name); // Vérifie que le nom est bien initialisé
            Assert.Equal(duration, taskType.Duration); // Vérifie que la durée est bien la durée mise en paramètre
            Assert.Equal(repetition, taskType.Repetition); // Vérifie que la repetition est bien la repetition mise en paramètre

        }

        [Fact]
        public void TestConstructeurNomEtDuree()
        {
            string name = "Task1";
            int duration = 15;
            string repetition = "Mensuel";

            TaskType taskType = new TaskType(name, duration, repetition);

            Assert.NotNull(taskType); // Vérifie que l'objet taskType n'est pas null
            Assert.Equal(name, taskType.Name); // Vérifie que le nom est correctement initialisé
            Assert.Equal(duration, taskType.Duration); // Vérifie que la durée est correctement initialisée
            Assert.Equal(repetition, taskType.Repetition); // Vérifie que la repetition est correctement initialisée
            Assert.Equal(0, taskType.Id); // Vérifie que l'id est bien celui par défaut donc 0
        }

        [Fact]
        public void TestConstructeurParDefaut()
        {
            TaskType taskType = new TaskType();

            Assert.NotNull(taskType); // Vérifie que l'objet taskType n'est pas null
            Assert.Equal(0, taskType.Id); // Vérifie que l'id est correctement initialisé par défaut
            Assert.Null(taskType.Name); // Vérifie que le nom est null par défaut
            Assert.Equal(0, taskType.Duration); // Vérifie que l'id est bien celui par défaut donc 0
        }

    }
}

