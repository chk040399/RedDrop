
# 🧾 Script Usage: `generate-sql-extended.ps1`

This PowerShell script automates the generation of **idempotent EF Core SQL migration scripts**, with enhanced logging, safety, test simulation, and CI-friendly output.

---

## 📦 Requirements

- .NET SDK 6.0+ with `dotnet ef` installed:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

- Your EF Core migration and startup projects must be valid and buildable.

---

## 🚀 Basic Usage

```powershell
.\generate-sql-extended.ps1 --project "src/MyApp.Infrastructure" --startup "src/MyApp.Web"
```

This will:
- Generate a `.sql` script for each migration
- Place them under `migrations-sql` (default output folder)
- Append log entries to `migration-sql-log/log.txt`

---

## 🔧 Parameters

| Parameter        | Type     | Description |
|------------------|----------|-------------|
| `--project`       | string (required) | Path to project containing `DbContext` |
| `--startup`       | string (required) | Path to startup project (FastEndpoints host) |
| `--output`        | string (default: `migrations-sql`) | Folder to store generated `.sql` files |
| `--logPath`       | string (default: `migration-sql-log`) | Folder to store logs |
| `--only`          | string[] | Specify migration names to generate (full timestamped names) |
| `--force`         | switch | Overwrite existing `.sql` files without prompt |
| `--dryRun`        | switch | Simulate changes, do not generate any files |
| `--testMode`      | switch | Like dry-run, but suppresses actual EF execution |
| `--verbose`       | switch | Show full EF commands being executed |
| `--noColor`       | switch | Disable colored output (for CI compatibility) |
| `--zipOutput`     | switch | Zip all `.sql` files after generation |
| `--deleteAfterZip`| switch | Delete `.sql` files after zipping |
| `--clean`         | switch | Delete all existing `.sql` files **before** generating |
| `--summaryJson`   | switch | Create JSON file summarizing the results of the run |

---

## 🧪 Examples

### 🧪 Simulate a Run Without Making Changes
```powershell
.\generate-sql-extended.ps1 --project src/Infra --startup src/Web --dryRun
```

### 🧪 Clean Existing SQL Files First
```powershell
.\generate-sql-extended.ps1 --project src/Infra --startup src/Web --clean
```

### 🧪 Force Overwrite and Zip Output
```powershell
.\generate-sql-extended.ps1 --project src/Infra --startup src/Web --force --zipOutput
```

---

## 📁 Output Files

### ✅ SQL Files
Located in `--output` folder (default: `migrations-sql`)

### ✅ Logs
- `log.txt` (persistent master log)
- `log_yyyyMMdd_HHmmss.txt` (run-specific log)

### ✅ JSON Summary (optional)
Enabled via `--summaryJson`, placed in `--logPath`

---

## 🛡 Safety Features

- Validates `Migrations` folder exists
- Aborts if no migrations are found
- Supports dry-run and test mode
- Tracks counts of created/skipped/overwritten files
- Logs actions and decisions

---

## 🧠 Best Practices

- Use `--only` to regenerate specific migrations
- Use `--zipOutput` for CI artifacts
- Use `--summaryJson` for integration in dashboards/pipelines

---

## 🧰 Recommended Aliases

For convenience, create a PowerShell alias:

```powershell
Set-Alias genmigrations .\generate-sql-extended.ps1
```

Then just run:
```powershell
genmigrations --project src/Infra --startup src/Web --force
```

---

## ❓ Support

For feature requests or bugs, feel free to extend the script or reach out.

Happy scripting! 🧪📜⚙️
