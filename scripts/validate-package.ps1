param([string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot), [switch]$RequireBinaries)
$ErrorActionPreference = 'Stop'
function Assert-Condition([bool]$Condition, [string]$Message) { if (-not $Condition) { throw $Message } }
$aboutPath = Join-Path $RepositoryRoot 'About\About.xml'; $defsPath = Join-Path $RepositoryRoot 'Defs\ThingDefs\CatLauncher.xml'; $previewPath = Join-Path $RepositoryRoot 'About\Preview.png'
[xml]$about = Get-Content -Raw $aboutPath; [xml]$defs = Get-Content -Raw $defsPath
Assert-Condition ($about.ModMetaData.packageId -eq 'konduct.catlauncher') 'Unexpected package ID.'
Assert-Condition (@($about.ModMetaData.supportedVersions.li) -contains '1.6') 'RimWorld 1.6 missing.'
Assert-Condition (@($about.ModMetaData.supportedVersions.li) -contains '1.7') 'RimWorld 1.7 missing.'
Assert-Condition ((Get-Item $previewPath).Length -lt 1MB) 'Workshop preview must be smaller than 1 MB.'
Assert-Condition ($defs.Defs.ThingDef.defName -contains 'Weapon_CatLauncher') 'Weapon definition missing.'
Assert-Condition ($defs.Defs.ThingDef.defName -contains 'Projectile_CatShell') 'Projectile definition missing.'
if ($RequireBinaries) { foreach ($version in '1.6','1.7') { Assert-Condition (Test-Path (Join-Path $RepositoryRoot "$version\Assemblies\CatLauncher.dll")) "Missing $version assembly." } }
Write-Output 'Package validation passed.'
