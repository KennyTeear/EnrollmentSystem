# Setup databases with proper dotnet-ef commands
Write-Host "=== Enrollment System Database Setup ===" -ForegroundColor Cyan
Write-Host ""

# Step 1: Restore packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore EnrollmentSystem.sln
Write-Host ""

# Step 2: Install dotnet-ef globally if not exists
Write-Host "Checking for Entity Framework Core tools..." -ForegroundColor Yellow
$efCheck = dotnet tool list -g | Select-String "dotnet-ef"
if (-not $efCheck) {
    Write-Host "Installing dotnet-ef globally..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef --version 8.0.0
} else {
    Write-Host "✓ dotnet-ef already installed" -ForegroundColor Green
}
Write-Host ""

# Step 3: Auth Service
Write-Host "Setting up Auth Service database..." -ForegroundColor Green
Set-Location -Path "src\EnrollmentSystem.AuthService"
dotnet ef migrations add InitialCreate --context AuthDbContext --force
if ($LASTEXITCODE -eq 0) {
    dotnet ef database update --context AuthDbContext
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Auth Service database created successfully" -ForegroundColor Green
    } else {
        Write-Host "✗ Auth Service database update failed" -ForegroundColor Red
    }
} else {
    Write-Host "✗ Auth Service migration failed" -ForegroundColor Red
}
Set-Location -Path "..\..\"

Write-Host ""

# Step 4: Course Service
Write-Host "Setting up Course Service database..." -ForegroundColor Green
Set-Location -Path "src\EnrollmentSystem.CourseService"
dotnet ef migrations add InitialCreate --context CourseDbContext --force
if ($LASTEXITCODE -eq 0) {
    dotnet ef database update --context CourseDbContext
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Course Service database created successfully" -ForegroundColor Green
    } else {
        Write-Host "✗ Course Service database update failed" -ForegroundColor Red
    }
} else {
    Write-Host "✗ Course Service migration failed" -ForegroundColor Red
}
Set-Location -Path "..\..\"

Write-Host ""

# Step 5: Grade Service
Write-Host "Setting up Grade Service database..." -ForegroundColor Green
Set-Location -Path "src\EnrollmentSystem.GradeService"
dotnet ef migrations add InitialCreate --context GradeDbContext --force
if ($LASTEXITCODE -eq 0) {
    dotnet ef database update --context GradeDbContext
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Grade Service database created successfully" -ForegroundColor Green
    } else {
        Write-Host "✗ Grade Service database update failed" -ForegroundColor Red
    }
} else {
    Write-Host "✗ Grade Service migration failed" -ForegroundColor Red
}
Set-Location -Path "..\..\"

Write-Host ""
Write-Host "=== Database Setup Complete! ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Test Accounts:" -ForegroundColor Yellow
Write-Host "  Student: username='student1', password='password123'" -ForegroundColor White
Write-Host "  Faculty: username='faculty1', password='password123'" -ForegroundColor White
Write-Host ""
Write-Host "Databases Created:" -ForegroundColor Yellow
Write-Host "  - EnrollmentAuth (2 users seeded)" -ForegroundColor White
Write-Host "  - EnrollmentCourses (3 courses seeded)" -ForegroundColor White
Write-Host "  - EnrollmentGrades (empty - ready for grade uploads)" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Run multiple startup projects in Visual Studio" -ForegroundColor White
Write-Host "  2. Or use: dotnet run --project src\EnrollmentSystem.Frontend" -ForegroundColor White