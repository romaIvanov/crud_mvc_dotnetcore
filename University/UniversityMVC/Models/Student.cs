using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityMVC.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Group Groups { get; set; }
    }
}
