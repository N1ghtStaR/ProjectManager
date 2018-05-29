namespace ProjectManager.Models
{
    using ProjectManagerDB.Entities;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class DeveloperViewModel : BaseViewModel
    {
        public enum Character
        {
            Developer, TeamLeader
        }

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

        public DeveloperViewModel(Developer developer)
        {
            ID = developer.ID;
            Username = developer.Username;
            Password = developer.Password;
            Email = developer.Email;
            Role = (Character)developer.Role;
        }

        public DeveloperViewModel()
        {

        }
    }
}