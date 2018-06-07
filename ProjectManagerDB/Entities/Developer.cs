namespace ProjectManagerDB.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Developer
    {
        public enum Character
        {
            Developer, TeamLeader
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(32, MinimumLength = 4)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DefaultValue("Develeper")]
        [Display(Name = "Position")]
        public Character Role { get; set; }

        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
