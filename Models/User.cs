using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class User
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId {get; set;}

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(60)")]
        public string FirstName { get; set;}

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(60)")]
        public string LastName { get; set;}

        [Required]
        [MaxLength(100)]
        public string Email { get; set;}

        [Required]
        public string Password { get; set;}

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "varchar(60)")]
        public string Phone { get; set;}

        public int SelectionStatus { get; set;}

        public int violation { get; set; }

        [JsonIgnore]
        [MaxLength(20)]
        [Column(TypeName = "varchar(60)")]
        public string? UserToken { get; set;}

        [JsonIgnore]
        [Column(TypeName = "nvarchar(150)")]
        public string? UserResumeFileName { get; set; }

        public bool? isAdmin { get; set;} = false;

        public UserTest UserTest { get; set;}
    }
}
