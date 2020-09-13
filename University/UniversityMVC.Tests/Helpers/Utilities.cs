using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMVC.Data;
using UniversityMVC.Models;

namespace UniversityMVC.Tests.Helpers
{
    public class Utilities
    {
        public static void InitializeDbForTests(UniversityContext db)
        {
            var courses = new Course[]
            {
                new Course{Name="C#/.NET", Description="C#/.Net Developer может создавать различные типы приложений, начиная от сайтов и настольных приложений, заканчивая компьютерными играми и решениями для мобильных платформ."},
                new Course{Name="КУРСЫ JAVA", Description="Java является одним из самых популярных языков программирования, используемых разработчиками программного обеспечения на сегодняшний день." },
                new Course{Name="ANDROID С НУЛЯ", Description="В 2017 году в пятерке крупнейших стран Европы доля Android составила 72,3%. В мае 2017 года Google объявила, что за всю историю Android было активировано более 2 млрд Android-устройств." },
                new Course{Name="UI/UX КУРСЫ", Description="На данный момент профессия UI/UX дизайнера одна из самых востребованных в IT индустрии. И спрос на нее только возрастает."},
                new Course{Name="КУРСЫ PYTHON", Description="Python — один из самых популярных и развивающихся языков. Он универсальный, используется в разных сферах. Он достаточно легок в изучении, поэтому порог вхождения низок. При этом разработчики на Python очень востребованы."},
                new Course{Name="Course Without Groups", Description="Description..."}
            };
            var groups = new Group[]
            {
                new Group{CourseId=1, Name="SR-01"},
                new Group{CourseId=2, Name="SR-02"},
                new Group{CourseId=2, Name="SR-03"},
                new Group{CourseId=3, Name="SR-04"},
                new Group{CourseId=4, Name="SR-05"},
                new Group{CourseId=5, Name="SR-06"},
                new Group{CourseId=5, Name="Group To Delete"}
            };
            var students = new Student[]
            {
                new Student{GroupId=1, FirstName="Eddard", LastName="Stark" },
                new Student{GroupId=1, FirstName="Robert", LastName="Lannister"},
                new Student{GroupId=1, FirstName="Catelyn", LastName="Stark"},
                new Student{GroupId=1, FirstName="Daenerys", LastName="Targaryen"},
                new Student{GroupId=1, FirstName="Jorah", LastName="Mormont"},
                new Student{GroupId=1, FirstName="Jon", LastName="Snow"},
                new Student{GroupId=1, FirstName="Sansa", LastName="Stark"},
                new Student{GroupId=1, FirstName="Theon", LastName="Greyjoy"},
                new Student{GroupId=1, FirstName="Samwell", LastName="Tarly"},
                new Student{GroupId=2, FirstName="Rubeus", LastName="Hagrid "},
                new Student{GroupId=2, FirstName="Igor", LastName="Karkaroff"},
                new Student{GroupId=2, FirstName="Viktor", LastName="Krum"},
                new Student{GroupId=2, FirstName="Bellatrix", LastName="Lestrange"},
                new Student{GroupId=2, FirstName="Neville", LastName="Longbottom"},
                new Student{GroupId=2, FirstName="Luna", LastName="Lovegood"},
                new Student{GroupId=2, FirstName="Lucius", LastName="Malfoy"},
                new Student{GroupId=2, FirstName="Pansy", LastName="Parkinson"},
                new Student{GroupId=2, FirstName="Peter", LastName="Pettigrew"},
                new Student{GroupId=2, FirstName="Harry", LastName="Potter" },
                new Student{GroupId=3, FirstName="James", LastName="Potter "},
                new Student{GroupId=3, FirstName="Quirinus", LastName="Quirrell"},
                new Student{GroupId=3, FirstName="Thomas", LastName="Riddle"},
                new Student{GroupId=3, FirstName="Newt", LastName="Scamander"},
                new Student{GroupId=3, FirstName="Rita", LastName="Skeeter"},
                new Student{GroupId=3, FirstName="Horace", LastName="Slughorn"},
                new Student{GroupId=3, FirstName="Salazar", LastName="Slytherin"},
                new Student{GroupId=3, FirstName="Zacharias", LastName="Smith"},
                new Student{GroupId=3, FirstName="Severus", LastName="Snape"},
                new Student{GroupId=3, FirstName="Nymphadora", LastName="Tonks"},
                new Student{GroupId=3, FirstName="Dolores", LastName="Umbridge"},
                new Student{GroupId=4, FirstName="Rick", LastName="Sanchez "},
                new Student{GroupId=4, FirstName="Morty", LastName="Smith"},
                new Student{GroupId=4, FirstName="Beth", LastName="Smith"},
                new Student{GroupId=5, FirstName="Tony", LastName="Stark"},
                new Student{GroupId=5, FirstName="Natasha", LastName="Romanoff"},
                new Student{GroupId=5, FirstName="Bruce", LastName="Banner"},
                new Student{GroupId=5, FirstName="Steve", LastName="Rogers"},
                new Student{GroupId=5, FirstName="Stephen", LastName="Strange"}
            };

            db.Courses.AddRange(courses);
            db.Groups.AddRange(groups);
            db.Students.AddRange(students);

            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(UniversityContext db)
        {
            db.Courses.RemoveRange(db.Courses);
            db.Groups.RemoveRange(db.Groups);
            db.Students.RemoveRange(db.Students);
            InitializeDbForTests(db);
        }
    }
}
