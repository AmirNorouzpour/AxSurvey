using System.ComponentModel.DataAnnotations.Schema;

namespace AxSurvey.Model.DomainModels
{
    public class Answer : BaseEntity<int>
    {
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }
    }
}
