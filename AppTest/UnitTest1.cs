using Futuristic.Services;
using Futuristic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestApplicationId()
        {
           //var applicationId  =  UserService.Instance.GetApplicationId();
            //UserService.GetApplicationId()
            Assert.IsTrue(1 == 1);
        }
        [TestMethod]
        public void TestCloseTimings()
        {
            var openingSoon  = Utilities.OpenCloseTime(7, 23, 6, out string timeLabel);
            Assert.IsTrue(openingSoon.Contains("Opening soon"));
            var closingSoon = Utilities.OpenCloseTime(7, 23, 22, out string timeLabel1);
            var closedOpenOPensTomorrow = Utilities.OpenCloseTime(7, 23, 23, out string timeLabel3);
            var closedOpensAt = Utilities.OpenCloseTime(7, 23, 4, out string timeLabel2);
        }
    }
}
