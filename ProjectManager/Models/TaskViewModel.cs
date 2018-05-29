namespace ProjectManager.Models
{
    using ProjectManagerDB.Entities;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TaskViewModel : BaseViewModel
    {
        public enum Stats
        {
            InProgress, Ready
        }

        public enum Anteriority
        {
            Low, Medium, High
        }

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

        public TaskViewModel(Task task)
        {
            ID = task.ID;
            ProjectID = task.ProjectID;
            Description = task.Description;
            Priority = (Anteriority)task.Priority;
            Status = (Stats)task.Status;
        }

        public TaskViewModel()
        {

        }
    }
}