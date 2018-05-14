namespace ProjectManagerDB.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Project
    {
        public enum Division
        {
            [Display(Name = "Front-end")] FE,
            [Display(Name = "Back-end")] BE
        }

        public enum Statistic
        {
            InProgress, Ready
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("Develeper")]
        public int DeveleperID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(32, MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(256, MinimumLength = 4)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Category")]
        public Division Category { get; set; }

        [DefaultValue("InProgress")]
        public Statistic Status { get; set; }

        public virtual Developer Develeper { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
