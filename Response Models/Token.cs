namespace AxeAssessmentToolWebAPI.Response_Models
{
    public class Token
    {
        public string? token { get; set; }
        public bool isAdmin { get; set; }
        public string? error { get; set; }
    }
}
