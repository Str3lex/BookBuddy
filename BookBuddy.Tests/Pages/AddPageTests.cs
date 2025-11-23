using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Data;
using BookBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class AddPageTests
    {
        private DataStore CreateStore()
        {
            var db = TestDbHelper.CreateSQLiteDb();
            return new DataStore(db);
        }

        private DataStore CreateStoreWithUser()
        {
            var db = TestDbHelper.CreateSQLiteDb();
            var store = new DataStore(db);

            var user = new Uporabnik
            {
                Id = 1,
                UporabniskoIme = "test",
                Email = "test@mail.si",
                Geslo = "123"
            };

            store.Uporabniki.Add(user);
            store.TrenutniUporabnik = user;

            return store;
        }

        [TestMethod]
        public void Test_OnPost_NotLoggedIn_RedirectToLogin()
        {
            var store = CreateStore();
            var page = new AddModel(store)
            {
                Naslov = "A",
                Avtor = "B",
                Zanr = "Drama",
                LetoIzdaje = 2000
            };

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        }

        [TestMethod]
        public void Test_OnPost_InvalidModel_ReturnsPage()
        {
            var store = CreateStoreWithUser();
            var page = new AddModel(store);

            page.ModelState.AddModelError("err", "napaka");

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public void Test_OnPost_Valid_AddsBook()
        {
            var store = CreateStoreWithUser();
            var page = new AddModel(store)
            {
                Naslov = "TestBook",
                Avtor = "Tester",
                Zanr = "Z",
                LetoIzdaje = 2020
            };

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(1, store.Knjige.Count);
            Assert.AreEqual("TestBook", store.Knjige[0].Naslov);
        }
    }
}
