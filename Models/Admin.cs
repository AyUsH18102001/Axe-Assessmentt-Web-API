using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxeAssessmentToolWebAPI.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(60)")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(TypeName = "nvarchar(150)")]
        public string Email { get; set; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string Phone { get; set; }
    }
}
