# Reorganize to EnrollmentSystem Directory
# This script moves everything from P4 to a cleaner EnrollmentSystem directory

Write-Host "?? Reorganizing Project Structure..." -ForegroundColor Cyan
Write-Host ""

$sourceDir = "C:\Users\kendr\OneDrive\Desktop\STDISCM\P4"
$targetDir = "C:\Users\kendr\OneDrive\Desktop\STDISCM\EnrollmentSystem"

# Check if source exists
if (-not (Test-Path $sourceDir)) {
    Write-Host "? Source directory not found: $sourceDir" -ForegroundColor Red
    exit
}

# Check if target already exists
if (Test-Path $targetDir) {
    Write-Host "??  Target directory already exists: $targetDir" -ForegroundColor Yellow
    $response = Read-Host "Do you want to delete it and recreate? (yes/no)"
    if ($response -ne "yes") {
        Write-Host "? Operation cancelled" -ForegroundColor Red
        exit
    }
    Remove-Item $targetDir -Recurse -Force
    Write-Host "???  Removed existing directory" -ForegroundColor Yellow
}

Write-Host "?? Creating new directory structure..." -ForegroundColor Cyan

# Create target directory
New-Item -ItemType Directory -Path $targetDir -Force | Out-Null

# Copy everything except P4-specific files
Write-Host "?? Copying files..." -ForegroundColor Cyan

Get-ChildItem $sourceDir -Exclude "P4.csproj","P4.csproj.user","P4.sln","Program.cs","bin","obj",".vs" | 
    Copy-Item -Destination $targetDir -Recurse -Force

Write-Host ""
Write-Host "? Project reorganized successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "?? New structure:" -ForegroundColor White
Write-Host "   $targetDir" -ForegroundColor Yellow
Write-Host "   ??? EnrollmentSystem.sln" -ForegroundColor Gray
Write-Host "   ??? src\" -ForegroundColor Gray
Write-Host "   ?   ??? EnrollmentSystem.AuthService\" -ForegroundColor Gray
Write-Host "   ?   ??? EnrollmentSystem.CourseService\" -ForegroundColor Gray
Write-Host "   ?   ??? EnrollmentSystem.GradeService\" -ForegroundColor Gray
Write-Host "   ?   ??? EnrollmentSystem.Frontend\" -ForegroundColor Gray
Write-Host "   ?   ??? EnrollmentSystem.Shared\" -ForegroundColor Gray
Write-Host "   ??? README.md" -ForegroundColor Gray
Write-Host "   ??? QUICK_START.md" -ForegroundColor Gray
Write-Host "   ??? start-all-services.ps1" -ForegroundColor Gray
Write-Host "   ??? stop-all-services.ps1" -ForegroundColor Gray
Write-Host ""
Write-Host "?? Next Steps:" -ForegroundColor White
Write-Host "   1. Close Visual Studio" -ForegroundColor Yellow
Write-Host "   2. Navigate to: $targetDir" -ForegroundColor Yellow
Write-Host "   3. Open: EnrollmentSystem.sln" -ForegroundColor Yellow
Write-Host ""
Write-Host "??  Note: You can delete the old P4 directory after verifying everything works" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
