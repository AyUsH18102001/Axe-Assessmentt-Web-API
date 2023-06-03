using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class UserTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTestId { get; set; }

        public bool EndTest { get; set; }

        public int Score { get; set; }

        public int Attempted { get; set; }

        public int TestPeriod { get; set; }
        public int TotalQuestions { get; set; }

        public int TestId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        
        [JsonIgnore]
        public User? User { get; set; }

    }
}
