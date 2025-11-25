# ?? Quick Start Guide - Enrollment System

## ? Database Setup Complete!

All three databases have been successfully created with LocalDB:

| Database | Tables | Sample Data |
|----------|--------|-------------|
| **EnrollmentAuth** | Users, RefreshTokens | ? 2 test users |
| **EnrollmentCourses** | Courses, Enrollments | ? 3 sample courses |
| **EnrollmentGrades** | Grades | ? Empty (ready for data) |

## ?? Test Accounts

### Student Account
- **Username:** `student1`
- **Password:** `password123`
- **Email:** student1@university.edu
- **Capabilities:** View courses, enroll, view grades

### Faculty Account
- **Username:** `faculty1`
- **Password:** `password123`
- **Email:** faculty1@university.edu
- **Capabilities:** Upload grades, view course grades

## ?? Sample Courses Available

1. **CS101 - Introduction to Programming**
   - Instructor: Dr. Smith
   - Capacity: 30 students
   - Status: Open for enrollment

2. **CS201 - Data Structures**
   - Instructor: Dr. Johnson
   - Capacity: 25 students
   - Status: Open for enrollment

3. **CS301 - Database Systems**
   - Instructor: Prof. Williams
   - Capacity: 20 students
   - Status: Open for enrollment

## ?? How to Start the System

### Option 1: Manual Start (4 Terminals)

**Terminal 1 - Auth Service (Port 5001):**
```powershell
cd src\EnrollmentSystem.AuthService
dotnet run
```

**Terminal 2 - Course Service (Port 5002):**
```powershell
cd src\EnrollmentSystem.CourseService
dotnet run
```

**Terminal 3 - Grade Service (Port 5003):**
```powershell
cd src\EnrollmentSystem.GradeService
dotnet run
```

**Terminal 4 - Frontend (Port 5176):**
```powershell
cd src\EnrollmentSystem.Frontend
dotnet run
```

### Option 2: Using PowerShell Script

Run the included script:
```powershell
.\start-all-services.ps1
```

This will automatically start all 4 services in separate windows.

## ?? Access the Application

Once all services are running:

### Main Application
- **Frontend:** http://localhost:5176
  - Login page
  - Course browsing and enrollment
  - Grade viewing

### API Documentation (Swagger)
- **Auth Service:** http://localhost:5001/swagger
- **Course Service:** http://localhost:5002/swagger
- **Grade Service:** http://localhost:5003/swagger

### Health Check Endpoints
- **Auth:** http://localhost:5001/health
- **Course:** http://localhost:5002/health
- **Grade:** http://localhost:5003/health
- **Frontend:** http://localhost:5176/health

## ?? Quick Test Workflow

### 1. Login as Student
1. Go to http://localhost:5176
2. Click "Login"
3. Enter: `student1` / `password123`
4. You'll be redirected to the home page

### 2. Browse and Enroll in Courses
1. Click "Courses" in the navigation
2. Browse available courses (CS101, CS201, CS301)
3. Click "Enroll" on any course
4. Verify enrollment success

### 3. View Grades (Initially Empty)
1. Click "My Grades" in navigation
2. You'll see "No grades available yet"

### 4. Upload Grades as Faculty
1. Logout (Click "Logout")
2. Login as: `faculty1` / `password123`
3. Use Swagger UI to upload grades:
   - Go to http://localhost:5003/swagger
   - Use POST `/api/grades/upload`
   - First, login via http://localhost:5001/swagger to get JWT token
   - Use the token in Grade Service Swagger

### 5. View Updated Grades as Student
1. Logout and login as `student1` again
2. Click "My Grades"
3. View the uploaded grades

## ?? Testing with .http Files

Visual Studio has built-in `.http` file support:

### 1. Test Auth Service
Open `src\EnrollmentSystem.AuthService\EnrollmentSystem.AuthService.http`

Click "Send Request" above:
```http
POST http://localhost:5001/api/auth/login
Content-Type: application/json

{
  "username": "student1",
  "password": "password123"
}
```

**Copy the token from response!**

### 2. Test Course Service
Open `src\EnrollmentSystem.CourseService\EnrollmentSystem.CourseService.http`

Replace `YOUR_TOKEN_HERE` with your JWT token:
```http
GET http://localhost:5002/api/courses
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 3. Test Grade Service
Open `src\EnrollmentSystem.GradeService\EnrollmentSystem.GradeService.http`

Same process - replace token and test endpoints.

## ?? Troubleshooting

### Database Connection Issues
```powershell
# Check if LocalDB is installed
sqllocaldb info

# Start LocalDB if needed
sqllocaldb start mssqllocaldb

# List databases
sqllocaldb info mssqllocaldb
```

### Port Already in Use
If ports 5001-5003 or 5176 are in use:
1. Edit `launchSettings.json` in each service's Properties folder
2. Change the port numbers
3. Update `appsettings.json` in Frontend to match new ports

### Service Not Starting
1. Check the terminal output for errors
2. Verify .NET 8 SDK is installed: `dotnet --version`
3. Rebuild the solution: `dotnet build EnrollmentSystem.sln`

### JWT Token Expired
Tokens expire after 60 minutes. Simply login again to get a new token.

## ?? Database Management

### View Data in Visual Studio
1. Open **SQL Server Object Explorer**
2. Navigate to **(localdb)\mssqllocaldb**
3. Expand **Databases**
4. Browse tables and data

### Reset All Data
```powershell
# Drop and recreate all databases
cd src\EnrollmentSystem.AuthService
dotnet ef database drop -f
dotnet ef database update

cd ..\EnrollmentSystem.CourseService
dotnet ef database drop -f
dotnet ef database update

cd ..\EnrollmentSystem.GradeService
dotnet ef database drop -f
dotnet ef database update
```

### Query Databases Directly
```sql
-- Check users
USE EnrollmentAuth;
SELECT * FROM Users;

-- Check courses
USE EnrollmentCourses;
SELECT * FROM Courses;
SELECT * FROM Enrollments;

-- Check grades
USE EnrollmentGrades;
SELECT * FROM Grades;
```

## ?? Demo Scenarios

### Scenario 1: Student Enrollment Flow
1. Login as student1
2. Browse courses ? CS101
3. Click "Enroll"
4. View "My Grades" (empty initially)

### Scenario 2: Faculty Grade Upload
1. Login as faculty1
2. Use Swagger to upload grade
3. Logout, login as student1
4. View grades - should see new grade

### Scenario 3: Fault Tolerance Test
1. Start all services
2. Login as student1
3. Stop Course Service (Ctrl+C in terminal)
4. Try to view courses ? See "Service Unavailable" message
5. Try to view grades ? Still works! (Different service)
6. Restart Course Service
7. Refresh courses page ? Works again!

## ?? Additional Resources

- **Swagger UI** - Interactive API testing
- **.http Files** - Quick endpoint testing
- **Health Checks** - Monitor service status
- **Polly Policies** - Automatic retry and circuit breaker

---

## ? You're All Set!

Your distributed enrollment system is ready to run. Start the services and begin testing! ??
