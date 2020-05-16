[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $PostTitle
)

$todaysDate = Get-Date -Format "yyyy-MM-dd"

$postName = "$todaysDate - $PostTitle"

New-Item ".\$postName\" -ItemType Directory
New-Item ".\$postName\$postName.md" -ItemType File