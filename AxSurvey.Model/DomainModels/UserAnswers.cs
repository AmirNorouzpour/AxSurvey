using System.ComponentModel.DataAnnotations.Schema;

namespace AxSurvey.Model.DomainModels
{
    public class UserAnswers : BaseEntity<int>
    {
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        public int? UserId { get; set; }
        public string Answers { get; set; }
    }
}
