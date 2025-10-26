#!/bin/bash
echo "=========================================="
echo "   Long Running Jobs Demo - Full Stack Launcher"
echo "=========================================="
echo ""
echo "API: http://localhost:5042"
echo "Hangfire Dashboard: http://localhost:5042/hangfire"
echo "Frontend: http://localhost:4200"
echo ""

echo "Starting API server in background..."
cd ../Backend/LongRunningJobs.Api
dotnet run --urls=http://localhost:5042 &
API_PID=$!

echo ""
echo "Waiting for API to initialize..."
sleep 5

echo ""
echo "Starting Angular frontend..."
cd ../../Frontend/job-progress-app
npm start

# Kill the API process when frontend exits
kill $API_PID 2>/dev/null