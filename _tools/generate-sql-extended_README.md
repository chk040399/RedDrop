
# 🚀 EF Core Migration Script Generator

[![PowerShell](https://img.shields.io/badge/PowerShell-v5.1+-blue)](https://docs.microsoft.com/powershell/)
[![.NET CLI](https://img.shields.io/badge/.NET-6+-blueviolet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

A robust PowerShell tool to generate **idempotent EF Core SQL migration scripts** with features suited for both local development and CI/CD pipelines.

---

## ✨ Features

- ✅ Generate individual SQL files per EF migration
- ✅ Supports dry-run and test-mode simulation
- ✅ Automatic logging with timestamped logs
- ✅ JSON summary output for CI/CD dashboards
- ✅ Optional ZIP packaging of generated scripts
- ✅ Safe overwrite behavior and cleanup options

---

## 🛠 Requirements

- .NET 6.0+ SDK
- EF CLI:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## 🧾 Usage

```powershell
.\generate-sql-extended.ps1 --project "src/MyApp.Infrastructure" --startup "src/MyApp.Web"
```

---

## ⚙️ Command Line Parameters

| Parameter        | Description |
|------------------|-------------|
| `--project`       | (Required) Path to the EF Core project |
| `--startup`       | (Required) Path to the startup project |
| `--output`        | Output folder for `.sql` scripts (default: `migrations-sql`) |
| `--logPath`       | Output folder for logs (default: `migration-sql-log`) |
| `--only`          | Limit generation to specific migration(s) |
| `--force`         | Overwrite `.sql` files without confirmation |
| `--dryRun`        | Simulate actions without file changes |
| `--testMode`      | Simulate behavior without EF command execution |
| `--verbose`       | Output detailed EF commands |
| `--noColor`       | Disable colored output |
| `--clean`         | Delete all existing `.sql` files in output directory |
| `--zipOutput`     | Compress generated `.sql` files into a zip |
| `--deleteAfterZip`| Remove `.sql` files after zipping |
| `--summaryJson`   | Create a machine-readable summary JSON |

---

## 📁 Output Files

- SQL Scripts: `migrations-sql/*.sql`
- Logs:
  - `migration-sql-log/log.txt` (cumulative)
  - `migration-sql-log/log_YYYYMMDD_HHmmss.txt` (per run)
- JSON Summary (if `--summaryJson` used): `migration-sql-log/summary_YYYYMMDD_HHmmss.json`

---

## 📦 CI/CD Integration

You can run this script as part of your pipeline to generate artifacts:

```yaml
- powershell: |
    .\generate-sql-extended.ps1 --project "src/Infra" --startup "src/Web" --force --zipOutput --summaryJson
  displayName: Generate EF Scripts
```

---

## 🧠 Best Practices

- Use `--only` to control migration output in monorepos
- Run with `--clean` in automation to avoid stale output
- Add `--summaryJson` for dashboard or telemetry use

---

## 📚 License

This project is licensed under the MIT License.

---

## 🙌 Contributions Welcome

Feel free to fork and enhance this script for your needs. PRs and feedback are always appreciated!

---

> Generated with ❤️ by OpenAI ChatGPT
