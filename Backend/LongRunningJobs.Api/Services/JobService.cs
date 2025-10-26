using Hangfire;
using Microsoft.AspNetCore.SignalR;
using LongRunningJobs.Api.Hubs;
using LongRunningJobs.Api.Models;

namespace LongRunningJobs.Api.Services;

public class JobService : IJobService
{
    private readonly IHubContext<JobProgressHub> _hubContext;
    private static readonly Dictionary<string, JobStatus> _activeJobs = new();

    public JobService(IHubContext<JobProgressHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task<List<string>> SubmitJobsAsync(List<JobConfiguration> jobs)
    {
        var jobIds = new List<string>();

        foreach (var job in jobs)
        {
            var jobId = Guid.NewGuid().ToString();
            jobIds.Add(jobId);

            // Create job status
            var jobStatus = new JobStatus
            {
                JobId = jobId,
                Name = job.Name,
                Status = job.ScheduleDelaySeconds > 0 ? "Scheduled" : "Queued",
                Progress = 0,
                StartTime = DateTime.UtcNow
            };

            _activeJobs[jobId] = jobStatus;

            // Enqueue the job with Hangfire (with optional delay)
            if (job.ScheduleDelaySeconds > 0)
            {
                BackgroundJob.Schedule(() => ExecuteLongRunningJobAsync(jobId, job.Name, job.DurationSeconds),
                    TimeSpan.FromSeconds(job.ScheduleDelaySeconds));
            }
            else
            {
                BackgroundJob.Enqueue(() => ExecuteLongRunningJobAsync(jobId, job.Name, job.DurationSeconds));
            }

            // Notify clients about new job
            var initialMessage = job.ScheduleDelaySeconds > 0
                ? $"Job scheduled to start in {job.ScheduleDelaySeconds} seconds"
                : "Job queued for execution";

            await _hubContext.Clients.Group("JobUpdates").SendAsync("JobStatusUpdate", new JobProgressUpdate
            {
                JobId = jobId,
                Name = job.Name,
                Progress = 0,
                Status = job.ScheduleDelaySeconds > 0 ? "Scheduled" : "Queued",
                Message = initialMessage
            });
        }

        return jobIds;
    }

    public Task<List<JobStatus>> GetActiveJobsAsync()
    {
        // Clean up completed jobs older than 1 hour
        var expiredJobs = _activeJobs.Where(j =>
            j.Value.Status == "Completed" &&
            j.Value.EndTime.HasValue &&
            j.Value.EndTime.Value < DateTime.UtcNow.AddHours(-1))
            .Select(j => j.Key)
            .ToList();

        foreach (var jobId in expiredJobs)
        {
            _activeJobs.Remove(jobId);
        }

        return Task.FromResult(_activeJobs.Values.ToList());
    }

    public async Task ExecuteLongRunningJobAsync(string jobId, string jobName, int durationSeconds)
    {
        try
        {
            // Update job status to running
            if (_activeJobs.TryGetValue(jobId, out var jobStatus))
            {
                jobStatus.Status = "Running";
                jobStatus.StartTime = DateTime.UtcNow;
            }

            await _hubContext.Clients.Group("JobUpdates").SendAsync("JobStatusUpdate", new JobProgressUpdate
            {
                JobId = jobId,
                Name = jobName,
                Progress = 0,
                Status = "Running",
                Message = $"Job started - will run for {durationSeconds} seconds"
            });

            // Simulate work with progress updates
            var totalSteps = durationSeconds / 3; // Update every 3 seconds
            for (int i = 0; i <= totalSteps; i++)
            {
                if (i < totalSteps)
                {
                    await Task.Delay(3000); // Wait 3 seconds
                }

                var progress = (int)((double)i / totalSteps * 100);

                // Update job status
                if (_activeJobs.TryGetValue(jobId, out var status))
                {
                    status.Progress = progress;
                }

                // Send progress update via SignalR
                await _hubContext.Clients.Group("JobUpdates").SendAsync("JobStatusUpdate", new JobProgressUpdate
                {
                    JobId = jobId,
                    Name = jobName,
                    Progress = progress,
                    Status = i == totalSteps ? "Completed" : "Running",
                    Message = i == totalSteps ? "Job completed successfully" : $"Processing step {i + 1} of {totalSteps + 1}"
                });
            }

            // Mark job as completed
            if (_activeJobs.TryGetValue(jobId, out var finalStatus))
            {
                finalStatus.Status = "Completed";
                finalStatus.Progress = 100;
                finalStatus.EndTime = DateTime.UtcNow;
            }
        }
        catch (Exception ex)
        {
            // Update job status to failed
            if (_activeJobs.TryGetValue(jobId, out var errorStatus))
            {
                errorStatus.Status = "Failed";
                errorStatus.ErrorMessage = ex.Message;
                errorStatus.EndTime = DateTime.UtcNow;
            }

            await _hubContext.Clients.Group("JobUpdates").SendAsync("JobStatusUpdate", new JobProgressUpdate
            {
                JobId = jobId,
                Name = jobName,
                Progress = 0,
                Status = "Failed",
                Message = $"Job failed: {ex.Message}"
            });
        }
    }
}