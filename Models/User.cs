using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; } = 0;

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
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string College { get; set; }

        public string? Password { get; set; } = null;

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "varchar(60)")]
        public string Phone { get; set;}

        public int SelectionStatus { get; set; } = 0;

        public int violation { get; set; }

        [JsonIgnore]
        [MaxLength(20)]
        [Column(TypeName = "varchar(60)")]
        public string? UserToken { get; set;}

        [Column(TypeName = "nvarchar(650)")]
        public string? UserResumeFileName { get; set; } = null;

        [Column(TypeName = "nvarchar(700)")]
        public string? UserProfileImage { get; set; } = null;

        public bool? isAdmin { get; set;} = false;


        public UserTest UserTest { get; set;}

        public List<User_QuestionAttempted> QuestionAttempted { get; set;} = new List<User_QuestionAttempted>();
    }
}
