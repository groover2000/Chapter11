string[] names = ["Michael", "Pam", "Jim", "Dwight", "Angel", "Kevin", "Toby", "Creed"];

SectionTitle("Deffered execution");

var query1 = names.Where(name => name.EndsWith("m"));


var query2 = from name in names where name.EndsWith("m") select name;

// Запрос выполняться не будет пока его не вызвать например


string[]? names1 = query1?.ToArray();

List<string>? names2 = query2?.ToList();
if (names1 is not null)
{
    foreach (string name in names1)
    {
        Console.WriteLine(name);
    }
}
Console.WriteLine(new string('-', 50));

var query = names.Where(new Func<string, bool>(NameLongerThenFour))
.OrderBy(name => name.Length)
.ThenBy(name => name);


foreach (string item in query)
{
    Console.WriteLine(item);
}

SectionTitle("Filtering by type");

List<Exception> exceptions = [

    new ArgumentException(),
    new SystemException(),
    new IndexOutOfRangeException(),
    new InvalidOperationException(),
    new NullReferenceException(),
    new InvalidCastException(),
    new OverflowException(),
    new DivideByZeroException(),
    new ApplicationException()

];
IEnumerable<ArithmeticException> arithmeticExceptionsQuery = exceptions.OfType<ArithmeticException>();

foreach (var exception in arithmeticExceptionsQuery)
{
    Console.WriteLine(exception);
}