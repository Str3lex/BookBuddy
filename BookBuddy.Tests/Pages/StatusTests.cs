using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class StatusModelTests
    {
        [TestMethod]
        public void Test_OnPost_WithValidBookAndStatus_UpdatesStatusAndAddsActivity()
        {
            // Arrange
            var testKnjiga = new Knjiga 
            { 
                Id = 1, 
                Naslov = "Test Knjiga", 
                Status = "Ni prebrana"  // Začetni status
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga> { testKnjiga };
            dataStore.Aktivnosti = new List<string>();
            dataStore.TrenutniUporabnik = new Uporabnik { UporabniskoIme = "TestUser" };

            var statusModel = new StatusModel(dataStore)
            {
                Id = 1,
                Status = "Prebrana"  // Nov status
            };

            // Act
            var result = statusModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Prebrana", testKnjiga.Status);  // Preveri posodobljen status
            Assert.AreEqual($"Status knjige 'Test Knjiga' je bil uspešno posodobljen na: Prebrana", statusModel.Sporocilo);
            Assert.AreEqual(1, dataStore.Aktivnosti.Count);
            Assert.IsTrue(dataStore.Aktivnosti[0].Contains("TestUser je spremenil status knjige 'Test Knjiga' iz 'Ni prebrana' na 'Prebrana'"));
        }

        [TestMethod]
        public void Test_OnPost_WithNonExistentBook_ReturnsErrorMessage()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga>();  // Prazna lista - knjiga ne obstaja
            dataStore.Aktivnosti = new List<string>();

            var statusModel = new StatusModel(dataStore)
            {
                Id = 999,  // Neobstoječi ID
                Status = "Prebrana"
            };

            // Act
            var result = statusModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Knjiga ni najdena!", statusModel.Sporocilo);
            Assert.AreEqual(0, dataStore.Aktivnosti.Count);  // Preveri, da ni aktivnosti
        }

        [TestMethod]
        public void Test_OnPost_WithZeroId_ReturnsErrorMessage()
        {
            // Arrange
            var testKnjiga = new Knjiga 
            { 
                Id = 1, 
                Naslov = "Test Knjiga", 
                Status = "Ni prebrana"
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga> { testKnjiga };
            dataStore.Aktivnosti = new List<string>();

            var statusModel = new StatusModel(dataStore)
            {
                Id = 0,  // Neveljaven ID
                Status = "Prebrana"
            };

            // Act
            var result = statusModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Izberite knjigo!", statusModel.Sporocilo);
            Assert.AreEqual("Ni prebrana", testKnjiga.Status);  // Preveri, da se status NI spremenil
            Assert.AreEqual(0, dataStore.Aktivnosti.Count);  // Preveri, da ni aktivnosti
        }
    }
}