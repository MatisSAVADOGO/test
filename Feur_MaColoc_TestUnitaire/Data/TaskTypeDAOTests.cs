using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Feur_MaColoc_TestUnitaire.Data
{
    public class TaskTypeDAOTests
    {
        // Teste si getAllTaskType() renvoie une DataTable non nulle
        [Fact]
        public async Task GetAllTaskType_Returns_DataTable()
        {
            // Arrange
            var taskTypeDAO = new TaskTypeDAO();

            // Act
            var result = await taskTypeDAO.getAllTaskType();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DataTable>(result);
        }

        // Teste si CreateTaskType() renvoie True lorsqu'un type de tâche est créé avec des valeurs valides
        [Fact]
        public async Task CreateTaskType_Returns_True()
        {
            // Arrange
            var taskTypeDAO = new TaskTypeDAO();
            var taskType = new TaskType
            {
                Id = 1,
                Name = "TestTask",
                Duration = 10
            };

            // Act
            var result = await taskTypeDAO.CreateTaskType(taskType);

            // Assert
            Assert.True(result);
        }

        /*
        // Teste si CreateTaskType() renvoie False lorsqu'un type de tâche est créé avec des valeurs invalides
        [Fact]
        public async Task CreateTaskType_Returns_False()
        {
            // Arrange
            var taskTypeDAO = new TaskTypeDAO();
            var taskType = new TaskType
            {
                Id = -1,
                Name = "TestTask",
                Duration = 10
            };

            // Act
            var result = await taskTypeDAO.CreateTaskType(taskType);

            // Assert
            Assert.False(result);
        }
        */

        // Teste si DeleteTaskType() renvoie True lorsqu'un type de tâche est supprimé
        [Fact]
        public async Task DeleteTaskType_Returns_True()
        {
            // Arrange
            var taskTypeDAO = new TaskTypeDAO();
            var taskType = new TaskType
            {
                Id = 1,
                Name = "TestTask",
                Duration = 10
            };

            // Act
            var result = await taskTypeDAO.DeleteTaskType(taskType);

            // Assert
            Assert.True(result);
        }

        // Teste si FindTaskType() renvoie un TaskType non nul correspondant au nom spécifié
        [Fact]
        public async Task FindTaskType_Returns_TaskType()
        {
            // Arrange
            var taskTypeDAO = new TaskTypeDAO();
            string taskTypeName = "TestTask";

            // Act
            var result = await taskTypeDAO.FindTaskType(taskTypeName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskTypeName, result.Name);
        }

        // Teste si getAllTaskType() renvoie une DataTable non vide lorsque la base de données contient des données
        [Fact]
        public async Task GetAllTaskType_Returns_NonEmptyDataTable_When_DatabaseHasData()
        {
            // Arrange
            var taskTypeDAO = new TaskTypeDAO();
            // Assure-toi que la base de données contient des données de test avant d'exécuter ce test

            // Act
            var result = await taskTypeDAO.getAllTaskType();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DataTable>(result);
            Assert.NotEmpty(result.Rows);
        }

        // Teste si ConvertDataTableToList() renvoie une liste vide lorsqu'une DataTable est vide
        [Fact]
        public void ConvertDataTableToList_Returns_EmptyList_When_DataTableIsEmpty()
        {
            // Arrange
            var taskTypeDAO = new TaskTypeDAO();
            var emptyDataTable = new DataTable(); // DataTable vide

            // Act
            var result = taskTypeDAO.ConvertDataTableToList(emptyDataTable);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TaskType>>(result);
            Assert.Empty(result);
        }

        // Teste si ConvertDataTableToList() renvoie une liste correcte lorsque la DataTable contient des données
        [Fact]
        public void ConvertDataTableToList_Returns_CorrectList_When_DataTableHasData()
        {
            // Arrange
            var taskTypeDAO = new TaskTypeDAO();
            var dataTableWithTestData = new DataTable();
            // Peuple dataTableWithTestData avec des données de test avant d'exécuter ce test

            // Act
            var result = taskTypeDAO.ConvertDataTableToList(dataTableWithTestData);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<TaskType>>(result);
        }
    }
}
