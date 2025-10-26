@echo off
echo ==========================================
echo    Long Running Jobs Demo - Full Stack Launcher
echo ==========================================
echo.
echo API: http://localhost:5042
echo Hangfire Dashboard: http://localhost:5042/hangfire
echo Frontend: http://localhost:4200
echo.

echo Starting API server in background...
start "API Server" cmd /c "cd ..\Backend\LongRunningJobs.Api && dotnet run --urls=http://localhost:5042"

echo.
echo Waiting for API to initialize...
timeout /t 5 >nul

echo.
echo Starting Angular frontend...
cd ..\Frontend\job-progress-app
npm start