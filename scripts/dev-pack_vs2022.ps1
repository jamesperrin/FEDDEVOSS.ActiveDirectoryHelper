##========================================
## Build project
##========================================
Write-Output "Packaging Project -- Configuration Debug"
Write-Output ""

dotnet build --configuration Debug ..\FEDDEVOSS.ActiveDirectoryHelper.sln
dotnet pack --configuration Debug ..\FEDDEVOSS.ActiveDirectoryHelper.sln

## Closing messaging
Write-Host ""
Write-Host "Script finished..."
Write-Host ""
Read-Host -Prompt "Press Enter to exit"
