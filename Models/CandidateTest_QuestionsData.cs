namespace AxeAssessmentToolWebAPI.Models
{
    public class CandidateTest_QuestionsData
    {
        public List<TestData> AptitudeQuestions = new List<TestData>();
        public IQueryable<SQL_TestData> SqlQuestions;
    }
}
