##========================================
## Build project
##========================================
Write-Output "Packaging Project -- Configuration Release"
Write-Output ""

dotnet build --configuration Release ..\FEDDEVOSS.ActiveDirectoryHelper.sln
dotnet pack ..\FEDDEVOSS.ActiveDirectoryHelper.sln

## Closing messaging
Write-Host ""
Write-Host "Script finished..."
Write-Host ""
Read-Host -Prompt "Press Enter to exit"
