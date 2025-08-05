using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.Models   // <-- make sure namespace matches your project
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(18, 60)]
        public int Age { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // NEW fields
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }   // nullable to avoid migration errors if existing rows

        [Range(0, 100)]
        public int Marks { get; set; }

        [StringLength(100)]
        public string City { get; set; }
    }
}
