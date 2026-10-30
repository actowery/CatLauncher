param([string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot))
$output = Join-Path $RepositoryRoot 'outputs'; New-Item -ItemType Directory -Force -Path $output | Out-Null
$zip = Join-Path $output 'CatLauncher.zip'; if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path (Join-Path $RepositoryRoot 'About'),(Join-Path $RepositoryRoot 'Defs'),(Join-Path $RepositoryRoot 'Languages'),(Join-Path $RepositoryRoot 'Textures'),(Join-Path $RepositoryRoot '1.6'),(Join-Path $RepositoryRoot '1.7'),(Join-Path $RepositoryRoot 'LoadFolders.xml') -DestinationPath $zip
(Get-FileHash $zip -Algorithm SHA256).Hash | Set-Content (Join-Path $output 'CatLauncher.zip.sha256')
