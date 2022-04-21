using AxSurvey.Controllers;
using AxSurvey.Model.DomainModels;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AxSurveyTest.Controllers
{
    [TestClass]
    public class SurveyControllerTest
    {
        private Mock<IBaseRepository<Survey>> _surveyRepository;
        private Mock<IBaseRepository<UserAnswers>> _userAnswersRepository;

        [TestMethod]
        public void TestItemView()
        {
            var controller = new SurveyController(_surveyRepository.Object, _userAnswersRepository.Object);
            var result = controller.Item(18) as ViewResult;
            Assert.AreEqual(null, result?.ViewName);

        }

        [TestInitialize]
        public void SetUp()
        {
            _surveyRepository = new Mock<IBaseRepository<Survey>>();
            _userAnswersRepository = new Mock<IBaseRepository<UserAnswers>>();

        }
    }
}
