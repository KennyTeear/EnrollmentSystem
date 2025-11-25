# Add Documentation and Scripts to Solution
# This script updates the solution file to include documentation and scripts

$solutionPath = "C:\Users\kendr\OneDrive\Desktop\STDISCM\EnrollmentSystem\EnrollmentSystem.sln"

Write-Host "?? Adding Documentation and Scripts to Visual Studio Solution..." -ForegroundColor Cyan
Write-Host ""

# Read the solution file
$content = Get-Content $solutionPath -Raw

# Find the line with Documentation folder
$docFolderStart = 'Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Documentation", "Documentation", "{5CE8E52D-A8AF-340A-4033-B85429601BE7}"'

# Add the documentation section
$docSection = @"
$docFolderStart
	ProjectSection(SolutionItems) = preProject
		README.md = README.md
		START_HERE.md = START_HERE.md
		QUICK_START.md = QUICK_START.md
		CLEANUP_SUMMARY.md = CLEANUP_SUMMARY.md
		DATABASE_SETUP_COMPLETE.md = DATABASE_SETUP_COMPLETE.md
		REORGANIZATION_COMPLETE.md = REORGANIZATION_COMPLETE.md
	EndProjectSection
"@

# Add Scripts folder before Global section
$scriptsSection = @"

Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Scripts", "Scripts", "{6DF9E62F-B8B1-4D4E-9C8A-1234567890AB}"
	ProjectSection(SolutionItems) = preProject
		start-all-services.ps1 = start-all-services.ps1
		stop-all-services.ps1 = stop-all-services.ps1
		setup-databases.ps1 = setup-databases.ps1
		cleanup-p4-files.ps1 = cleanup-p4-files.ps1
		reorganize-to-enrollmentsystem.ps1 = reorganize-to-enrollmentsystem.ps1
	EndProjectSection
EndProject
"@

# Replace the Documentation folder line
$content = $content -replace [regex]::Escape($docFolderStart), $docSection

# Add Scripts section before Global
$content = $content -replace '(EndProject\s+)Global', "`$1$scriptsSection`nGlobal"

# Write back to file
Set-Content -Path $solutionPath -Value $content -NoNewline

Write-Host "? Solution file updated successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "?? Added to Solution Explorer:" -ForegroundColor White
Write-Host "   ?? Documentation" -ForegroundColor Yellow
Write-Host "      ??? README.md" -ForegroundColor Gray
Write-Host "      ??? START_HERE.md" -ForegroundColor Gray
Write-Host "      ??? QUICK_START.md" -ForegroundColor Gray
Write-Host "      ??? CLEANUP_SUMMARY.md" -ForegroundColor Gray
Write-Host "      ??? DATABASE_SETUP_COMPLETE.md" -ForegroundColor Gray
Write-Host "      ??? REORGANIZATION_COMPLETE.md" -ForegroundColor Gray
Write-Host ""
Write-Host "   ?? Scripts" -ForegroundColor Yellow
Write-Host "      ??? start-all-services.ps1" -ForegroundColor Gray
Write-Host "      ??? stop-all-services.ps1" -ForegroundColor Gray
Write-Host "      ??? setup-databases.ps1" -ForegroundColor Gray
Write-Host "      ??? cleanup-p4-files.ps1" -ForegroundColor Gray
Write-Host "      ??? reorganize-to-enrollmentsystem.ps1" -ForegroundColor Gray
Write-Host ""
Write-Host "?? Next Steps:" -ForegroundColor White
Write-Host "   1. Close Visual Studio" -ForegroundColor Cyan
Write-Host "   2. Reopen EnrollmentSystem.sln" -ForegroundColor Cyan
Write-Host "   3. Look for 'Documentation' and 'Scripts' folders in Solution Explorer" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
