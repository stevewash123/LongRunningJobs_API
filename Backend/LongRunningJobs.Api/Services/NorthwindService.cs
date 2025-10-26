using LongRunningJobs.Api.Models;

namespace LongRunningJobs.Api.Services;

public class NorthwindService : INorthwindService
{
    private static readonly List<Product> _products = new()
    {
        new Product { ProductId = 1, ProductName = "Chai", Category = "Beverages", UnitPrice = 18.00m, UnitsInStock = 39, Discontinued = false },
        new Product { ProductId = 2, ProductName = "Chang", Category = "Beverages", UnitPrice = 19.00m, UnitsInStock = 17, Discontinued = false },
        new Product { ProductId = 3, ProductName = "Aniseed Syrup", Category = "Condiments", UnitPrice = 10.00m, UnitsInStock = 13, Discontinued = false },
        new Product { ProductId = 4, ProductName = "Chef Anton's Cajun Seasoning", Category = "Condiments", UnitPrice = 22.00m, UnitsInStock = 53, Discontinued = false },
        new Product { ProductId = 5, ProductName = "Chef Anton's Gumbo Mix", Category = "Condiments", UnitPrice = 21.35m, UnitsInStock = 0, Discontinued = true },
        new Product { ProductId = 6, ProductName = "Grandma's Boysenberry Spread", Category = "Condiments", UnitPrice = 25.00m, UnitsInStock = 120, Discontinued = false },
        new Product { ProductId = 7, ProductName = "Uncle Bob's Organic Dried Pears", Category = "Produce", UnitPrice = 30.00m, UnitsInStock = 15, Discontinued = false },
        new Product { ProductId = 8, ProductName = "Northwoods Cranberry Sauce", Category = "Condiments", UnitPrice = 40.00m, UnitsInStock = 6, Discontinued = false },
        new Product { ProductId = 9, ProductName = "Mishi Kobe Niku", Category = "Meat/Poultry", UnitPrice = 97.00m, UnitsInStock = 29, Discontinued = true },
        new Product { ProductId = 10, ProductName = "Ikura", Category = "Seafood", UnitPrice = 31.00m, UnitsInStock = 31, Discontinued = false },
        new Product { ProductId = 11, ProductName = "Queso Cabrales", Category = "Dairy Products", UnitPrice = 21.00m, UnitsInStock = 22, Discontinued = false },
        new Product { ProductId = 12, ProductName = "Queso Manchego La Pastora", Category = "Dairy Products", UnitPrice = 38.00m, UnitsInStock = 86, Discontinued = false },
        new Product { ProductId = 13, ProductName = "Konbu", Category = "Seafood", UnitPrice = 6.00m, UnitsInStock = 24, Discontinued = false },
        new Product { ProductId = 14, ProductName = "Tofu", Category = "Produce", UnitPrice = 23.25m, UnitsInStock = 35, Discontinued = false },
        new Product { ProductId = 15, ProductName = "Genen Shouyu", Category = "Condiments", UnitPrice = 15.50m, UnitsInStock = 39, Discontinued = false },
        new Product { ProductId = 16, ProductName = "Pavlova", Category = "Confections", UnitPrice = 17.45m, UnitsInStock = 29, Discontinued = false },
        new Product { ProductId = 17, ProductName = "Alice Mutton", Category = "Meat/Poultry", UnitPrice = 39.00m, UnitsInStock = 0, Discontinued = true },
        new Product { ProductId = 18, ProductName = "Carnarvon Tigers", Category = "Seafood", UnitPrice = 62.50m, UnitsInStock = 42, Discontinued = false },
        new Product { ProductId = 19, ProductName = "Teatime Chocolate Biscuits", Category = "Confections", UnitPrice = 9.20m, UnitsInStock = 25, Discontinued = false },
        new Product { ProductId = 20, ProductName = "Sir Rodney's Marmalade", Category = "Confections", UnitPrice = 81.00m, UnitsInStock = 40, Discontinued = false },
        new Product { ProductId = 21, ProductName = "Sir Rodney's Scones", Category = "Confections", UnitPrice = 10.00m, UnitsInStock = 3, Discontinued = false },
        new Product { ProductId = 22, ProductName = "Gustaf's Knäckebröd", Category = "Grains/Cereals", UnitPrice = 21.00m, UnitsInStock = 104, Discontinued = false },
        new Product { ProductId = 23, ProductName = "Tunnbröd", Category = "Grains/Cereals", UnitPrice = 9.00m, UnitsInStock = 61, Discontinued = false },
        new Product { ProductId = 24, ProductName = "Guaraná Fantástica", Category = "Beverages", UnitPrice = 4.50m, UnitsInStock = 20, Discontinued = true },
        new Product { ProductId = 25, ProductName = "NuNuCa Nuß-Nougat-Creme", Category = "Confections", UnitPrice = 14.00m, UnitsInStock = 76, Discontinued = false },
        new Product { ProductId = 26, ProductName = "Gumbär Gummibärchen", Category = "Confections", UnitPrice = 31.23m, UnitsInStock = 15, Discontinued = false },
        new Product { ProductId = 27, ProductName = "Schoggi Schokolade", Category = "Confections", UnitPrice = 43.90m, UnitsInStock = 49, Discontinued = false },
        new Product { ProductId = 28, ProductName = "Rössle Sauerkraut", Category = "Produce", UnitPrice = 45.60m, UnitsInStock = 26, Discontinued = false },
        new Product { ProductId = 29, ProductName = "Thüringer Rostbratwurst", Category = "Meat/Poultry", UnitPrice = 123.79m, UnitsInStock = 0, Discontinued = true },
        new Product { ProductId = 30, ProductName = "Nord-Ost Matjeshering", Category = "Seafood", UnitPrice = 25.89m, UnitsInStock = 10, Discontinued = false }
    };

    public async Task<ProductSearchResponse> SearchProductsAsync(ProductSearchRequest request)
    {
        // Simulate async operation
        await Task.Delay(50);

        var query = _products.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p => p.ProductName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        // Apply category filter
        if (!string.IsNullOrWhiteSpace(request.CategoryFilter) && request.CategoryFilter != "All Categories")
        {
            query = query.Where(p => p.Category == request.CategoryFilter);
        }

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var products = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new ProductSearchResponse
        {
            Products = products,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        // Simulate async operation
        await Task.Delay(10);

        return _products
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();
    }
}