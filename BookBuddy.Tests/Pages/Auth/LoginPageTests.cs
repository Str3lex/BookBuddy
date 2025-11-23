using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookBuddy.Pages.Auth;
using BookBuddy.Services;
using BookBuddy.Data;
using BookBuddy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class LoginPageTests
    {
        private AppDbContext CreateFakeDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().Options;
            return new AppDbContext(options);
        }

        private DataStore CreateStoreWithUsers()
        {
            var db = CreateFakeDb();
            var store = new DataStore(db);

            store.Uporabniki.Add(new Uporabnik
            {
                Id = 1,
                UporabniskoIme = "testuser",
                Geslo = "123",
                Email = "test@mail.si"
            });

            return store;
        }

        [TestMethod]
        public void Test_OnPost_InvalidCredentials_ReturnsPageWithError()
        {
            var store = CreateStoreWithUsers();
            var page = new LoginModel(store)
            {
                UporabniskoIme = "neobstaja",
                Geslo = "napacno"
            };

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Napačno uporabniško ime ali geslo.", page.Sporocilo);
        }

        [TestMethod]
        public void Test_OnPost_ValidCredentials_SetsLoggedInUser()
        {
            var store = CreateStoreWithUsers();
            var page = new LoginModel(store)
            {
                UporabniskoIme = "testuser",
                Geslo = "123"
            };

            var result = page.OnPost();

            Assert.IsNotNull(store.TrenutniUporabnik);
            Assert.AreEqual("testuser", store.TrenutniUporabnik.UporabniskoIme);
        }

        [TestMethod]
        public void Test_OnPost_ValidCredentials_RedirectsToProfile()
        {
            var store = CreateStoreWithUsers();
            var page = new LoginModel(store)
            {
                UporabniskoIme = "testuser",
                Geslo = "123"
            };

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;

            Assert.AreEqual("/Uporabnik/Profile", redirect.PageName);
        }

        [TestMethod]
        public void Test_OnPost_ValidCredentials_LogsActivity()
        {
            var store = CreateStoreWithUsers();
            var page = new LoginModel(store)
            {
                UporabniskoIme = "testuser",
                Geslo = "123"
            };

            page.OnPost();

            Assert.AreEqual(1, store.Aktivnosti.Count);
            Assert.IsTrue(store.Aktivnosti[0].Contains("testuser"));
            Assert.IsTrue(store.Aktivnosti[0].Contains("prijavil"));
        }
    }
}
