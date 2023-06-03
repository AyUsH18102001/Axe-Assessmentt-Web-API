using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class TestType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TestTypeId { get; set; }

        [MaxLength(200)]
        public string Test { get; set; }

        [MaxLength(30)]
        public string short_name { get; set; }

        [JsonIgnore]
        public IEnumerable<Question> Questions { get; set; }
    }
}
