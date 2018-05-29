namespace ProjectManager.Models
{
    using ProjectManagerDB.Entities;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProjectViewModel : BaseViewModel
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

        public ProjectViewModel(Project project)
        {
            ID = project.ID;
            DeveleperID = project.DeveleperID;
            Title = project.Title;
            Description = project.Description;
            Category = (Division)project.Category;
            Status = (Statistic)project.Status;
        }

        public ProjectViewModel()
        {

        }
    }
}