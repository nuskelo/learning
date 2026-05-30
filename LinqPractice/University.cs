using System.Data.Common;

class UniversityManager
{
    public List<Student> students;
    public List<University> universities;

    public UniversityManager()
    {
        universities = new List<University>();
        students = new List<Student>();

        universities.Add(new University { Id = 1, Name = "nltu" });
        universities.Add(new University { Id = 2, Name = "lnu" });

        students.Add(new Student { Id = 1, Name = "Vladа", Age = 20, Gender = "Female", UniversityId = 1 });
        students.Add(new Student { Id = 2, Name = "Dima", Age = 20, Gender = "Male", UniversityId = 1 });
        students.Add(new Student { Id = 3, Name = "Yarik", Age = 21, Gender = "Male", UniversityId = 2 });
        students.Add(new Student { Id = 4, Name = "Tikhon", Age = 20, Gender = "Male", UniversityId = 1 });
        students.Add(new Student { Id = 5, Name = "Oksana", Age = 19, Gender = "Female", UniversityId = 2 });
        students.Add(new Student { Id = 6, Name = "Andriy", Age = 22, Gender = "Male", UniversityId = 2 });
    }

    public void FemaleStudents()
    {
        var maleStudents = from student in students where student.Gender == "Female" select student;
        System.Console.WriteLine("Female Students:");
        foreach (var student in maleStudents)
        {
            student.StudentInfo();
        }
    }

    public void SortStudentsByAge()
    {
        var sortedStudents = from student in students orderby student.Age select student;
        System.Console.WriteLine("Students sorted by age:");
        foreach (var student in sortedStudents)
        {
            student.StudentInfo();
        }
    }

    public void AllStudentsFromNltu()
    {
        var studentsFromNltu = from student in students join university in universities on student.UniversityId equals university.Id where university.Name == "nltu" select student;

        System.Console.WriteLine("Students from NLTU:");

        foreach (var student in studentsFromNltu)
        {
            student.StudentInfo();
        }
    }

    public void StudentAndUniversityNameCollection()
    {
        var newCollection = from student in students
                            join university in universities
                            on student.UniversityId equals university.Id
                            orderby student.Name
                            select new { StudentName = student.Name, UniversityName = university.Name };
        System.Console.WriteLine("New collection:");
        foreach (var n in newCollection)
        {
            System.Console.WriteLine($"Student: {n.StudentName}, University: {n.UniversityName}");
        }
    }
}

class Student
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public int Age { get; set; }

    public int UniversityId { get; set; }

    public void StudentInfo()
    {
        Console.WriteLine($"Id: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}, UniversityId: {UniversityId}");
    }


}

class University
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public void UniversityInfo()
    {
        Console.WriteLine($"Id: {Id}, Name: {Name}");
    }
}