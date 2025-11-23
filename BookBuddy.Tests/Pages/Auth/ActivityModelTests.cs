using BookBuddy.Pages.Auth;
using BookBuddy.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BookBuddy.Tests.Pages.Auth
{
    [TestClass]
    public class ActivityModelTests
    {
        [TestMethod]
        public void Test_OnGet_LoadsActivitiesFromDataStore()
        {
            // Arrange
            var testActivities = new List<string> { "Test aktivnost 1", "Test aktivnost 2" };
            var dataStore = new DataStore(null);
            dataStore.Aktivnosti = testActivities;
            var activityModel = new ActivityModel(dataStore);

            // Act
            activityModel.OnGet();

            // Assert
            Assert.AreEqual(2, activityModel.Aktivnosti.Count);
            Assert.AreEqual("Test aktivnost 1", activityModel.Aktivnosti[0]);
            Assert.AreEqual("Test aktivnost 2", activityModel.Aktivnosti[1]);
        }

        [TestMethod]
        public void Test_OnGet_WithNullActivities_InitializesEmptyList()
        {
            // Arrange
            var dataStore = new DataStore(null);
            dataStore.Aktivnosti = null; // Nastavi na null
            var activityModel = new ActivityModel(dataStore);

            // Act
            activityModel.OnGet();

            // Assert
            Assert.IsNotNull(activityModel.Aktivnosti);
            Assert.AreEqual(0, activityModel.Aktivnosti.Count);
        }

        [TestMethod]
        public void Test_ActivityModel_Constructor_SetsDataStore()
        {
            // Arrange
            var dataStore = new DataStore(null);

            // Act
            var activityModel = new ActivityModel(dataStore);

            // Assert
            Assert.IsNotNull(activityModel);
            Assert.AreEqual(dataStore, activityModel.GetType().GetField("_dataStore",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(activityModel));
        }
    }
}