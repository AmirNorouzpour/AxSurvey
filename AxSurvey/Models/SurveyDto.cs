namespace AxSurvey.Models
{
    public class SurveyDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int QuestionsCount { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class SurveyPostDto
    {
        public int Id { get; set; }
     
        public List<QuestionPostDto> Questions { get; set; }
    }
}
