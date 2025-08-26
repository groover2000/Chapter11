partial class Program
{
    static void SectionTitle(string title)
    {
        ConsoleColor previousColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("*");
        Console.WriteLine($"{title}");
        Console.WriteLine("*");
        Console.ForegroundColor = previousColor;
    }

    static void Output(IEnumerable<string> cohort, string description = "")
    {
        if (!string.IsNullOrEmpty(description))
        {
            Console.WriteLine(description);
        }
        Console.Write(" ");
        Console.WriteLine(string.Join(",", cohort.ToArray()));
        Console.WriteLine();
    }
}