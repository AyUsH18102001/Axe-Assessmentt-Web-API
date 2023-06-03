using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class TestRules
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestRulesId { get; set; }

        [Required]
        public int Rule { get; set; }

        [JsonIgnore]
        [Required]
        [ForeignKey(nameof(TestId))]
        public int TestId { get; set; }

        [JsonIgnore]
        public Test? Test { get; set; } = null;
    }
}
