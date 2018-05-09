namespace ProjectManagerDB.Entities
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Task
    {
        public enum Stats
        {
            InProgress, Ready
        }

        public enum Anteriority
        {
            Low, Medium, High
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("Project")]
        public int ProjectID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(256, MinimumLength = 4)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public Anteriority Priority { get; set; }

        [DefaultValue("InProgress")]
        public Stats Status { get; set; }

        public virtual Project Project { get; set; }
    }
}
