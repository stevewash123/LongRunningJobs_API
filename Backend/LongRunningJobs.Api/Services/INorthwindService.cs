using LongRunningJobs.Api.Models;

namespace LongRunningJobs.Api.Services;

public interface INorthwindService
{
    Task<ProductSearchResponse> SearchProductsAsync(ProductSearchRequest request);
    Task<List<string>> GetCategoriesAsync();
}