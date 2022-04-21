namespace AxSurvey.Model.DomainModels
{
    public class Survey : BaseEntity<int>
    {
        public int? UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
    }
}
