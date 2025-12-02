# Enrollment System - Distributed Microservices Application

A distributed online enrollment system built with .NET 8, demonstrating microservices architecture, fault tolerance, and JWT authentication.

##  Project Overview

This system implements a **distributed microservices architecture** for university enrollment management:
- **4 independent services** running on separate nodes (ports)
- **3 separate databases** for data isolation
- **JWT token authentication** with session tracking
- **Fault-tolerant design** with retry and circuit breaker patterns

## Features

### Core Requirements 
1. ?? **Login/Logout** - JWT authentication with token refresh
2. ?? **View Available Courses** - Browse course catalog with real-time capacity
3. ?? **Student Enrollment** - Enroll in courses with validation
4. ?? **View Grades** - Students view their academic history
5. ????? **Faculty Grade Upload** - Role-based grade management

### Technical Features 
- **Microservices Architecture** - Independent, scalable services
- **Distributed Databases** - SQL Server LocalDB per service
- **Health Checks** - Monitor service availability
- **API Documentation** - Swagger/OpenAPI on all services
- **Fault Tolerance** - Polly retry and circuit breaker policies
- **CORS Support** - Cross-origin resource sharing
- **Graceful Degradation** - Services fail independently

## Architecture

```
???????????????????????????????????????????????????????????
?          Frontend (Razor Pages - Port 5176)             ?
?         Cookie Auth + Polly Resilience Policies         ?
???????????????????????????????????????????????????????????
                ?              ?              ?
         HTTP + JWT     HTTP + JWT     HTTP + JWT
                ?              ?              ?
         ???????????? ???????????? ????????????
         ?   Auth      ? ?  Course   ? ?   Grade    ?
         ?  Service    ? ?  Service  ? ?  Service   ?
         ? (Port 5001) ? ? (Port 5002)? ? (Port 5003)?
         ???????????? ???????????? ????????????
                ?             ?               ?
         ???????????? ???????????? ????????????
         ? Auth DB     ? ? Course DB ? ? Grade DB   ?
         ? (LocalDB)   ? ? (LocalDB) ? ? (LocalDB)  ?
         ???????????? ???????????? ????????????
```

## Quick Start

### Prerequisites
- ? .NET 8 SDK
- ? SQL Server LocalDB (included with Visual Studio)
- ? Visual Studio 2022 or VS Code

### 1Database Setup (Already Done! ?)
Databases are already created with sample data:
- `EnrollmentAuth` - 2 test users
- `EnrollmentCourses` - 3 sample courses
- `EnrollmentGrades` - Ready for data

### 2 Start All Services

**Option A: PowerShell Script (Easiest)**
```powershell
.\start-all-services.ps1
```

**Option B: Manual (4 terminals)**
```powershell
# Terminal 1
cd src\EnrollmentSystem.AuthService
dotnet run

# Terminal 2
cd src\EnrollmentSystem.CourseService
dotnet run

# Terminal 3
cd src\EnrollmentSystem.GradeService
dotnet run

# Terminal 4
cd src\EnrollmentSystem.Frontend
dotnet run
```

### 3 Access the Application
- **Web App:** http://localhost:5176
- **Login:** `student1` / `password123`

## Documentation

- **[QUICK_START.md](QUICK_START.md)** - Detailed setup and testing guide
- **Swagger UI:**
  - Auth API: http://localhost:5001/swagger
  - Course API: http://localhost:5002/swagger
  - Grade API: http://localhost:5003/swagger

## Testing

### Test Accounts
| Role | Username | Password |
|------|----------|----------|
| Student | `student1` | `password123` |
| Faculty | `faculty1` | `password123` |

### Sample Courses
1. **CS101** - Introduction to Programming (Dr. Smith, 30 students)
2. **CS201** - Data Structures (Dr. Johnson, 25 students)
3. **CS301** - Database Systems (Prof. Williams, 20 students)

### Test Scenarios

#### 1. Student Workflow
```
1. Login as student1
2. Browse courses
3. Enroll in CS101
4. View grades (initially empty)
```

#### 2. Faculty Workflow
```
1. Login as faculty1
2. Use Swagger to upload grades
3. View course grades
```

#### 3. Fault Tolerance Test
```
1. Start all services
2. Stop Course Service
3. Verify other services still work
4. Restart Course Service
5. Verify full functionality restored
```

##  Technology Stack
### Backend Services
- **Framework:** ASP.NET Core 8.0
- **Authentication:** JWT Bearer Tokens
- **Database:** SQL Server LocalDB
- **ORM:** Entity Framework Core 8.0
- **API Docs:** Swashbuckle (Swagger/OpenAPI)

### Frontend
- **Framework:** ASP.NET Core Razor Pages
- **Authentication:** Cookie-based with JWT
- **HTTP Client:** Polly for resilience
- **UI:** Bootstrap 5

### DevOps
- **Health Checks:** ASP.NET Core Health Checks
- **API Testing:** .http files (REST Client)
- **Logging:** Built-in ASP.NET Core logging

##  Project Structure

```
EnrollmentSystem/
? src/
?   ? EnrollmentSystem.AuthService/      # Authentication & JWT
?   ?   ? Controllers/                   # Auth endpoints
?   ?   ? Services/                      # Token & auth services
?   ?   ? Data/                          # Auth database context
?   ?   ? Models/                        # User & token models
?   ?
?   ? EnrollmentSystem.CourseService/    # Course management
?   ?   ? Controllers/                   # Course endpoints
?   ?   ? Data/                          # Course database
?   ?   ? Models/                        # Course & enrollment models
?   ?
?   ? EnrollmentSystem.GradeService/     # Grade management
?   ?   ? Controllers/                   # Grade endpoints
?   ?   ? Data/                          # Grade database
?   ?   ? Models/                        # Grade models
?   ?
?   ? EnrollmentSystem.Frontend/         # Web UI
?   ?   ? Pages/                         # Razor Pages
?   ?   ? Services/                      # HTTP clients
?   ?   ? wwwroot/                       # Static files
?   ?
?   ? EnrollmentSystem.Shared/           # Shared DTOs
?       ? Models/                        # Data transfer objects
?
? start-all-services.ps1                 # Start script
? stop-all-services.ps1                  # Stop script
? QUICK_START.md                         # User guide
? README.md                              # This file
```

## Security Features

- **JWT Authentication** - Stateless token-based auth
- **Role-Based Authorization** - Student vs Faculty permissions
- **Password Hashing** - BCrypt with salt
- **HTTPS Redirect** - Force secure connections
- **CORS Policy** - Controlled cross-origin access
- **Token Expiration** - 60-minute token lifetime

## Fault Tolerance

The system demonstrates fault tolerance through:

1. **Independent Services** - Each service can fail without affecting others
2. **Retry Policies** - Automatic retry on transient failures (3 retries with exponential backoff)
3. **Circuit Breaker** - Prevent cascading failures (opens after 5 failures for 30 seconds)
4. **Graceful Degradation** - UI shows service-specific error messages
5. **Health Checks** - Monitor and report service status

### Example: Course Service Down
-  Cannot view or enroll in courses
-  Can still login/logout (Auth Service)
-  Can still view grades (Grade Service)
-  Frontend shows "Service Unavailable" message

##  Database Schema

### EnrollmentAuth
```sql
Users           (Id, Username, PasswordHash, Email, Role, CreatedAt)
RefreshTokens   (Id, UserId, Token, ExpiresAt, CreatedAt, IsRevoked)
```

### EnrollmentCourses
```sql
Courses         (Id, Code, Name, Description, MaxStudents, IsOpen, Instructor)
Enrollments     (Id, StudentId, CourseId, EnrolledAt)
```

### EnrollmentGrades
```sql
Grades          (Id, StudentId, CourseId, CourseName, CourseCode, 
                 NumericGrade, LetterGrade, Semester, DatePosted)
```

## ?? API Endpoints

### Auth Service (Port 5001)
- `POST /api/auth/login` - Login and get JWT token
- `POST /api/auth/logout` - Revoke token
- `GET /api/auth/validate` - Validate token
- `GET /health` - Service health check

### Course Service (Port 5002)
- `GET /api/courses` - List all courses
- `GET /api/courses/{id}` - Get course details
- `GET /api/courses/student/{studentId}` - Get student's enrolled courses
- `POST /api/courses/enroll` - Enroll in a course
- `GET /health` - Service health check

### Grade Service (Port 5003)
- `GET /api/grades/student/{studentId}` - Get student grades
- `GET /api/grades/course/{courseId}` - Get course grades (Faculty only)
- `POST /api/grades/upload` - Upload grade (Faculty only)
- `GET /health` - Service health check

## ?? Configuration

All services use `appsettings.json` for configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DbName;..."
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "Issuer": "EnrollmentSystem.AuthService",
    "Audience": "EnrollmentSystem.Clients",
    "ExpirationMinutes": 60
  }
}
```

##  Troubleshooting

### Database Connection Failed
```powershell
# Check LocalDB status
sqllocaldb info

# Start LocalDB
sqllocaldb start mssqllocaldb
```

### Port Already in Use
Edit `Properties/launchSettings.json` in each service to change ports.

### Build Errors
```powershell
# Clean and rebuild
dotnet clean
dotnet build
```

##  Academic Requirements Met

### Distributed Systems Concepts
**Multiple Nodes** - 4 independent services  
**Fault Tolerance** - Services fail independently  
**Session Management** - JWT tokens across nodes  
**Distributed Data** - 3 separate databases  
**Inter-Service Communication** - HTTP/REST  
**Load Distribution** - Independent service scaling  

### Bonus Features
**Redundant Persistence** - Separate DB per service  
**Health Monitoring** - Health check endpoints  
**API Documentation** - Swagger/OpenAPI  
**Resilience Patterns** - Retry & Circuit Breaker  

## License

This project is for educational purposes as part of a distributed systems course.

## Contributors

Developed as a course project demonstrating microservices architecture and distributed systems concepts.

---

## You're Ready to Go!

Run `.\start-all-services.ps1` and start testing! 

For detailed testing instructions, see **[QUICK_START.md](QUICK_START.md)**
