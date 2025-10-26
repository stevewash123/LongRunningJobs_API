@echo off
cd ..\Backend\LongRunningJobs.Api
echo Starting Long Running Jobs API server...
dotnet run --urls=http://localhost:5042