using BookBuddy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BookBuddy.Tests.Models
{
    [TestClass]
    public class KomentarTests
    {
        [TestMethod]
        public void Test_DefaultDatum_IsCurrentDate()
        {
            // Arrange
            var komentar = new Komentar();
            var expectedDate = DateTime.Now;

            // Assert - preveri da je datum približno trenutni datum (do minute natančno)
            Assert.AreEqual(expectedDate.Year, komentar.Datum.Year);
            Assert.AreEqual(expectedDate.Month, komentar.Datum.Month);
            Assert.AreEqual(expectedDate.Day, komentar.Datum.Day);
        }

        [TestMethod]
        public void Test_CanSetAllProperties()
        {
            // Arrange
            var testDatum = new DateTime(2024, 1, 15);
            var komentar = new Komentar
            {
                Id = 1,
                Uporabnik = "Testni Uporabnik",
                Besedilo = "To je testni komentar",
                Datum = testDatum,
                KnjigaId = 5
            };

            // Assert
            Assert.AreEqual(1, komentar.Id);
            Assert.AreEqual("Testni Uporabnik", komentar.Uporabnik);
            Assert.AreEqual("To je testni komentar", komentar.Besedilo);
            Assert.AreEqual(testDatum, komentar.Datum);
            Assert.AreEqual(5, komentar.KnjigaId);
        }

        [TestMethod]
        public void Test_Besedilo_MaxLengthConstraint()
        {
            // Arrange
            var komentar = new Komentar();

            // Act - testiraj da lahko shrani besedilo do 500 znakov
            var maxLengthText = new string('a', 500);
            komentar.Besedilo = maxLengthText;

            // Assert
            Assert.AreEqual(500, komentar.Besedilo.Length);
            Assert.AreEqual(maxLengthText, komentar.Besedilo);
        }
    }
}
