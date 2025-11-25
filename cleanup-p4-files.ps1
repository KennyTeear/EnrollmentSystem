# Clean Up P4 Files
# This script removes P4-specific files, keeping only EnrollmentSystem files

Write-Host "?? Cleaning up P4-specific files..." -ForegroundColor Cyan
Write-Host ""

$p4Files = @(
    "P4.csproj",
    "P4.csproj.user", 
    "P4.sln",
    "Program.cs"
)

$currentDir = Get-Location

Write-Host "?? Current directory: $currentDir" -ForegroundColor Yellow
Write-Host ""

foreach ($file in $p4Files) {
    $filePath = Join-Path $currentDir $file
    if (Test-Path $filePath) {
        Write-Host "???  Removing: $file" -ForegroundColor Gray
        Remove-Item $filePath -Force
    } else {
        Write-Host "??  Skipping (not found): $file" -ForegroundColor DarkGray
    }
}

Write-Host ""
Write-Host "? Cleanup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "?? Your directory now contains only EnrollmentSystem files:" -ForegroundColor White
Write-Host "   ? EnrollmentSystem.sln" -ForegroundColor Green
Write-Host "   ? src\ (with all 5 projects)" -ForegroundColor Green
Write-Host "   ? README.md" -ForegroundColor Green
Write-Host "   ? QUICK_START.md" -ForegroundColor Green
Write-Host "   ? start-all-services.ps1" -ForegroundColor Green
Write-Host "   ? stop-all-services.ps1" -ForegroundColor Green
Write-Host "   ? Database setup scripts" -ForegroundColor Green
Write-Host ""
Write-Host "?? Tip: You can rename this directory to 'EnrollmentSystem' if desired" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
