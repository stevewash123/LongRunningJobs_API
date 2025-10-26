@echo off
cd ..\Frontend\job-progress-app
echo Fixing Angular dependencies...
if exist node_modules rmdir /s /q node_modules
if exist package-lock.json del package-lock.json
npm cache clean --force
npm install
npm install @microsoft/signalr