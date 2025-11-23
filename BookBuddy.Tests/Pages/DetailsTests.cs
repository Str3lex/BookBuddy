using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class DetailsModelTests
    {
        [TestMethod]
        public void Test_OnGet_FindsBookById()
        {
            // Arrange
            var testBook = new Knjiga { Id = 1, Naslov = "Test Knjiga", Avtor = "Test Avtor" };
            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga> { testBook };
            var detailsModel = new DetailsModel(dataStore) { Id = 1 };

            // Act
            detailsModel.OnGet();

            // Assert
            Assert.IsNotNull(detailsModel.Knjiga);
            Assert.AreEqual("Test Knjiga", detailsModel.Knjiga.Naslov);
            Assert.AreEqual("Test Avtor", detailsModel.Knjiga.Avtor);
        }

        [TestMethod]
        public void Test_OnGet_WithNonExistentId_SetsBookToNull()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga>();
            var detailsModel = new DetailsModel(dataStore) { Id = 999 };

            // Act
            detailsModel.OnGet();

            // Assert
            Assert.IsNull(detailsModel.Knjiga);
        }

        [TestMethod]
        public void Test_OnGet_FiltersCommentsByBookId()
        {
            // Arrange
            var testBook = new Knjiga { Id = 1, Naslov = "Test Knjiga" };
            var comments = new List<Komentar>
            {
                new Komentar { Id = 1, KnjigaId = 1, Besedilo = "Komentar za knjigo 1" },
                new Komentar { Id = 2, KnjigaId = 2, Besedilo = "Komentar za knjigo 2" },
                new Komentar { Id = 3, KnjigaId = 1, Besedilo = "Drugi komentar za knjigo 1" }
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga> { testBook };
            dataStore.Komentarji = comments;
            var detailsModel = new DetailsModel(dataStore) { Id = 1 };

            // Act
            detailsModel.OnGet();

            // Assert
            Assert.AreEqual(2, detailsModel.KomentarjiZaKnjigo.Count);
            Assert.IsTrue(detailsModel.KomentarjiZaKnjigo.All(k => k.KnjigaId == 1));
            Assert.IsFalse(detailsModel.KomentarjiZaKnjigo.Any(k => k.KnjigaId == 2));
        }

        [TestMethod]
        public void Test_TrenutniUporabnikIme_ReturnsCorrectUsername()
        {
            // Arrange
            var currentUser = new Uporabnik { UporabniskoIme = "testuser", Email = "test@test.com" };
            var dataStore = new DataStore(null);
            dataStore.TrenutniUporabnik = currentUser;
            var detailsModel = new DetailsModel(dataStore);

            // Act & Assert
            Assert.AreEqual("testuser", detailsModel.TrenutniUporabnikIme);
        }

        [TestMethod]
        public void Test_TrenutniUporabnikIme_WithNullUser_ReturnsNull()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.TrenutniUporabnik = null;
            var detailsModel = new DetailsModel(dataStore);

            // Act & Assert
            Assert.IsNull(detailsModel.TrenutniUporabnikIme);
        }

        [TestMethod]
        public void Test_OnGet_OrdersCommentsByDateDescending()
        {
            // Arrange
            var testBook = new Knjiga { Id = 1, Naslov = "Test Knjiga" };
            var comments = new List<Komentar>
            {
                new Komentar { Id = 1, KnjigaId = 1, Besedilo = "Starejši", Datum = new DateTime(2024, 1, 1) },
                new Komentar { Id = 2, KnjigaId = 1, Besedilo = "Novejši", Datum = new DateTime(2024, 1, 2) }
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = new List<Knjiga> { testBook };
            dataStore.Komentarji = comments;
            var detailsModel = new DetailsModel(dataStore) { Id = 1 };

            // Act
            detailsModel.OnGet();

            // Assert
            Assert.AreEqual("Novejši", detailsModel.KomentarjiZaKnjigo[0].Besedilo);
            Assert.AreEqual("Starejši", detailsModel.KomentarjiZaKnjigo[1].Besedilo);
        }
    }
}