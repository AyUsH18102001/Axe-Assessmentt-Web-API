namespace AxeAssessmentToolWebAPI.FrontendModel
{
    public class TestSubmit
    {
        public int UserId { get; set; }
        public int TestPeriod { get; set; }
        public int TotalQuestions { get; set; }
        public int Violation { get; set; }
        public List<CandidatesQuestion> CandidatesQuestions { get; set; }
    }

    public class CandidatesQuestion
    {
        public int QuestionId { get; set; }
        public int QuestionNo { get; set; }
        public List<int> SelectedOption { get; set; }
    }

    public class Question_Option
    {
        public int QuestionId { get; set; }
        public List<Option> Options {get; set;}
        public List<int> Answer { get; set; }
    }


}
