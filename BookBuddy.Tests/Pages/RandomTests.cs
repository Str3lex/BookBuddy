using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class RandomModelTests
    {
        [TestMethod]
        public void Test_OnGet_WithBooks_SetsRandomBookAndAddsActivity()
        {
            // Arrange
            var testBooks = new List<Knjiga>
            {
                new Knjiga { Id = 1, Naslov = "Test Knjiga 1", Avtor = "Avtor 1", Zanr = "Roman" },
                new Knjiga { Id = 2, Naslov = "Test Knjiga 2", Avtor = "Avtor 2", Zanr = "Fantazija" }
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = testBooks;
            dataStore.Aktivnosti = new List<string>();
            dataStore.TrenutniUporabnik = new Uporabnik { UporabniskoIme = "TestUser" };

            var randomModel = new RandomModel(dataStore);

            // Act
            randomModel.OnGet();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(randomModel.RandomBook));
            Assert.IsTrue(randomModel.RandomBook.Contains(" - "));
            Assert.IsTrue(randomModel.RandomBook.Contains("("));
            Assert.AreEqual(1, dataStore.Aktivnosti.Count);
            Assert.IsTrue(dataStore.Aktivnosti[0].Contains("TestUser je pregledal naključno knjigo"));
        }

        [TestMethod]
        public void Test_OnGet_WithNoBooks_SetsDefaultMessage()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga>();
            dataStore.Aktivnosti = new List<string>();

            var randomModel = new RandomModel(dataStore);

            // Act
            randomModel.OnGet();

            // Assert
            Assert.AreEqual("Ni knjig na voljo", randomModel.RandomBook);
            Assert.AreEqual(0, dataStore.Aktivnosti.Count);
        }

        [TestMethod]
        public void Test_OnPost_ReturnsRedirectAndSetsRandomBook()
        {
            // Arrange
            var testBooks = new List<Knjiga>
            {
                new Knjiga { Id = 1, Naslov = "Test Knjiga", Avtor = "Test Avtor", Zanr = "Roman" }
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = testBooks;
            dataStore.Aktivnosti = new List<string>();

            var randomModel = new RandomModel(dataStore);

            // Act
            var result = randomModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.IsFalse(string.IsNullOrEmpty(randomModel.RandomBook));
            Assert.IsTrue(randomModel.RandomBook.Contains("Test Knjiga"));
        }
    }
}