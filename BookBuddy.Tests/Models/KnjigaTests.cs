using BookBuddy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookBuddy.Tests.Models
{
    [TestClass]
    public class KnjigaTests
    {
        [TestMethod]
        public void Test_DefaultRate_IsZero()
        {
            var k = new Knjiga();
            Assert.AreEqual(0, k.Rate);
        }

        [TestMethod]
        public void Test_CanSetBasicProperties()
        {
            var k = new Knjiga
            {
                Id = 1,
                Naslov = "Testni naslov",
                Avtor = "Testni avtor",
                Status = "Prebrana",
                Rate = 5
            };

            Assert.AreEqual(1, k.Id);
            Assert.AreEqual("Testni naslov", k.Naslov);
            Assert.AreEqual("Testni avtor", k.Avtor);
            Assert.AreEqual("Prebrana", k.Status);
            Assert.AreEqual(5, k.Rate);
        }

        [TestMethod]
        public void Test_Status_AllowsAnyString()
        {
            var k = new Knjiga
            {
                Status = "V teku"
            };

            Assert.AreEqual("V teku", k.Status);
        }
    }
}
