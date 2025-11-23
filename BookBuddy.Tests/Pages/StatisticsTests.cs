using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class StatisticsModelTests
    {
        [TestMethod]
        public void Test_OnGet_WithMixedBooks_CalculatesCorrectStatistics()
        {
            // Arrange
            var testKnjige = new List<Knjiga>
            {
                new Knjiga { Id = 1, Naslov = "Knjiga 1", Status = "Prebrana", Rate = 5 },
                new Knjiga { Id = 2, Naslov = "Knjiga 2", Status = "Prebrana", Rate = 4 },
                new Knjiga { Id = 3, Naslov = "Knjiga 3", Status = "V branju", Rate = 0 },
                new Knjiga { Id = 4, Naslov = "Knjiga 4", Status = "Ni prebrana", Rate = 0 },
                new Knjiga { Id = 5, Naslov = "Knjiga 5", Status = "Prebrana", Rate = 3 }
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = testKnjige;

            var statisticsModel = new StatisticsModel(dataStore);

            // Act
            statisticsModel.OnGet();

            // Assert
            Assert.AreEqual(5, statisticsModel.VsehKnjig);
            Assert.AreEqual(3, statisticsModel.SteviloPrebranihKnjig);
            Assert.AreEqual(1, statisticsModel.SteviloVBranju);
            Assert.AreEqual(1, statisticsModel.SteviloNeprebranih);
            Assert.AreEqual(4.0, statisticsModel.PovprecnaOcena); // (5+4+3)/3 = 4.0
            Assert.AreEqual(5, statisticsModel.VseKnjige.Count);
        }

        [TestMethod]
        public void Test_OnGet_WithNoBooks_ReturnsZeroStatistics()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga>(); // Prazna lista

            var statisticsModel = new StatisticsModel(dataStore);

            // Act
            statisticsModel.OnGet();

            // Assert
            Assert.AreEqual(0, statisticsModel.VsehKnjig);
            Assert.AreEqual(0, statisticsModel.SteviloPrebranihKnjig);
            Assert.AreEqual(0, statisticsModel.SteviloVBranju);
            Assert.AreEqual(0, statisticsModel.SteviloNeprebranih);
            Assert.AreEqual(0, statisticsModel.PovprecnaOcena);
            Assert.AreEqual(0, statisticsModel.VseKnjige.Count);
        }

        [TestMethod]
        public void Test_OnGet_WithNoRatedBooks_CalculatesZeroAverageRating()
        {
            // Arrange
            var testKnjige = new List<Knjiga>
            {
                new Knjiga { Id = 1, Naslov = "Knjiga 1", Status = "Prebrana", Rate = 0 },
                new Knjiga { Id = 2, Naslov = "Knjiga 2", Status = "V branju", Rate = 0 },
                new Knjiga { Id = 3, Naslov = "Knjiga 3", Status = "Ni prebrana", Rate = 0 }
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = testKnjige;

            var statisticsModel = new StatisticsModel(dataStore);

            // Act
            statisticsModel.OnGet();

            // Assert
            Assert.AreEqual(3, statisticsModel.VsehKnjig);
            Assert.AreEqual(1, statisticsModel.SteviloPrebranihKnjig);
            Assert.AreEqual(1, statisticsModel.SteviloVBranju);
            Assert.AreEqual(1, statisticsModel.SteviloNeprebranih);
            Assert.AreEqual(0, statisticsModel.PovprecnaOcena); // Vse ocene so 0
            Assert.AreEqual(3, statisticsModel.VseKnjige.Count);
        }
    }
}