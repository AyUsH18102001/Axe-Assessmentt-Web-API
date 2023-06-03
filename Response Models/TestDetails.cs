namespace AxeAssessmentToolWebAPI.Response_Models
{
    public class TestDetails
    {
        public int TestId { get; set; }
        public int TestCreator { get; set; }
        public string TestName { get; set; }
        public int MCQ_Count { get; set; }
        public int SQL_Count { get; set; }
        public DateTime TestCreatedDate { get; set; }
        public IEnumerable<TestQuestions> TestQuestions { get; set; }
        public IEnumerable<TestRules> TestRules { get; set; }
    }
}
