namespace AxeAssessmentToolWebAPI.FrontendModel
{
    public class SubmitSql
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string Query { get; set; }
        public bool Result { get; set; }
    }
}
