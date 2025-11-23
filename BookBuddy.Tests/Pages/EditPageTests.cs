using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Data;
using BookBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class EditPageTests
    {
        private DataStore CreateStoreWithBook()
        {
            var db = TestDbHelper.CreateSQLiteDb();
            var store = new DataStore(db);

            var book = new Knjiga
            {
                Id = 1,
                Naslov = "A",
                Avtor = "B",
                Zanr = "C",
                LetoIzdaje = 2000
            };

            // Add into BOTH lists: in-memory + database
            store.Knjige.Add(book);
            db.Knjige.Add(book);
            db.SaveChanges();

            return store;
        }

        [TestMethod]
        public void Test_OnGet_LoadsBook()
        {
            var store = CreateStoreWithBook();
            var page = new EditModel(store) { Id = 1 };

            page.OnGet();

            Assert.AreEqual("A", page.Knjiga.Naslov);
        }

        [TestMethod]
        public void Test_OnGet_NotFound()
        {
            var store = CreateStoreWithBook();
            var page = new EditModel(store) { Id = 999 };

            page.OnGet();

            Assert.AreEqual("Knjiga ni najdena!", page.ErrorSporocilo);
        }

        [TestMethod]
        public void Test_OnPost_Invalid_ReturnsPage()
        {
            var store = CreateStoreWithBook();
            var page = new EditModel(store) { Id = 1 };

            page.ModelState.AddModelError("err", "napaka");

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public void Test_OnPost_Valid_UpdatesBook()
        {
            var store = CreateStoreWithBook();
            var page = new EditModel(store)
            {
                Id = 1,
                Knjiga = new KnjigaViewModel
                {
                    Id = 1,
                    Naslov = "X",
                    Avtor = "Y",
                    Zanr = "Z",
                    LetoIzdaje = 2023
                }
            };

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("X", store.Knjige.First().Naslov);
        }

      
    }
}
