using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityMVC.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }

        public Course Courses { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
