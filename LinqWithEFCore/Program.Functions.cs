
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

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

            Console.WriteLine($"{"Product count: ",-25} {db.Products.Count(), 10}");
            Console.WriteLine($"{"Discounted product count: ",-27} {db.Products.Count(product => product.Discontinued), 8}");
            Console.WriteLine($"{"Highest product count: ",-25} {db.Products.Max(p => p.UnitPrice), 10:$#,##0.00}");
            Console.WriteLine($"{"Sum of units in stock: ",-25} {db.Products.Sum(p => p.UnitsInStock), 10:N0}");
            Console.WriteLine($"{"Sum of units in order: ",-25} {db.Products.Sum(p => p.UnitsOnOrder), 10:N0}");
            Console.WriteLine($"{"Average unit price: ",-25} {db.Products.Average(p => p.UnitPrice), 10:$#,##0.00}");
            Console.WriteLine($"{"Value of units in stock: ",-25} {db.Products.Sum(p => p.UnitPrice * p.UnitsInStock), 10:$#,##0.00}");
        }
    }


}