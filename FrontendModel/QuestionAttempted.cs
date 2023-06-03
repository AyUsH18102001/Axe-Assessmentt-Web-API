namespace AxeAssessmentToolWebAPI.FrontendModel
{
    public class QuestionAttempted
    {
        public int userId { get; set; }
        public int questionId { get; set; }
        public List<int> optionsIndex { get; set; }
    }
}
