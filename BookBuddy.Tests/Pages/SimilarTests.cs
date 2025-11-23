using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class SimilarModelTests
    {
        [TestMethod]
        public void Test_OnGet_WithIzbraneKnjige_ReturnsSimilarBooksSameGenre()
        {
            // Arrange
            var zadnjaIzbranaKnjiga = new Knjiga 
            { 
                Id = 1, 
                Naslov = "Glavna Knjiga", 
                Avtor = "Avtor A", 
                Zanr = "Fantazija" 
            };

            var testKnjige = new List<Knjiga>
            {
                zadnjaIzbranaKnjiga,
                new Knjiga { Id = 2, Naslov = "Podobna Knjiga 1", Avtor = "Avtor B", Zanr = "Fantazija" },
                new Knjiga { Id = 3, Naslov = "Podobna Knjiga 2", Avtor = "Avtor C", Zanr = "Fantazija" },
                new Knjiga { Id = 4, Naslov = "Drugačen Žanr", Avtor = "Avtor D", Zanr = "Roman" } // Ne bi smela biti vključena
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = testKnjige;
            dataStore.IzbraneKnjige = new List<Knjiga> { 
                new Knjiga { Id = 5, Naslov = "Nekaj Starega", Avtor = "Avtor E", Zanr = "Zgodovina" },
                zadnjaIzbranaKnjiga  // Zadnja izbrana knjiga
            };

            var similarModel = new SimilarModel(dataStore);

            // Act
            similarModel.OnGet();

            // Assert
            Assert.AreEqual(2, similarModel.PodobneKnjige.Count);
            Assert.IsTrue(similarModel.PodobneKnjige.Contains("Podobna Knjiga 1 - Avtor B"));
            Assert.IsTrue(similarModel.PodobneKnjige.Contains("Podobna Knjiga 2 - Avtor C"));
            Assert.IsFalse(similarModel.PodobneKnjige.Contains("Glavna Knjiga - Avtor A")); // Ne vključi izvirne knjige
            Assert.IsFalse(similarModel.PodobneKnjige.Contains("Drugačen Žanr - Avtor D")); // Ne vključi drugega žanra
        }

        [TestMethod]
        public void Test_OnGet_WithNoIzbraneKnjige_ReturnsEmptyList()
        {
            // Arrange
            var testKnjige = new List<Knjiga>
            {
                new Knjiga { Id = 1, Naslov = "Knjiga 1", Avtor = "Avtor 1", Zanr = "Fantazija" },
                new Knjiga { Id = 2, Naslov = "Knjiga 2", Avtor = "Avtor 2", Zanr = "Fantazija" }
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = testKnjige;
            dataStore.IzbraneKnjige = new List<Knjiga>(); // Prazna lista izbranih knjig

            var similarModel = new SimilarModel(dataStore);

            // Act
            similarModel.OnGet();

            // Assert
            Assert.AreEqual(0, similarModel.PodobneKnjige.Count);
        }

        [TestMethod]
        public void Test_OnGet_WithIzbraneKnjigeButNoSimilar_ReturnsEmptyList()
        {
            // Arrange
            var zadnjaIzbranaKnjiga = new Knjiga 
            { 
                Id = 1, 
                Naslov = "Samotna Knjiga", 
                Avtor = "Avtor A", 
                Zanr = "Redki Žanr" 
            };

            var testKnjige = new List<Knjiga>
            {
                zadnjaIzbranaKnjiga,
                new Knjiga { Id = 2, Naslov = "Druga Knjiga", Avtor = "Avtor B", Zanr = "Pogost Žanr" } // Drugačen žanr
            };

            var dataStore = new DataStore(null);
            dataStore.Knjige = testKnjige;
            dataStore.IzbraneKnjige = new List<Knjiga> { zadnjaIzbranaKnjiga };

            var similarModel = new SimilarModel(dataStore);

            // Act
            similarModel.OnGet();

            // Assert
            Assert.AreEqual(0, similarModel.PodobneKnjige.Count); // Ni podobnih knjig istega žanra
        }
    }
}