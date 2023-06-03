namespace AxeAssessmentToolWebAPI.Models
{
    public class Pagination
    {
        public float pageSize { get; set; } = 9f;
        public int pageCount { get; set; }
        public int currentPage { get; set; } = 1;
    }
}
