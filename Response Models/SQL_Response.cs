namespace AxeAssessmentToolWebAPI.Response_Models
{
    public class SQL_Response
    {
        public MessageAndCode? Message { get; set; }
        public List<List<string>>? Result { get; set; }
    }
}
