using AxSurvey.Model.DomainModels;

namespace AxSurvey.Models
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QType { get; set; }
        public IEnumerable<AnswerDto> Answers { get; set; }
        public string AnswersText { get; set; }
        public bool HasTextualAnswer => QType == QuestionType.Text;
    }

    public class QuestionPostDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
