using ProjectManagerDB.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class MessageViewModel : BaseViewModel
    {
        [ForeignKey("Developer")]
        public int DeveloperID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public MessageViewModel(Message message)
        {
            ID = message.ID;
            DeveloperID = message.DeveloperID;
            Username = message.Username;
            Description = message.Description;
            Date = message.Date;
        }

        public MessageViewModel(int id, int developerID, string username, string password, string description, DateTime date)
        {
            ID = id;
            DeveloperID = developerID;
            Username = username;
            Description = description;
            Date = date;
        }

        public MessageViewModel()
        {

        }
    }
}