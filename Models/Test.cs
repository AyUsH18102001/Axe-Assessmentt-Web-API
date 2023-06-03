using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxeAssessmentToolWebAPI.Models
{
    public class Test
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestId { get; set; }
        public string TestName { get; set; }
        public int TestCreator { get; set; }
        public int MCQ_Count { get; set; }
        public int SQL_Count { get; set; }
        public DateTime TestCreatedDate { get; set; }
        public DateTime? D_Date { get; set; }
        public int? D_Id { get; set; }
        public DateTime? U_Date { get; set; }
        public int? U_Id { get; set; }
        public List<TestQuestions> TestQuestions { get; set; } = new List<TestQuestions>();
        public List<TestRules> TestRules { get; set; } = new List<TestRules>();
    }
}
