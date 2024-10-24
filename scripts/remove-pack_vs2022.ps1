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
  Write-Host "* Removing published Project package                                           *"
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
  $version = Read-Host -Prompt "Please provide package version."

  $exitResponses = "q"

  if ($exitResponses -contains $apiKeyPat.ToLower()) {

    Write-Host ""
    Write-Host "Exiting..."
    Read-Host -Prompt "Press Enter to exit"
    Exit
  }
}While ($null -eq $apiKeyPat)

dotnet nuget delete FEDDEVOSS.ActiveDirectoryHelper "$($version)" --api-key "$($apiKeyPat)" --source nuget.org

## Closing messaging
Write-Host ""
Write-Host "Script finished...."
Read-Host -Prompt "Press Enter to exit"
Exit
