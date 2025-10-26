using Microsoft.AspNetCore.Mvc;
using LongRunningJobs.Api.Models;
using LongRunningJobs.Api.Services;

namespace LongRunningJobs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    /// <summary>
    /// Submit multiple background jobs for execution
    /// </summary>
    [HttpPost("submit")]
    public async Task<ActionResult<List<string>>> SubmitJobs([FromBody] JobSubmissionRequest request)
    {
        try
        {
            if (request?.Jobs == null || !request.Jobs.Any())
            {
                return BadRequest("At least one job configuration is required");
            }

            // Validate job configurations
            foreach (var job in request.Jobs)
            {
                if (string.IsNullOrWhiteSpace(job.Name))
                {
                    return BadRequest("Job name is required");
                }

                if (job.DurationSeconds < 10 || job.DurationSeconds > 60)
                {
                    return BadRequest("Job duration must be between 10 and 60 seconds");
                }

                if (job.ScheduleDelaySeconds < 0 || job.ScheduleDelaySeconds > 30)
                {
                    return BadRequest("Schedule delay must be between 0 and 30 seconds");
                }
            }

            var jobIds = await _jobService.SubmitJobsAsync(request.Jobs);
            return Ok(new { JobIds = jobIds, Message = $"Successfully submitted {jobIds.Count} jobs" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Failed to submit jobs", Details = ex.Message });
        }
    }

    /// <summary>
    /// Get status of all active jobs
    /// </summary>
    [HttpGet("status")]
    public async Task<ActionResult<List<JobStatus>>> GetJobStatus()
    {
        try
        {
            var jobs = await _jobService.GetActiveJobsAsync();
            return Ok(jobs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Failed to get job status", Details = ex.Message });
        }
    }
}