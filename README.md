# Long Running Jobs Demo

**Enterprise Background Job Processing with Real-Time Updates**

A comprehensive demonstration of Hangfire background job processing with SignalR real-time updates and non-blocking UI interaction.

## 🚀 Quick Start

**See [Full-Stack Setup Guide](../FULL-STACK-SETUP-GUIDE.md)** for standardized C#/Angular setup patterns and **critical Windows/WSL debugging considerations**.

**Run the complete application:**
```cmd
.\Scripts\launch-both.bat
```

**If Angular issues occur:**
```cmd
Scripts\fix-angular.bat
.\Scripts\launch-both.bat
```

This will start:
- **API**: http://localhost:7000 (with Swagger at /swagger)
- **Hangfire Dashboard**: http://localhost:7000/hangfire
- **Frontend**: http://localhost:4200 (opens automatically)

## 🚀 Features

### **Background Job Processing**
- **Hangfire Integration**: Enterprise-grade background job scheduler
- **Real-Time Updates**: SignalR for live progress monitoring
- **Non-Blocking UI**: Data table remains fully interactive during job execution
- **Configurable Jobs**: Dynamic job count and duration settings
- **Progress Tracking**: Visual progress bars with percentage completion

### **Technology Stack**
- **Backend**: C# ASP.NET Core 8 with Hangfire and SignalR
- **Frontend**: Angular 18 with SignalR client integration
- **Database**: In-memory for demo (Northwind Products)
- **Real-Time**: SignalR hubs for job status broadcasts

## 🏗️ Architecture

### **Job Processing Pipeline**
1. **Job Submission**: Frontend submits job configurations to API
2. **Hangfire Scheduling**: API queues jobs with Hangfire
3. **Background Execution**: Jobs run in background with progress reporting
4. **SignalR Updates**: Real-time progress broadcast to connected clients
5. **Non-Blocking UI**: Users can interact with data table while jobs run

### **Key Components**
- **Hangfire Dashboard**: Built-in job monitoring and management
- **SignalR Hubs**: Real-time communication between server and clients
- **Progress Tracking**: Jobs report completion percentage
- **Northwind Data**: Sample product catalog for UI interaction testing

## 🎯 Portfolio Value

### **Enterprise Patterns Demonstrated**
- **Background Processing**: Long-running tasks without blocking user interface
- **Real-Time Communication**: WebSocket-based updates with SignalR
- **Scalable Architecture**: Hangfire job queuing and processing
- **Production Monitoring**: Built-in dashboards and logging

### **Technical Skills Showcased**
- **Async Programming**: Non-blocking job execution patterns
- **WebSocket Communication**: Real-time bidirectional communication
- **Job Scheduling**: Enterprise background job management
- **UI/UX Design**: Responsive interface with live updates

## 📋 Implementation Plan

Based on the wireframe specifications:

### **Job Configuration Panel**
- Dynamic job count selection (1-10 jobs)
- Individual duration settings (30-300 seconds in 30s intervals)
- Real-time job status display with progress bars
- Start/stop job controls

### **Data Interaction Panel**
- Northwind Products table with search and filtering
- Pagination and sorting capabilities
- Fully functional while background jobs execute
- Demonstrates non-blocking UI architecture

### **Real-Time Status Updates**
- Live progress bars updating via SignalR
- Job completion notifications
- Status changes (Running, Completed, Failed)
- Connection status indicators

---

**See [Full-Stack Setup Guide](../FULL-STACK-SETUP-GUIDE.md)** for standardized C#/Angular development patterns.

*Part of a portfolio demonstrating enterprise-level background processing and real-time communication patterns.*