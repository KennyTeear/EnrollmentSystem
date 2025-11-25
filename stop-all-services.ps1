# Enrollment System - Stop All Services
# This script stops all running .NET processes for the enrollment system

Write-Host "?? Stopping Enrollment System Services..." -ForegroundColor Red
Write-Host ""

# Find and stop all dotnet processes running our services
$processes = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue

if ($processes) {
    Write-Host "Found $($processes.Count) dotnet process(es)" -ForegroundColor Yellow
    
    foreach ($process in $processes) {
        try {
            # Try to get the process command line to identify our services
            $commandLine = (Get-WmiObject Win32_Process -Filter "ProcessId = $($process.Id)").CommandLine
            
            if ($commandLine -like "*EnrollmentSystem*") {
                Write-Host "Stopping: $commandLine" -ForegroundColor Cyan
                Stop-Process -Id $process.Id -Force
                Write-Host "? Stopped process $($process.Id)" -ForegroundColor Green
            }
        }
        catch {
            Write-Host "?? Could not stop process $($process.Id)" -ForegroundColor Yellow
        }
    }
    
    Write-Host ""
    Write-Host "? All Enrollment System services stopped" -ForegroundColor Green
}
else {
    Write-Host "No running dotnet processes found" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
