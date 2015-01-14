########################################################################
### BOOTSTRAP. Copy this section into each application setup script  ###
###            to make sure required functions are available.        ###
########################################################################

$pathOfThisScript = Split-Path $MyInvocation.MyCommand.Path -Parent
$parentFolderOfThisScript = $pathOfThisScript | Split-Path -Parent
$scriptsProject = 'Escc.WebApplicationSetupScripts'
$functionsPath = "$pathOfThisScript\..\$scriptsProject\functions.ps1"
if (Test-Path $functionsPath) {
  Write-Host "Checking $scriptsProject is up-to-date"
  Push-Location "$pathOfThisScript\..\$scriptsProject"
  git pull origin master
  Pop-Location
  Write-Host
  .$functionsPath
} else {
  if ($env:GIT_ORIGIN_URL) {
    $repoUrl = $env:GIT_ORIGIN_URL -f $scriptsProject
    git clone $repoUrl "$pathOfThisScript\..\$scriptsProject"
  } 
  else 
  {
    Write-Warning '$scriptsProject project not found. Please set a GIT_ORIGIN_URL environment variable on your system so that it can be downloaded.
  
Example: C:\>set GIT_ORIGIN_URL=https://example-git-server.com/{0}"
  
{0} will be replaced with the name of the repository to download.'
    Exit
  }
}

########################################################################
### END BOOTSTRAP. #####################################################
########################################################################

$projectName = "Escc.EastSussexGovUK"

DownloadProjectIfMissing $parentFolderOfThisScript "Escc.NavigationControls"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.Data.Web"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.Data.Xml"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.Egms"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.HouseStyle"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.Feeds"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.Data.ActiveDirectory"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.Cms"
DownloadProjectIfMissing $parentFolderOfThisScript "Escc.Elibrary"
NuGetRestoreForProject "$parentFolderOfThisScript\Escc.Egms"
NuGetRestoreForProject "$parentFolderOfThisScript\Escc.Cms"

EnableDotNet40InIIS
CreateApplicationPool $projectName true
CreateSSLCertificate $projectName
CreateWebsite $projectName $pathOfThisScript
CreateHTTPSBinding $projectName
RemoveHTTPBindings $projectName
CopyConfig "Web.example.config" "web.config"
CopyConfig "css\Web.example.config" "css\web.config"
CopyConfig "js\Web.example.config" "js\web.config"
CopyConfig "masterpages\remote\Web.example.config" "masterpages\remote\web.config"

Write-Host
Write-Host "Done." -ForegroundColor "Green"
Write-Host
Write-Host "Now replace the sample values in `web.config` with ones appropriate to your setup, and build the solution." -ForegroundColor "Green"