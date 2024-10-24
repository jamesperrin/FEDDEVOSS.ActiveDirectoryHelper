##========================================
## Build project
##========================================

Do {

  Clear-Host

  Write-Host "========================== FEDDEVOSS.ActiveDirectoryHelper ============================"
  Write-Host ""
  Write-Host ""
  Write-Host "********************************************************************************"
  Write-Host "*                                                                              *"
  Write-Host "*                                                                              *"
  Write-Host "* Publishing Project package -- Configuration Release                          *"
  Write-Host "*                                                                              *"
  Write-Host "*                                                                              *"
  Write-Host "********************************************************************************"
  Write-Host ""
  Write-Host "Enter Q to Exit"
  Write-Host "---------------------------------------------------------------------------------"
  Write-Host "[Q] Exit"
  Write-Host ""
  Write-Host ""
  $apiKeyPat = Read-Host -Prompt "Please provide an API key, or a personal token."

  $exitResponses = "q"

  if ($exitResponses -contains $apiKeyPat.ToLower()) {

    Write-Host ""
    Write-Host "Exiting..."
    Read-Host -Prompt "Press Enter to exit"
    Exit
  }
}While ($null -eq $apiKeyPat)

dotnet build --configuration Release ..\FEDDEVOSS.ActiveDirectoryHelper.sln

## Check if package exists
try {
  $package = Get-ChildItem -Path "..\build\Release\FEDDEVOSS.ActiveDirectoryHelper*.nupkg"
}
catch {
  Write-Host ""
  Write-Host "Exiting..."
  Write-Host "No package found."
  Read-Host -Prompt "Press Enter to exit"
  Exit
}

$packageName = $($package).Name

Write-Host "Publishing Project package -- $($packageName)"

dotnet nuget push "../build/Release/$($packageName)" --api-key "$($apiKeyPat)" --source nuget.org

## Closing messaging
Write-Host ""
Write-Host "Script finished...."
Read-Host -Prompt "Press Enter to exit"
Exit
