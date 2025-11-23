using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookBuddy.Pages.Books;
using BookBuddy.Services;
using BookBuddy.Data;
using BookBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookBuddy.Tests.Pages
{
    [TestClass]
    public class ListPageTests
    {
        private DataStore CreateStoreWithBooks()
        {
            var db = TestDbHelper.CreateSQLiteDb();
            var store = new DataStore(db);

            store.Knjige.AddRange(new[]
            {
                new Knjiga { Id = 1, Naslov = "B", Avtor = "X", Zanr = "Z" },
                new Knjiga { Id = 2, Naslov = "A", Avtor = "Y", Zanr = "Z" }
            });

            foreach (var k in store.Knjige)
                db.Knjige.Add(k);

            db.SaveChanges();
            return store;
        }

        [TestMethod]
        public void Test_OnGet_SortsBooks()
        {
            var store = CreateStoreWithBooks();
            var page = new ListModel(store);

            page.OnGet();

            Assert.AreEqual("A", page.Knjige.First().Naslov);
        }

        [TestMethod]
        public void Test_OnGet_Filtering()
        {
            var store = CreateStoreWithBooks();
            var page = new ListModel(store)
            {
                Iskanje = "B"
            };

            page.OnGet();

            Assert.AreEqual(1, page.Knjige.Count);
            Assert.AreEqual("B", page.Knjige[0].Naslov);
        }

        [TestMethod]
        public void Test_OnPostDelete_DeletesBook()
        {
            var store = CreateStoreWithBooks();
            var page = new ListModel(store);

            var result = page.OnPostDelete(1);

            Assert.AreEqual(1, store.Knjige.Count);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        }
    }
}
