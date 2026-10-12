param([string]$ConnectionString)
$ErrorActionPreference = 'Stop'
$projectRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
Push-Location $projectRoot
try {
    if ($ConnectionString) { $env:ConnectionStrings__DefaultConnection = $ConnectionString }
    $env:ASPNETCORE_ENVIRONMENT = 'Development'
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw 'EF araçları yüklenemedi.' }
    dotnet restore ApiProjeKampi.sln
    if ($LASTEXITCODE -ne 0) { throw 'Paketler yüklenemedi.' }
    dotnet ef database update --project ApiProjeKampi.WebApi
    if ($LASTEXITCODE -ne 0) { throw 'Veritabanı oluşturulamadı. SQL Server/LocalDB bağlantısını kontrol edin.' }
    Write-Host 'Veritabanı hazır. README içindeki iki terminal komutuyla API ve arayüzü başlatabilirsiniz.'
} finally { Pop-Location }
