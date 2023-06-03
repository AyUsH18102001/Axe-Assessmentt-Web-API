namespace AxeAssessmentToolWebAPI.Models
{
    public class TestPaper
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string short_name { get; set; }
        public int? TestId { get; set; }
    }

    public class TestPaperEdit
    {
        public List<TestRules> TestRules { get; set; }
        public List<TestPaper> TestQuestions { get; set;}
    }
}
