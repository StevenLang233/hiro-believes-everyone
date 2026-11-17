param(
    [Parameter(Mandatory=$true)][string]$GameRoot
)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $GameRoot).Path
$files = @(
    @((Join-Path $PSScriptRoot 'BepInEx\plugins\ManosabaTrial12Fix.dll'), (Join-Path $root 'BepInEx\plugins\ManosabaTrial12Fix.dll')),
    @((Join-Path $PSScriptRoot 'ManosabaMod\HiroBelievesEveryone\info.json'), (Join-Path $root 'ManosabaMod\HiroBelievesEveryone\info.json'))
)
foreach ($pair in $files) {
    $src = $pair[0]; $dst = $pair[1]
    if (!(Test-Path -LiteralPath $dst)) { continue }
    $expected = (Get-FileHash -LiteralPath $src -Algorithm SHA256).Hash
    $actual = (Get-FileHash -LiteralPath $dst -Algorithm SHA256).Hash
    if ($actual -eq $expected) {
        Remove-Item -LiteralPath $dst
        Write-Output "Removed: $dst"
    } else {
        Write-Warning "Left modified file in place: $dst"
    }
}
