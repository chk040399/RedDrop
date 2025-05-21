
param (
    [Parameter(Mandatory=$true)][string]$project,
    [Parameter(Mandatory=$true)][string]$startup,
    [string]$output = "migrations-sql",
    [string]$logPath = "migration-sql-log",
    [string[]]$only,
    [switch]$force,
    [switch]$dryRun,
    [switch]$verbose,
    [switch]$noColor,
    [switch]$zipOutput,
    [switch]$deleteAfterZip,
    [switch]$clean,
    [switch]$testMode,
    [switch]$summaryJson
)

function Show-Usage {
    Write-Host "Usage: .\generate-sql.ps1 --project <path> --startup <path> [--output <folder>] [--logPath <folder>]" -ForegroundColor Cyan
    Write-Host "                                     [--only <mig1> <mig2>] [--force] [--dry-run] [--verbose] [--noColor]" -ForegroundColor Cyan
    Write-Host "                                     [--zipOutput] [--deleteAfterZip] [--clean] [--testMode] [--summaryJson]" -ForegroundColor Cyan
    Write-Host ""
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

function Resolve-AbsolutePath([string]$inputPath) {
    if ([string]::IsNullOrWhiteSpace($inputPath)) {
        throw "A required path argument is missing."
    }

    if ([System.IO.Path]::IsPathRooted($inputPath)) {
        return (Resolve-Path -Path $inputPath).Path
    } else {
        return (Resolve-Path -Path (Join-Path $scriptDir $inputPath)).Path
    }
}

$resolvedProject = Resolve-AbsolutePath $project
$resolvedStartup = Resolve-AbsolutePath $startup
$resolvedOutput = Resolve-AbsolutePath $output
$resolvedLogPath = Resolve-AbsolutePath $logPath

if (-not (Test-Path $resolvedOutput)) {
    New-Item -ItemType Directory -Path $resolvedOutput | Out-Null
}

if (-not (Test-Path $resolvedLogPath)) {
    New-Item -ItemType Directory -Path $resolvedLogPath | Out-Null
}

$timestamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
$logFile = Join-Path $resolvedLogPath "log.txt"
$runLogFile = Join-Path $resolvedLogPath "log_$($timestamp.Replace(':','-').Replace(' ','_')).txt"
"--- Migration Script Generation - $timestamp ---" | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile

[int]$createdCount = 0
[int]$skippedCount = 0
[int]$overwrittenCount = 0

# Clean output folder if --clean
if ($clean -and -not $dryRun -and -not $testMode) {
    Get-ChildItem -Path $resolvedOutput -Filter *.sql -File | Remove-Item
    Write-Host "Cleaned existing .sql files in: $resolvedOutput" -ForegroundColor DarkGray
    "CLEANED existing .sql files" | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile -Append
}

# Validate migrations exist
$migrationFolder = Join-Path (Split-Path $resolvedProject) "Migrations"
if (-not (Test-Path $migrationFolder)) {
    Write-Warning "Migrations folder not found at: $migrationFolder"
    exit 1
}

$migrations = dotnet ef migrations list --project $resolvedProject --startup-project $resolvedStartup |
              Where-Object { $_ -match "^\d{14}_" } |
              ForEach-Object { $_.Trim() }

if ($migrations.Count -eq 0) {
    Write-Warning "No migrations found."
    exit 1
}

if ($only) {
    $migrations = $migrations | Where-Object { $only -contains $_ }
}

for ($i = 0; $i -lt $migrations.Count; $i++) {
    $from = if ($i -eq 0) { "" } else { $migrations[$i - 1] }
    $to = $migrations[$i]
    $fileName = "$to.sql"
    $filePath = Join-Path $resolvedOutput $fileName

    $action = ""

    if (Test-Path $filePath) {
        if ($dryRun -or $testMode) {
            $action = "SKIPPED (exists, dry-run)"
            if (-not $noColor) { Write-Host $action -ForegroundColor Red } else { Write-Host $action }
            "$action: $fileName" | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile -Append
            $skippedCount++
            continue
        }

        if ($force) {
            $action = "OVERWRITTEN (force)"
            if (-not $noColor) { Write-Host $action -ForegroundColor Yellow } else { Write-Host $action }
        } else {
            $response = Read-Host "File '$fileName' already exists. Overwrite? (y/n)"
            if ($response -ne 'y') {
                $action = "SKIPPED (exists, no overwrite)"
                if (-not $noColor) { Write-Host $action -ForegroundColor Red } else { Write-Host $action }
                "$action: $fileName" | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile -Append
                $skippedCount++
                continue
            } else {
                $action = "OVERWRITTEN (prompted)"
                if (-not $noColor) { Write-Host $action -ForegroundColor Yellow } else { Write-Host $action }
            }
        }
    } else {
        if ($dryRun -or $testMode) {
            $action = "CREATED (dry-run)"
            if (-not $noColor) { Write-Host $action -ForegroundColor Green } else { Write-Host $action }
            "$action: $fileName" | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile -Append
            $createdCount++
            continue
        }

        $action = "CREATED"
        if (-not $noColor) { Write-Host $action -ForegroundColor Green } else { Write-Host $action }
    }

    if (-not $testMode) {
        dotnet ef migrations script `
            $from $to `
            --idempotent `
            --project $resolvedProject `
            --startup-project $resolvedStartup `
            -o $filePath
    }

    "$action: $fileName" | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile -Append

    switch ($action) {
        { $_ -like "CREATED*" }      { $createdCount++ }
        { $_ -like "SKIPPED*" }      { $skippedCount++ }
        { $_ -like "OVERWRITTEN*" }  { $overwrittenCount++ }
    }
}

$summary = @(
    ""
    "Summary for run at $timestamp:"
    "  CREATED     : $createdCount"
    "  SKIPPED     : $skippedCount"
    "  OVERWRITTEN : $overwrittenCount"
    ""
    "------------------------------------------------------------"
)

Write-Host ""
if (-not $noColor) {
    Write-Host "Summary:" -ForegroundColor Cyan
    Write-Host "  CREATED     : $createdCount" -ForegroundColor Green
    Write-Host "  SKIPPED     : $skippedCount" -ForegroundColor Red
    Write-Host "  OVERWRITTEN : $overwrittenCount" -ForegroundColor Yellow
} else {
    Write-Host "Summary:"
    Write-Host "  CREATED     : $createdCount"
    Write-Host "  SKIPPED     : $skippedCount"
    Write-Host "  OVERWRITTEN : $overwrittenCount"
}

$summary | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile -Append

if ($summaryJson) {
    $summaryObject = @{
        Timestamp    = $timestamp
        Created      = $createdCount
        Skipped      = $skippedCount
        Overwritten  = $overwrittenCount
    }
    $jsonPath = Join-Path $resolvedLogPath "summary_$($timestamp.Replace(':','-').Replace(' ','_')).json"
    $summaryObject | ConvertTo-Json | Out-File $jsonPath
}

if ($zipOutput -and -not $testMode) {
    $zipName = "migrations_${timestamp.Replace(':','-').Replace(' ','_')}.zip"
    $zipPath = Join-Path $resolvedOutput $zipName
    $sqlFiles = Get-ChildItem -Path $resolvedOutput -Filter *.sql

    if ($sqlFiles.Count -gt 0) {
        Compress-Archive -Path $sqlFiles.FullName -DestinationPath $zipPath -Force
        Write-Host "Zipped output created at: $zipPath"
        "ZIPPED: $zipPath" | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile -Append

        if ($deleteAfterZip) {
            $sqlFiles | Remove-Item
            Write-Host "Original .sql files deleted after zipping."
            "DELETED SQL FILES after ZIP." | Tee-Object -FilePath $logFile -Append | Out-File $runLogFile -Append
        }
    }
}
