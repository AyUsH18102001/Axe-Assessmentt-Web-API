using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxeAssessmentToolWebAPI.Models
{
    public class Rules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RuleId { get; set; }

        public string Rule { get; set; }
        [MaxLength(50)]
        public string short_name { get; set; }

        public string RuleDisplay { get; set; }

    }
}
