# Enrollment System - Start All Services
# This script starts all microservices in separate PowerShell windows

Write-Host "Starting Enrollment System Services..." -ForegroundColor Green
Write-Host ""

# Get the current directory (should be the solution root)
$rootPath = Get-Location

# Start Auth Service (Port 5001)
Write-Host "Starting Auth Service (Port 5001)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", `
    "Set-Location '$rootPath\src\EnrollmentSystem.AuthService'; Write-Host ' Auth Service Starting...' -ForegroundColor Yellow; dotnet run"

Start-Sleep -Seconds 2

# Start Course Service (Port 5002)
Write-Host "Starting Course Service (Port 5002)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", `
    "Set-Location '$rootPath\src\EnrollmentSystem.CourseService'; Write-Host ' Course Service Starting...' -ForegroundColor Yellow; dotnet run"

Start-Sleep -Seconds 2

# Start Grade Service (Port 5003)
Write-Host "Starting Grade Service (Port 5003)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", `
    "Set-Location '$rootPath\src\EnrollmentSystem.GradeService'; Write-Host 'Grade Service Starting...' -ForegroundColor Yellow; dotnet run"

Start-Sleep -Seconds 2

# Start Frontend (Port 5176)
Write-Host "Starting Frontend Web App (Port 5176)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", `
    "Set-Location '$rootPath\src\EnrollmentSystem.Frontend'; Write-Host ' Frontend Starting...' -ForegroundColor Yellow; dotnet run"

Start-Sleep -Seconds 3

Write-Host ""
Write-Host "? All services are starting up..." -ForegroundColor Green
Write-Host ""
Write-Host " Service URLs:" -ForegroundColor White
Write-Host "   Frontend:   http://localhost:5176" -ForegroundColor Yellow
Write-Host "   Auth API:   http://localhost:5001/swagger" -ForegroundColor Yellow
Write-Host "   Course API: http://localhost:5002/swagger" -ForegroundColor Yellow
Write-Host "   Grade API:  http://localhost:5003/swagger" -ForegroundColor Yellow
Write-Host ""
Write-Host "? Wait 10-15 seconds for all services to fully start..." -ForegroundColor Cyan
Write-Host ""
Write-Host " Test Accounts:" -ForegroundColor White
Write-Host "  Student: student1 / password123" -ForegroundColor Green
Write-Host "   Faculty: faculty1 / password123" -ForegroundColor Green
Write-Host ""
Write-Host "Press any key to exit this window..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
