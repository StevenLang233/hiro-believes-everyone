param(
    [Parameter(Mandatory=$true)][string]$GameRoot
)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $GameRoot).Path
$pluginSource = Join-Path $PSScriptRoot 'BepInEx\plugins\ManosabaTrial12Fix.dll'
$infoSource = Join-Path $PSScriptRoot 'ManosabaMod\HiroBelievesEveryone\info.json'
$pluginTarget = Join-Path $root 'BepInEx\plugins\ManosabaTrial12Fix.dll'
$infoTarget = Join-Path $root 'ManosabaMod\HiroBelievesEveryone\info.json'
if (!(Test-Path -LiteralPath (Join-Path $root 'BepInEx\core\BepInEx.Unity.IL2CPP.dll'))) { throw 'BepInEx 6 Unity IL2CPP was not found in this game directory.' }
if (!(Test-Path -LiteralPath (Join-Path $root 'BepInEx\plugins\ManosabaLoader.dll'))) { throw 'ManosabaLoader was not found in this game directory.' }
foreach ($pair in @(@($pluginSource,$pluginTarget), @($infoSource,$infoTarget))) {
    $src = $pair[0]; $dst = $pair[1]
    $expected = (Get-FileHash -LiteralPath $src -Algorithm SHA256).Hash
    if (Test-Path -LiteralPath $dst) {
        $actual = (Get-FileHash -LiteralPath $dst -Algorithm SHA256).Hash
        if ($actual -ne $expected) { throw "Refusing to overwrite a different existing file: $dst" }
        Write-Output "Already installed: $dst"
    } else {
        $null = New-Item -ItemType Directory -Path (Split-Path -Parent $dst) -Force
        Copy-Item -LiteralPath $src -Destination $dst
        Write-Output "Installed: $dst"
    }
}


