using BookBuddy.Models;
using BookBuddy.Pages.Auth;
using BookBuddy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BookBuddy.Tests.Pages.Auth
{
    [TestClass]
    public class FollowModelTests
    {
        [TestMethod]
        public void Test_OnGet_FiltersOutCurrentUser()
        {
            // Arrange
            var currentUser = new Uporabnik { UporabniskoIme = "current", Email = "current@test.com" };
            var otherUser1 = new Uporabnik { UporabniskoIme = "user1", Email = "user1@test.com" };
            var otherUser2 = new Uporabnik { UporabniskoIme = "user2", Email = "user2@test.com" };

            var dataStore = new DataStore(null);
            dataStore.Uporabniki = new List<Uporabnik> { currentUser, otherUser1, otherUser2 };
            dataStore.TrenutniUporabnik = currentUser;

            var followModel = new FollowModel(dataStore);

            // Act
            followModel.OnGet();

            // Assert
            Assert.AreEqual(2, followModel.VsiUporabniki.Count);
            Assert.IsFalse(followModel.VsiUporabniki.Any(u => u.UporabniskoIme == "current"));
            Assert.IsTrue(followModel.VsiUporabniki.Any(u => u.UporabniskoIme == "user1"));
            Assert.IsTrue(followModel.VsiUporabniki.Any(u => u.UporabniskoIme == "user2"));
        }

        [TestMethod]
        public void Test_OnPost_Follow_AddsUserToFollowing()
        {
            // Arrange
            var currentUser = new Uporabnik { UporabniskoIme = "current", Email = "current@test.com", Sledi = new List<Uporabnik>() };
            var targetUser = new Uporabnik { UporabniskoIme = "target", Email = "target@test.com" };

            var dataStore = new DataStore(null);
            dataStore.Uporabniki = new List<Uporabnik> { currentUser, targetUser };
            dataStore.TrenutniUporabnik = currentUser;
            dataStore.Aktivnosti = new List<string>();

            var followModel = new FollowModel(dataStore);

            // Act
            var result = followModel.OnPost("target");

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual(1, currentUser.Sledi.Count);
            Assert.AreEqual("target", currentUser.Sledi[0].UporabniskoIme);
            Assert.IsTrue(dataStore.Aktivnosti.Any(a => a.Contains("sledi")));
        }

        [TestMethod]
        public void Test_OnPost_Unfollow_RemovesUserFromFollowing()
        {
            // Arrange
            var targetUser = new Uporabnik { UporabniskoIme = "target", Email = "target@test.com" };
            var currentUser = new Uporabnik
            {
                UporabniskoIme = "current",
                Email = "current@test.com",
                Sledi = new List<Uporabnik> { targetUser }
            };

            var dataStore = new DataStore(null);
            dataStore.Uporabniki = new List<Uporabnik> { currentUser, targetUser };
            dataStore.TrenutniUporabnik = currentUser;
            dataStore.Aktivnosti = new List<string>();

            var followModel = new FollowModel(dataStore);

            // Act
            var result = followModel.OnPost("target");

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual(0, currentUser.Sledi.Count);
            Assert.IsTrue(dataStore.Aktivnosti.Any(a => a.Contains("neha slediti")));
        }

        [TestMethod]
        public void Test_AliSlediUporabniku_ReturnsCorrectBoolean()
        {
            // Arrange
            var targetUser = new Uporabnik { UporabniskoIme = "target", Email = "target@test.com" };
            var currentUser = new Uporabnik
            {
                UporabniskoIme = "current",
                Email = "current@test.com",
                Sledi = new List<Uporabnik> { targetUser }
            };

            var dataStore = new DataStore(null);
            dataStore.TrenutniUporabnik = currentUser;
            var followModel = new FollowModel(dataStore);

            // Act & Assert
            Assert.IsTrue(followModel.AliSlediUporabniku("target"));
            Assert.IsFalse(followModel.AliSlediUporabniku("nonexistent"));
        }

        [TestMethod]
        public void Test_OnPost_WithNullCurrentUser_ReturnsPage()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.TrenutniUporabnik = null; // Ni prijavljenega uporabnika
            var followModel = new FollowModel(dataStore);

            // Act
            var result = followModel.OnPost("someuser");

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public void Test_OnPost_WithNonExistentUser_ReturnsPage()
        {
            // Arrange
            var currentUser = new Uporabnik { UporabniskoIme = "current", Email = "current@test.com" };
            var dataStore = new DataStore(null);
            dataStore.Uporabniki = new List<Uporabnik> { currentUser };
            dataStore.TrenutniUporabnik = currentUser;
            var followModel = new FollowModel(dataStore);

            // Act
            var result = followModel.OnPost("nonexistent");

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }
    }
}