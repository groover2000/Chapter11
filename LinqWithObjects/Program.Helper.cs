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
}