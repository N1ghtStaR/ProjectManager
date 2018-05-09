namespace ProjectManagerDB.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Income
    {
        [Key]
        [ForeignKey("Project")]
        public int ProjectID { get; set; }

        [ForeignKey("Develeper")]
        public int DeveleperID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must have positive value!")]
        public double Amount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ReleaseDate { get; set; }

        public virtual Developer Develeper { get; set; }
        public virtual Project Project { get; set; }
    }
}
