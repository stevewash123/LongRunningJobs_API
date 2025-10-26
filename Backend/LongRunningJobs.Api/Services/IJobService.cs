using LongRunningJobs.Api.Models;

namespace LongRunningJobs.Api.Services;

public interface IJobService
{
    Task<List<string>> SubmitJobsAsync(List<JobConfiguration> jobs);
    Task<List<JobStatus>> GetActiveJobsAsync();
    Task ExecuteLongRunningJobAsync(string jobId, string jobName, int durationSeconds);
}