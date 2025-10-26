using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using LongRunningJobs.Api.Hubs;
using LongRunningJobs.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure port for Render.com deployment
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                  "http://localhost:4200",                           // Local development
                  "https://longrunningjobs-ui.onrender.com"          // Production frontend
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add SignalR
builder.Services.AddSignalR();

// Add Hangfire services
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMemoryStorage());

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

// Add custom services
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<INorthwindService, NorthwindService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseRouting();
app.UseAuthorization();

// Add Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.MapControllers();
app.MapHub<JobProgressHub>("/jobProgressHub");

app.Run();

// Custom authorization filter for Hangfire Dashboard (allows all in development)
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true; // Allow all users in demo
    }
}
