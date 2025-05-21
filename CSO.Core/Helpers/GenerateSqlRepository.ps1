# Ask the user for the table name
$tableName = Read-Host "Please enter the name of the table for the repository"

# Ask the user if they want to change the repository table name
$repoTableName = Read-Host "Do you want to change the repository table name? Enter new name or press Enter to use '$tableName'"

# If the user presses Enter without providing a new name, use the original table name
if ([string]::IsNullOrWhiteSpace($repoTableName)) {
    $repoTableName = $tableName
}

# Get the script's directory
$scriptDir = $PSScriptRoot

# Retract two folder levels
$baseDir = (Get-Item $scriptDir).Parent.FullName

# Determine the tableType by inspecting files in $baseDir\DatabaseContext
$dbContextDir = Join-Path -Path $baseDir -ChildPath "DatabaseContext"
$tableType = $null

if (Test-Path -Path $dbContextDir) {
    $files = Get-ChildItem -Path $dbContextDir -Filter *.cs -File
    foreach ($file in $files) {
        $content = Get-Content -Path $file.FullName
        $tableFound = $false
        for ($i = 0; $i -lt $content.Length; $i++) {
            if ($content[$i] -match "\[Table\(`"$tableName`"\)\]") {
                # Look ahead a few lines to find the class declaration
                $tableFound = $true
                for ($j = $i + 1; $j -lt $i + 10 -and $j -lt $content.Length; $j++) {
                    if ($content[$j] -match "class\s+\w+\s*:\s*(\w+)") {
                        $tableType = $matches[1]
                        break
                    }
                }
                if ($tableType) {
                    break
                }
            }
        }
        if ($tableType) {
            break
        }
    }
}

# If tableType is not found, report an error and exit
if (-not $tableType) {
    Write-Host "Error: Unable to determine the Table Type for the table $tableName." -ForegroundColor Red
    exit 1
}

# Output the table name, repository table name, tableType, and directory (for verification purposes)
Write-Host "You have entered: $tableName"
Write-Host "Repository table name: $repoTableName"
Write-Host "Table Type determined: $tableType"

# Ensure the "Repository" directory exists
$repositoryDir = Join-Path -Path $baseDir -ChildPath "Repositories"
if (-not (Test-Path -Path $repositoryDir)) {
    New-Item -Path $repositoryDir -ItemType Directory -Force
}

# Append $repoTableName`Repo to the "Repository" path
$outputDir = Join-Path -Path $repositoryDir -ChildPath "$repoTableName`Repo"

# Ensure the $repoTableName`Repo directory exists
if (-not (Test-Path -Path $outputDir)) {
    New-Item -Path $outputDir -ItemType Directory -Force
}

Write-Host "Files will be generated in: $outputDir"

# Define the output paths for the interface and class files
$interfaceFile = Join-Path -Path $outputDir -ChildPath "I${repoTableName}Repository.cs"
$classFile = Join-Path -Path $outputDir -ChildPath "${repoTableName}Repository.cs"

# Check if the interface file already exists and rename if necessary
if (Test-Path -Path $interfaceFile) {
    Rename-Item -Path $interfaceFile -NewName "I${repoTableName}Repository.cs.old" -Force
    Write-Host "Existing interface file renamed to: I${repoTableName}Repository.cs.old"
}

# Check if the class file already exists and rename if necessary
if (Test-Path -Path $classFile) {
    Rename-Item -Path $classFile -NewName "${repoTableName}Repository.cs.old" -Force
    Write-Host "Existing class file renamed to: ${repoTableName}Repository.cs.old"
}
$interfaceContent = ""
$classContent = ""
# Generate content for the interface
$interfaceContent += @"
namespace CSO.Core.Repositories.${repoTableName}Repo;

public interface I${repoTableName}Repository
{
    
}
"@

# Generate content for the class
$classContent += @"
using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.Shared;

namespace CSO.Core.Repositories.${repoTableName}Repo;

public class ${repoTableName}Repository : SqlTableRepository, I${repoTableName}Repository
{
    private readonly CSOResponsiveDbContext _dbContext;
    public ${repoTableName}Repository(CSOResponsiveDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
"@



# Write the content to the interface and class files
Set-Content -Path $interfaceFile -Value $interfaceContent
Set-Content -Path $classFile -Value $classContent

# Confirm file creation
Write-Host "Interface file created at: $interfaceFile"
Write-Host "Class file created at: $classFile"