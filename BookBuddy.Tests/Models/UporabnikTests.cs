using BookBuddy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookBuddy.Tests.Models
{
    [TestClass]
    public class UporabnikTests
    {
        [TestMethod]
        public void Test_DefaultValues_AreEmptyStrings()
        {
            var u = new Uporabnik();

            Assert.AreEqual("", u.UporabniskoIme);
            Assert.AreEqual("", u.Email);
            Assert.AreEqual("", u.Geslo);
            Assert.AreEqual("", u.Ime);
            Assert.AreEqual("", u.Priimek);
            Assert.AreEqual("", u.NajljubsaZvrst);
        }

        [TestMethod]
        public void Test_CanAssignBasicProperties()
        {
            var u = new Uporabnik
            {
                Id = 1,
                UporabniskoIme = "testuser",
                Email = "user@email.com",
                Geslo = "geslo123",
                Ime = "Janez",
                Priimek = "Novak",
                NajljubsaZvrst = "Znanstvena"
            };

            Assert.AreEqual(1, u.Id);
            Assert.AreEqual("testuser", u.UporabniskoIme);
            Assert.AreEqual("user@email.com", u.Email);
            Assert.AreEqual("geslo123", u.Geslo);
            Assert.AreEqual("Janez", u.Ime);
            Assert.AreEqual("Novak", u.Priimek);
            Assert.AreEqual("Znanstvena", u.NajljubsaZvrst);
        }

        [TestMethod]
        public void Test_UserAllowsUpdatingProperties()
        {
            var u = new Uporabnik();

            u.UporabniskoIme = "nova";
            u.Email = "nova@mail.com";
            u.NajljubsaZvrst = "Kriminalka";

            Assert.AreEqual("nova", u.UporabniskoIme);
            Assert.AreEqual("nova@mail.com", u.Email);
            Assert.AreEqual("Kriminalka", u.NajljubsaZvrst);
        }
    }
}
