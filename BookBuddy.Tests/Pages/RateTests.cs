using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class RateModelTests
    {
        [TestMethod]
        public void Test_OnPost_WithValidRating_UpdatesBookRatingAndAddsActivity()
        {
            // Arrange
            var testBook = new Knjiga 
            { 
                Id = 1, 
                Naslov = "Test Knjiga", 
                Avtor = "Test Avtor", 
                Rate = 0  // Začetna ocena
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga> { testBook };
            dataStore.Aktivnosti = new List<string>();
            dataStore.TrenutniUporabnik = new Uporabnik { UporabniskoIme = "TestUser" };

            var rateModel = new RateModel(dataStore)
            {
                Id = 1,
                Ocena = 4  // Nova ocena
            };

            // Act
            var result = rateModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual("/Books/List", ((RedirectToPageResult)result).PageName);
            Assert.AreEqual(4, testBook.Rate);  // Preveri, da se je ocena posodobila
            Assert.AreEqual(1, dataStore.Aktivnosti.Count);
            Assert.IsTrue(dataStore.Aktivnosti[0].Contains("TestUser je ocenil knjigo 'Test Knjiga' z oceno 4"));
        }

        [TestMethod]
        public void Test_OnPost_WithInvalidRating_ReturnsPageWithErrorMessage()
        {
            // Arrange
            var testBook = new Knjiga 
            { 
                Id = 1, 
                Naslov = "Test Knjiga", 
                Avtor = "Test Avtor",
                Rate = 3
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga> { testBook };
            dataStore.Aktivnosti = new List<string>();

            var rateModel = new RateModel(dataStore)
            {
                Id = 1,
                Ocena = 6  // Neveljavna ocena
            };

            // Act
            var result = rateModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult)); // PageResult je pravi tip
            Assert.AreEqual("Ocena mora biti med 1 in 5!", rateModel.Sporocilo);
            Assert.AreEqual(3, testBook.Rate);  // Preveri, da se ocena NI spremenila
            Assert.AreEqual(0, dataStore.Aktivnosti.Count);  // Preveri, da ni aktivnosti
        }

        [TestMethod]
        public void Test_OnPost_WithNonExistentBook_ReturnsPageWithErrorMessage()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga>();  // Prazna lista - knjiga ne obstaja
            dataStore.Aktivnosti = new List<string>();

            var rateModel = new RateModel(dataStore)
            {
                Id = 999,  // Neobstoječi ID
                Ocena = 5
            };

            // Act
            var result = rateModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult)); // PageResult je pravi tip
            Assert.AreEqual("Knjiga ni najdena!", rateModel.Sporocilo);
            Assert.AreEqual(0, dataStore.Aktivnosti.Count);  // Preveri, da ni aktivnosti
        }
    }
}