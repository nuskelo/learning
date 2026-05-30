UniversityManager um = new UniversityManager();
um.FemaleStudents();
um.SortStudentsByAge();
um.AllStudentsFromNltu();
um.StudentAndUniversityNameCollection();

int[] numbers = { 1, 5, 3, 2, 0, 10, 15, 8 };

var sortedNumbers = from number in numbers orderby number select number;
System.Console.WriteLine("Sorted numbers:");
foreach (var number in sortedNumbers)
{
    System.Console.WriteLine(number);
}

var reversedNumbers = sortedNumbers.Reverse();
System.Console.WriteLine("Reversed numbers:");
foreach (var number in reversedNumbers)
{
    System.Console.WriteLine(number);
}

var sortedByDescendingNumbers = from number in numbers orderby number descending select number;
System.Console.WriteLine("Sorted by descending numbers:");
foreach (var number in sortedByDescendingNumbers)
{
    System.Console.WriteLine(number);
}