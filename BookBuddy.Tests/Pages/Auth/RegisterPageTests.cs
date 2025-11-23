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
    public class RegisterPageTests
    {
        private AppDbContext CreateFakeDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().Options;
            return new AppDbContext(options);
        }

        private DataStore CreateEmptyStore()
        {
            var db = CreateFakeDb();
            return new DataStore(db);
        }

        private DataStore CreateStoreWithUser()
        {
            var db = CreateFakeDb();
            var store = new DataStore(db);

            store.Uporabniki.Add(new Uporabnik
            {
                Id = 1,
                UporabniskoIme = "obstaja",
                Email = "test@mail.si",
                Geslo = "123"
            });

            return store;
        }

        [TestMethod]
        public void Test_OnPost_UsernameExists_ReturnsPageWithError()
        {
            var store = CreateStoreWithUser();
            var page = new RegisterModel(store)
            {
                UporabniskoIme = "obstaja",
                Eposta = "nova@mail.si",
                Geslo = "1234"
            };

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Uporabnik že obstaja!", page.Sporocilo);
        }

        [TestMethod]
        public void Test_OnPost_ValidRegistration_AddsUser()
        {
            var store = CreateEmptyStore();
            var page = new RegisterModel(store)
            {
                UporabniskoIme = "nov",
                Eposta = "nov@mail.si",
                Geslo = "geslo"
            };

            var result = page.OnPost();

            Assert.AreEqual(1, store.Uporabniki.Count);
            Assert.AreEqual("nov", store.Uporabniki[0].UporabniskoIme);
            Assert.AreEqual("nov@mail.si", store.Uporabniki[0].Email);
        }

        [TestMethod]
        public void Test_OnPost_ValidRegistration_SetsLoggedInUser()
        {
            var store = CreateEmptyStore();
            var page = new RegisterModel(store)
            {
                UporabniskoIme = "nov",
                Eposta = "nov@mail.si",
                Geslo = "geslo"
            };

            page.OnPost();

            Assert.IsNotNull(store.TrenutniUporabnik);
            Assert.AreEqual("nov", store.TrenutniUporabnik.UporabniskoIme);
        }

        [TestMethod]
        public void Test_OnPost_ValidRegistration_AddsActivity()
        {
            var store = CreateEmptyStore();
            var page = new RegisterModel(store)
            {
                UporabniskoIme = "nov",
                Eposta = "nov@mail.si",
                Geslo = "geslo"
            };

            page.OnPost();

            Assert.AreEqual(1, store.Aktivnosti.Count);
            Assert.IsTrue(store.Aktivnosti[0].Contains("nov"));
        }

        [TestMethod]
        public void Test_OnPost_ValidRegistration_RedirectsToProfile()
        {
            var store = CreateEmptyStore();
            var page = new RegisterModel(store)
            {
                UporabniskoIme = "nov",
                Eposta = "nov@mail.si",
                Geslo = "geslo"
            };

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("/Uporabnik/Profile", redirect.PageName);
        }
    }
}
