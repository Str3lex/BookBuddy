using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class CommentsModelTests
    {
        [TestMethod]
        public void Test_Komentarji_ReturnsDataStoreComments()
        {
            // Arrange
            var testComments = new List<Komentar>
            {
                new Komentar { Id = 1, Uporabnik = "User1", Besedilo = "Test komentar 1" },
                new Komentar { Id = 2, Uporabnik = "User2", Besedilo = "Test komentar 2" }
            };

            var dataStore = new DataStore(null);
            dataStore.Komentarji = testComments;
            var commentsModel = new CommentsModel(dataStore);

            // Act
            var result = commentsModel.Komentarji;

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Test komentar 1", result[0].Besedilo);
            Assert.AreEqual("Test komentar 2", result[1].Besedilo);
        }

        [TestMethod]
        public void Test_OnPost_WithValidComment_AddsCommentToDataStore()
        {
            // Arrange
            var currentUser = new Uporabnik { UporabniskoIme = "testuser", Email = "test@test.com" };
            var dataStore = new DataStore(null);
            dataStore.Komentarji = new List<Komentar>();
            dataStore.Aktivnosti = new List<string>();
            dataStore.TrenutniUporabnik = currentUser;

            var commentsModel = new CommentsModel(dataStore)
            {
                VnosKomentarja = "Nov testni komentar"
            };

            // Act
            var result = commentsModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual(1, dataStore.Komentarji.Count);
            Assert.AreEqual("testuser", dataStore.Komentarji[0].Uporabnik);
            Assert.AreEqual("Nov testni komentar", dataStore.Komentarji[0].Besedilo);
            Assert.IsTrue(dataStore.Aktivnosti.Any(a => a.Contains("komentiral")));
        }

        [TestMethod]
        public void Test_OnPost_WithEmptyComment_DoesNotAddComment()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Komentarji = new List<Komentar>();
            dataStore.Aktivnosti = new List<string>();

            var commentsModel = new CommentsModel(dataStore)
            {
                VnosKomentarja = "" // Prazen komentar
            };

            // Act
            var result = commentsModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual(0, dataStore.Komentarji.Count);
            Assert.AreEqual(0, dataStore.Aktivnosti.Count);
        }

        [TestMethod]
        public void Test_OnPost_WithWhitespaceComment_DoesNotAddComment()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Komentarji = new List<Komentar>();
            dataStore.Aktivnosti = new List<string>();

            var commentsModel = new CommentsModel(dataStore)
            {
                VnosKomentarja = "   " // Samo presledki
            };

            // Act
            var result = commentsModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual(0, dataStore.Komentarji.Count);
            Assert.AreEqual(0, dataStore.Aktivnosti.Count);
        }

        [TestMethod]
        public void Test_OnPost_WithNullCurrentUser_UsesGuestName()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Komentarji = new List<Komentar>();
            dataStore.Aktivnosti = new List<string>();
            dataStore.TrenutniUporabnik = null; // Brez prijavljenega uporabnika

            var commentsModel = new CommentsModel(dataStore)
            {
                VnosKomentarja = "Komentar kot gost"
            };

            // Act
            var result = commentsModel.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual(1, dataStore.Komentarji.Count);
            Assert.AreEqual("Gost", dataStore.Komentarji[0].Uporabnik);
            Assert.AreEqual("Komentar kot gost", dataStore.Komentarji[0].Besedilo);
            Assert.IsTrue(dataStore.Aktivnosti.Any(a => a.Contains("Gost")));
        }

        [TestMethod]
        public void Test_OnPost_SetsCurrentDateTime()
        {
            // Arrange
            var currentUser = new Uporabnik { UporabniskoIme = "testuser", Email = "test@test.com" };
            var dataStore = new DataStore(null);
            dataStore.Komentarji = new List<Komentar>();
            dataStore.Aktivnosti = new List<string>();
            dataStore.TrenutniUporabnik = currentUser;

            var commentsModel = new CommentsModel(dataStore)
            {
                VnosKomentarja = "Test komentar z datumom"
            };

            var beforePost = DateTime.Now;

            // Act
            var result = commentsModel.OnPost();

            // Assert
            var afterPost = DateTime.Now;
            var commentDate = dataStore.Komentarji[0].Datum;

            Assert.IsTrue(commentDate >= beforePost && commentDate <= afterPost);
        }
    }
}
