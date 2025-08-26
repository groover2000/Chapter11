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

string[] cohort1 = ["Rachel", "Gareth", "Jonathan", "George"];

string[] cohort2 = ["Jack", "Stephen", "Daniel", "Jack", "Jared"];

string[] cohort3 = ["Declan", "Jack", "Jack", "Jasmine", "Conor"];

SectionTitle("The cohorts");

Output(cohort1, "Cohort 1");
Output(cohort2, "Cohort 2");
Output(cohort3, "Cohort 3");

SectionTitle("Set operations");

Output(cohort2.Distinct(), "cohort2.Disctinct()");
Output(cohort2.DistinctBy(name => name.Substring(0, 2)), "cohort2.DistinctBy(name => name.Substring(0,2))");
Output(cohort2.Union(cohort3), "cohort2.Union(cohort3)");
Output(cohort2.Concat(cohort3), "cohort2.Concat(cohort3)");
Output(cohort2.Intersect(cohort3), "cohort2.Intersect(cohort3)");
Output(cohort2.Except(cohort3), "cohort2.Except(cohort3)");
Output(cohort1.Zip(cohort2, (c1,c2) => $"{c1} matched with {c2}"), "cohort1.Zip(cohort2)");