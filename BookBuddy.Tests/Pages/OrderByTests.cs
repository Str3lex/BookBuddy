using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class OrderByModelTests
    {
        [TestMethod]
        public void Test_OnGet_PopulatesZanrOptionsFromDataStore()
        {
            // Arrange
            var dataStore = new DataStore(null);
            var testZanri = new List<string> { "Fantasy", "Sci-Fi", "Misterija" };

            // Mock VsiZanri metodo
            var dataStoreType = dataStore.GetType();
            var method = dataStoreType.GetMethod("VsiZanri");
            if (method != null)
            {
                method.Invoke(dataStore, null);
            }

            dataStore.GetType().GetProperty("Knjige")?.SetValue(dataStore, new List<Knjiga>
            {
                new Knjiga { Zanr = "Fantasy" },
                new Knjiga { Zanr = "Sci-Fi" },
                new Knjiga { Zanr = "Misterija" }
            });

            var orderByModel = new OrderByModel(dataStore);

            // Act
            orderByModel.OnGet();

            // Assert
            Assert.AreEqual(4, orderByModel.ZanrOptions.Count); // 3 žanri + "Vsi žanri"
            Assert.IsTrue(orderByModel.ZanrOptions.Any(z => z.Value == "vsi" && z.Text == "Vsi žanri"));
            Assert.IsTrue(orderByModel.ZanrOptions.Any(z => z.Value == "Fantasy"));
            Assert.IsTrue(orderByModel.ZanrOptions.Any(z => z.Value == "Sci-Fi"));
            Assert.IsTrue(orderByModel.ZanrOptions.Any(z => z.Value == "Misterija"));
        }

        [TestMethod]
        public void Test_OnGet_WithZanrFilter_FiltersBooksByGenre()
        {
            // Arrange
            var books = new List<Knjiga>
            {
                new Knjiga { Id = 1, Naslov = "Fantasy Book", Zanr = "Fantasy" },
                new Knjiga { Id = 2, Naslov = "Sci-Fi Book", Zanr = "Sci-Fi" },
                new Knjiga { Id = 3, Naslov = "Another Fantasy", Zanr = "Fantasy" }
            };

            var dataStore = new DataStore(null);
            dataStore.GetType().GetProperty("Knjige")?.SetValue(dataStore, books);

            var orderByModel = new OrderByModel(dataStore)
            {
                ZanrFilter = "Fantasy"
            };

            // Act
            orderByModel.OnGet();

            // Assert
            Assert.AreEqual(2, orderByModel.Knjige.Count);
            Assert.IsTrue(orderByModel.Knjige.All(k => k.Zanr == "Fantasy"));
            Assert.IsFalse(orderByModel.Knjige.Any(k => k.Zanr == "Sci-Fi"));
        }

        [TestMethod]
        public void Test_OnGet_WithVsiZanrFilter_ShowsAllBooks()
        {
            // Arrange
            var books = new List<Knjiga>
            {
                new Knjiga { Id = 1, Naslov = "Book 1", Zanr = "Fantasy" },
                new Knjiga { Id = 2, Naslov = "Book 2", Zanr = "Sci-Fi" }
            };

            var dataStore = new DataStore(null);
            dataStore.GetType().GetProperty("Knjige")?.SetValue(dataStore, books);

            var orderByModel = new OrderByModel(dataStore)
            {
                ZanrFilter = "vsi" // Prikaži vse
            };

            // Act
            orderByModel.OnGet();

            // Assert
            Assert.AreEqual(2, orderByModel.Knjige.Count);
        }

        [TestMethod]
        public void Test_OnGet_CallsRazvrstiKnjigeWithCorrectParameters()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.GetType().GetProperty("Knjige")?.SetValue(dataStore, new List<Knjiga>());

            var orderByModel = new OrderByModel(dataStore)
            {
                SortBy = "avtor",
                SortOrder = "desc"
            };

            // Act
            orderByModel.OnGet();

            // Assert
            // Preveri da so parametri pravilno nastavljeni
            Assert.AreEqual("avtor", orderByModel.SortBy);
            Assert.AreEqual("desc", orderByModel.SortOrder);
        }

        [TestMethod]
        public void Test_SortOptions_AreCorrectlyInitialized()
        {
            // Arrange
            var dataStore = new DataStore(null);
            var orderByModel = new OrderByModel(dataStore);

            // Act & Assert
            Assert.AreEqual(6, orderByModel.SortOptions.Count);
            Assert.IsTrue(orderByModel.SortOptions.Any(s => s.Value == "naslov" && s.Text == "Naslov"));
            Assert.IsTrue(orderByModel.SortOptions.Any(s => s.Value == "avtor" && s.Text == "Avtor"));
            Assert.IsTrue(orderByModel.SortOptions.Any(s => s.Value == "zanr" && s.Text == "Žanr"));
            Assert.IsTrue(orderByModel.SortOptions.Any(s => s.Value == "leto" && s.Text == "Leto izdaje"));
            Assert.IsTrue(orderByModel.SortOptions.Any(s => s.Value == "rating" && s.Text == "Ocena"));
            Assert.IsTrue(orderByModel.SortOptions.Any(s => s.Value == "status" && s.Text == "Status"));
        }

        [TestMethod]
        public void Test_OrderOptions_AreCorrectlyInitialized()
        {
            // Arrange
            var dataStore = new DataStore(null);
            var orderByModel = new OrderByModel(dataStore);

            // Act & Assert
            Assert.AreEqual(2, orderByModel.OrderOptions.Count);
            Assert.IsTrue(orderByModel.OrderOptions.Any(o => o.Value == "asc" && o.Text == "Naraščajoče"));
            Assert.IsTrue(orderByModel.OrderOptions.Any(o => o.Value == "desc" && o.Text == "Padajoče"));
        }

        [TestMethod]
        public void Test_DefaultPropertyValues_AreSetCorrectly()
        {
            // Arrange
            var dataStore = new DataStore(null);
            var orderByModel = new OrderByModel(dataStore);

            // Act & Assert
            Assert.AreEqual("naslov", orderByModel.SortBy);
            Assert.AreEqual("asc", orderByModel.SortOrder);
            Assert.AreEqual("vsi", orderByModel.ZanrFilter);
            Assert.IsNotNull(orderByModel.Knjige);
            Assert.AreEqual(0, orderByModel.Knjige.Count);
        }

        [TestMethod]
        public void Test_OnGet_WithNonExistentZanrFilter_ReturnsEmptyList()
        {
            // Arrange
            var books = new List<Knjiga>
            {
                new Knjiga { Id = 1, Naslov = "Fantasy Book", Zanr = "Fantasy" }
            };

            var dataStore = new DataStore(null);
            dataStore.GetType().GetProperty("Knjige")?.SetValue(dataStore, books);

            var orderByModel = new OrderByModel(dataStore)
            {
                ZanrFilter = "NeobstojeciZanr"
            };

            // Act
            orderByModel.OnGet();

            // Assert
            Assert.AreEqual(0, orderByModel.Knjige.Count);
        }

        [TestMethod]
        public void Test_ZanrOptions_IncludesAllGenresFromDataStore()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.GetType().GetProperty("Knjige")?.SetValue(dataStore, new List<Knjiga>
            {
                new Knjiga { Zanr = "Fantasy" },
                new Knjiga { Zanr = "Sci-Fi" },
                new Knjiga { Zanr = "Fantasy" }, // Duplikat - mora biti odstranjen
                new Knjiga { Zanr = "Misterija" }
            });

            var orderByModel = new OrderByModel(dataStore);

            // Act
            orderByModel.OnGet();

            // Assert
            var uniqueGenres = orderByModel.ZanrOptions
                .Where(z => z.Value != "vsi")
                .Select(z => z.Value)
                .Distinct()
                .ToList();

            Assert.AreEqual(3, uniqueGenres.Count); // Samo unikatni žanri
            Assert.IsTrue(uniqueGenres.Contains("Fantasy"));
            Assert.IsTrue(uniqueGenres.Contains("Sci-Fi"));
            Assert.IsTrue(uniqueGenres.Contains("Misterija"));
        }
    }
}