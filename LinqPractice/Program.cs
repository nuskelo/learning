int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

OddNumbers(numbers);

static void OddNumbers(int[] numbers)
{
    var oddNumbers = from number in numbers where number % 2 != 0 select number;

    Console.WriteLine(oddNumbers);

    foreach (var n in oddNumbers)
    {
        Console.WriteLine(n);
    }
}