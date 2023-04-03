namespace AxeAssessmentToolWebAPI.Models
{
    public class TestData
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public List<string> Options { get; set; }
    }
}
