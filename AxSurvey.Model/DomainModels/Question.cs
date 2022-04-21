using System.ComponentModel.DataAnnotations.Schema;

namespace AxSurvey.Model.DomainModels
{
    public class Question : BaseEntity<int>
    {
        public int SurveyId { get; set; }
        [ForeignKey("SurveyId")]
        public Survey Survey { get; set; }
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public List<Answer> Answers { get; set; }
        public bool HasTextualAnswer => Type == QuestionType.Text;
    }

    public enum QuestionType
    {
        Text,
        Dropdown,
        Multiple
    }
}
