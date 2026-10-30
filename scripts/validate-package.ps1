param([string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot), [switch]$RequireBinaries)
$ErrorActionPreference = 'Stop'
function Assert-Condition([bool]$Condition, [string]$Message) { if (-not $Condition) { throw $Message } }
$aboutPath = Join-Path $RepositoryRoot 'About\About.xml'; $defsPath = Join-Path $RepositoryRoot 'Defs\ThingDefs\CatLauncher.xml'; $previewPath = Join-Path $RepositoryRoot 'About\Preview.png'
$loadFoldersPath = Join-Path $RepositoryRoot 'LoadFolders.xml'
[xml]$about = Get-Content -Raw $aboutPath; [xml]$defs = Get-Content -Raw $defsPath; [xml]$loadFolders = Get-Content -Raw $loadFoldersPath
Assert-Condition ($about.ModMetaData.packageId -eq 'konduct.catlauncher') 'Unexpected package ID.'
Assert-Condition (@($about.ModMetaData.supportedVersions.li) -contains '1.6') 'RimWorld 1.6 missing.'
Assert-Condition (@($about.ModMetaData.supportedVersions.li) -contains '1.7') 'RimWorld 1.7 missing.'
Assert-Condition ((Get-Item $previewPath).Length -lt 1MB) 'Workshop preview must be smaller than 1 MB.'
Assert-Condition ($defs.Defs.ThingDef.defName -contains 'Gun_CatLauncher') 'Weapon definition missing.'
Assert-Condition (($defs.Defs.ThingDef | Where-Object { $_.defName -eq 'Gun_CatLauncher' }).thingClass -eq 'CatLauncher.Thing_CatLauncher') 'Weapon equip hook missing.'
Assert-Condition (Test-Path (Join-Path $RepositoryRoot 'Defs\SoundDefs\CatLauncher_Sounds.xml')) 'Weapon equip sound definition missing.'
Assert-Condition (Test-Path (Join-Path $RepositoryRoot 'Defs\HediffDefs\CatLauncher_SurvivorTints.xml')) 'Survivor cat tint definitions missing.'
Assert-Condition ($defs.Defs.ThingDef.defName -contains 'Projectile_CatShell') 'Projectile definition missing.'
Assert-Condition ($defs.Defs.ThingDef | Where-Object { $_.defName -eq 'Mote_CatFragment' } | ForEach-Object { $_.mote -and $_.graphicData.graphicClass -eq 'Graphic_Multi' }) 'Cat fragment mote definition is invalid.'
foreach ($version in 'v1.6','v1.7') { Assert-Condition (@($loadFolders.loadFolders.$version.li) -contains '/') "Shared root is not loaded for $version." }
if ($RequireBinaries) { foreach ($version in '1.6','1.7') { Assert-Condition (Test-Path (Join-Path $RepositoryRoot "$version\Assemblies\CatLauncher.dll")) "Missing $version assembly." } }
Write-Output 'Package validation passed.'
