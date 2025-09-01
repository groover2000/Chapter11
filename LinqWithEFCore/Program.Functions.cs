
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Linq;


partial class Program
{
    static void FilterAndSort()
    {
        SectionTitle("Filter and sort");

        using (Northwind db = new())
        {
            var filteredProducts = db.Products.Where(product => product.UnitPrice < 10);
            var sortedAndFilteredProducts = filteredProducts.OrderByDescending(product => product.UnitPrice);
            var projectedProducts = sortedAndFilteredProducts.Select(product => new
            {
                product.ProductId,
                product.ProductName,
                product.UnitPrice
            }); //Анонимный объект новый


            Console.WriteLine(projectedProducts.ToQueryString());

            Console.WriteLine("Products that cost less then 10$: ");

            foreach (var p in projectedProducts)
            {
                Console.WriteLine($"{p.ProductId}: {p.ProductName}, costs {p.UnitPrice:$#,##0.00}");
            }
            Console.WriteLine();
        }
    }
    static void JoinCategoriesAndProducts()
    {
        SectionTitle("Join categories and products");

        using (Northwind db = new())
        {
            var queryJoin = db.Categories.Join(
                inner: db.Products,
                outerKeySelector: category => category.CategoryId,
                innerKeySelector: product => product.CategoryId,
                resultSelector: (c, p) => new { c.CategoryName, p.ProductName, p.ProductId });

            Console.WriteLine(queryJoin.ToQueryString());

            foreach (var item in queryJoin)
            {
                Console.WriteLine($"{item.ProductId}: {item.ProductName} is in {item.CategoryName}");
            }
        }
    }
    static void GroupJoinCategoriesAndProducts()
    {
        SectionTitle("Group join categories and products");

        using (Northwind db = new())
        {
            var queryGroup = db.Categories.AsEnumerable().GroupJoin(
                inner: db.Products,
                outerKeySelector: category => category.CategoryId,
                innerKeySelector: products => products.CategoryId,
                resultSelector: (c, matchingProducts) => new
                {
                    c.CategoryName,
                    Products = matchingProducts.OrderBy(p => p.ProductName)
                });

            foreach (var category in queryGroup)
            {
                Console.WriteLine($"{category.CategoryName} has {category.Products.Count()} products ");

                foreach (var product in category.Products)
                {
                    Console.WriteLine($"    {product.ProductName}");
                }
            }
        }
    }

    static void AggregateProducts()
    {
        SectionTitle("Aggregate products");

        using (Northwind db = new())
        {
            if (db.Products.TryGetNonEnumeratedCount(out int countDbSet))
            {
                Console.WriteLine($"{"Product count from DbSet:",-25} {countDbSet,10}");

            }
            else
            {
                Console.WriteLine("Products DbSet does not have a Count property");
            }

            List<Product> products = db.Products.ToList();

            if (products.TryGetNonEnumeratedCount(out int countList))
            {
                Console.WriteLine($"{"Products count from list:",-25} {countList}");
            }
            else
            {
                Console.WriteLine("Products List does not have a Count property");
            }

            Console.WriteLine($"{"Product count: ",-25} {db.Products.Count(),10}");
            Console.WriteLine($"{"Discounted product count: ",-27} {db.Products.Count(product => product.Discontinued),8}");
            Console.WriteLine($"{"Highest product count: ",-25} {db.Products.Max(p => p.UnitPrice),10:$#,##0.00}");
            Console.WriteLine($"{"Sum of units in stock: ",-25} {db.Products.Sum(p => p.UnitsInStock),10:N0}");
            Console.WriteLine($"{"Sum of units in order: ",-25} {db.Products.Sum(p => p.UnitsOnOrder),10:N0}");
            Console.WriteLine($"{"Average unit price: ",-25} {db.Products.Average(p => p.UnitPrice),10:$#,##0.00}");
            Console.WriteLine($"{"Value of units in stock: ",-25} {db.Products.Sum(p => p.UnitPrice * p.UnitsInStock),10:$#,##0.00}");
        }
    }

    static void OutputTableOfProducts(Product[] products, int currentPage, int totalPages)
    {
        string line = new('-', 73);
        string lineHalf = new('-', 30);

        Console.WriteLine(line);
        Console.WriteLine($"{"ID",4} {"Product Name",-40} {"Unit Price",12} {"Discontinued",-15}");

        foreach (Product p in products)
        {
            Console.WriteLine($"{p.ProductId,4} {p.ProductName,-40} {p.UnitPrice,12} {p.Discontinued,-15}");
        }

        Console.WriteLine($"{lineHalf} Page {currentPage + 1} of {totalPages + 1} {lineHalf}");

    }

    static void OutputPageOfProducts(IQueryable<Product> products, int pageSize, int currentPage, int totalPages)
    {
        var pagingQuery = products.OrderBy(p => p.ProductId).Skip(currentPage * pageSize).Take(pageSize);

        SectionTitle(pagingQuery.ToQueryString());

        OutputTableOfProducts(pagingQuery.ToArray(), currentPage, totalPages);
    }

    static void PagingProducts()
    {
        SectionTitle("Paging products");

        using (Northwind db = new())
        {
            int pageSize = 10;
            int currentPage = 0;
            int productCount = db.Products.Count();
            int totalPages = productCount / pageSize;

            while (true)
            {
                OutputPageOfProducts(db.Products, pageSize, currentPage, totalPages);

                Console.Write("Press <- to page back, press -> to page forward, any key to exit.");
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    if (currentPage == 0)
                        currentPage = totalPages;
                    else
                        currentPage--;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    if (currentPage == totalPages)
                        currentPage = 0;
                    else
                        currentPage++;
                }
                else
                    break;

                Console.WriteLine();
            }
        }
    }

    static void CustomExtensionsMethods()
    {
        SectionTitle("Custom aggregate extension methods");

        using (Northwind db = new())
        {
            Console.WriteLine("{0,-25} {1,10:N0}",
            "Mean units in stock:",
            db.Products.Average(p => p.UnitsInStock));
            Console.WriteLine("{0,-25} {1,10:$#,##0.00}",
            "Mean unit price:",
            db.Products.Average(p => p.UnitPrice));

            Console.WriteLine("{0,-25} {1,10:N0}",
            "Median units in stock:",
            db.Products.Median(p => p.UnitsInStock));

            Console.WriteLine("{0,-25} {1,10:$#,##0.00}",
            "Median unit price:",
            db.Products.Median(p => p.UnitPrice));

            Console.WriteLine("{0,-25} {1,10:N0}",
            "Mode units in stock:",
            db.Products.Mode(p => p.UnitsInStock));

            Console.WriteLine("{0,-25} {1,10:$#,##0.00}",
            "Mode unit price:",
            db.Products.Mode(p => p.UnitPrice));

        }
    }

    static void OutputProductsAsXml()
    {
        SectionTitle("Output products as XML");

        using (Northwind db = new())
        {
            Product[] productsArray = db.Products.ToArray();

            XElement xml = new("products", from p in productsArray
                                           select new XElement("product", new XAttribute("id", p.ProductId),
                                           new XAttribute("price", p.UnitPrice), new XElement("name", p.ProductName)));

            Console.WriteLine(xml.ToString());
        }
    }
    static void ProcessSettings()
    {
        string path = Path.Combine(Environment.CurrentDirectory, "settings.xml");

        Console.WriteLine($"Settings file path: {path}");

        XDocument doc = XDocument.Load(path);
        var appSettings = doc.Descendants("appSettings")
        .Descendants("add")
        .Select(node => new
        {
            Key = node.Attribute("key")?.Value,
            Value = node.Attribute("value")?.Value
        }).ToArray();

        foreach (var item in appSettings)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }
    }
}