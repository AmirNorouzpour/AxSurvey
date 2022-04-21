using AxSurvey.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AxSurvey.Model.DomainModels;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AxSurvey.Controllers
{
    public class SurveyController : Controller
    {
        private readonly IBaseRepository<Survey> _repository;
        private readonly IBaseRepository<UserAnswers> _userAnswersRepository;

        public SurveyController(IBaseRepository<Survey> repository, IBaseRepository<UserAnswers> userAnswersRepository)
        {
            _repository = repository;
            _userAnswersRepository = userAnswersRepository;
        }

        public IActionResult Index()
        {
            var surveys = _repository.GetAll().Select(survey => new SurveyDto
            {
                Id = survey.Id,
                Title = survey.Title,
                CreationDate = survey.CreationDate,
                ModifiedDate = survey.ModifiedDate,
                QuestionsCount = survey.Questions.Count
            }).ToList();
            return View(surveys);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Item(int? id = null)
        {
            var viewModel = GetViewModel(id);
            return View(viewModel);
        }

        private SurveyDto GetViewModel(int? id)
        {
            var item = _repository.GetAll(x => x.Id == id).Include(x => x.Questions).ThenInclude(x => x.Answers).FirstOrDefault();
            if (item == null)
                return new SurveyDto();

            var viewModel = new SurveyDto
            {
                Id = item.Id,
                QuestionsCount = item.Questions.Count,
                Title = item.Title,
                Questions = item.Questions.Select(question => new QuestionDto
                {
                    Id = question.Id,
                    QType = question.Type,
                    QuestionText = question.QuestionText,
                    AnswersText = string.Join("\n", question.Answers.OrderBy(o => o.Value).Select(answer => answer.Title))
                }).ToList()
            };
            return viewModel;
        }

        public IActionResult View(int? id = null)
        {
            var viewModel = GetViewModel(id);
            return View(viewModel);
        }

        public JsonResult Delete(int id)
        {
            var item = _repository.GetById(id);
            _repository.Delete(item);
            return new JsonResult(new Result { Success = true, Msg = "Survey deleted successfully." });
        }

        public ActionResult DisplayNewQuizSet()
        {
            return PartialView("_Question", new QuestionDto());
        }

        [HttpPost]
        public JsonResult Save([FromBody] SurveyDto dto)
        {
            if (dto.Id > 0)
            {
                var survey = _repository.GetAll(x => x.Id == dto.Id).Include(x => x.Questions).ThenInclude(x => x.Answers).FirstOrDefault();

                if (survey != null)
                {
                    survey.ModifiedDate = DateTime.Now;
                    survey.Title = dto.Title;
                    //survey.Questions = new List<Question>();

                    foreach (var question in dto.Questions)
                    {
                        var q = survey.Questions.FirstOrDefault(x => x.Id == question.Id);
                        if (q != null)
                        {
                            q.QuestionText = question.QuestionText;
                            q.Type = question.QType;
                            q.Answers = GetQAnswers(question);
                        }
                        else
                        {
                            survey.Questions.Add(new Question
                            {
                                Type = question.QType,
                                QuestionText = question.QuestionText,
                                Answers = GetQAnswers(question),
                                CreationDate = DateTime.Now
                            });
                        }
                    }

                    _repository.Update(survey);
                }
            }
            else
            {
                var survey = new Survey
                {
                    CreationDate = DateTime.Now,
                    Title = dto.Title,
                    UserId = 1,
                    Questions = new List<Question>()
                };
                foreach (var question in dto.Questions)
                {
                    survey.Questions.Add(new Question
                    {
                        Type = question.QType,
                        QuestionText = question.QuestionText,
                        Answers = GetQAnswers(question),
                        CreationDate = DateTime.Now
                    });
                }
                _repository.Add(survey);
            }
            return new JsonResult(new Result { Success = true, Msg = "Survey saved successfully." });
        }

        private List<Answer> GetQAnswers(QuestionDto questionDto)
        {

            if (questionDto.QType == QuestionType.Text)
            {
                return null!;
            }

            if (questionDto.QType is QuestionType.Dropdown or QuestionType.Multiple)
            {
                var list = new List<Answer>();
                var strings = questionDto.AnswersText.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                for (var index = 0; index < strings.Length; index++)
                {
                    var answer = strings[index];
                    list.Add(new Answer { Value = index, Title = answer, CreationDate = DateTime.Now });
                }
                return list;
            }

            throw new ArgumentOutOfRangeException();
        }

        public IActionResult SaveResult([FromBody] SurveyPostDto dto)
        {
            var answers = new List<UserAnswers>();
            foreach (var q in dto.Questions)
            {
                var au = new UserAnswers { QuestionId = q.Id, UserId = null, Answers = q.Value, CreationDate = DateTime.Now };
                answers.Add(au);
            }
            _userAnswersRepository.AddRange(answers);
            return new JsonResult(new Result { Success = true, Msg = "Survey submitted successfully." });
        }
    }

}