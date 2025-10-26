namespace LongRunningJobs.Api.Models;

public class JobSubmissionRequest
{
    public List<JobConfiguration> Jobs { get; set; } = new();
}

public class JobConfiguration
{
    public string Name { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
    public int ScheduleDelaySeconds { get; set; } = 0; // Delay before job starts (0 = immediate)
}

public class JobStatus
{
    public string JobId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Progress { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? ErrorMessage { get; set; }
}

public class JobProgressUpdate
{
    public string JobId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Progress { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Message { get; set; }
}

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
    public bool Discontinued { get; set; }
}

public class ProductSearchRequest
{
    public string? SearchTerm { get; set; }
    public string? CategoryFilter { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class ProductSearchResponse
{
    public List<Product> Products { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}