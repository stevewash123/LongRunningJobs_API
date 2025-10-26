# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY Backend/LongRunningJobs.Api/LongRunningJobs.Api.csproj Backend/LongRunningJobs.Api/
RUN dotnet restore Backend/LongRunningJobs.Api/LongRunningJobs.Api.csproj

# Copy source code and build
COPY Backend/LongRunningJobs.Api/ Backend/LongRunningJobs.Api/
RUN dotnet build Backend/LongRunningJobs.Api/LongRunningJobs.Api.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish Backend/LongRunningJobs.Api/LongRunningJobs.Api.csproj -c Release -o /app/publish

# Use the official .NET 8 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Expose port 8080 (Render's default)
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "LongRunningJobs.Api.dll"]