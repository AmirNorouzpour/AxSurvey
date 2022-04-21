using AxSurvey.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AxSurvey.Tests.Controllers
{
    [TestClass]
    public class SurveyControllerTest
    {
        [TestMethod]
        public void TestDetailsView()
        {
            var controller = new SurveyController();
            var result = controller.Item(2) as ViewResult;
            Assert.AreEqual("Item", result.ViewName);

        }
    }
}
