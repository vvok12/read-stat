# ReadStat

Minimal Avalonia desktop app (Linux / macOS) demonstrating reading gamification:

- Uses Avalonia UI, CommunityToolkit.Mvvm and SQLite (Dapper).
- Views: book list, edit/create dialog, statistics view.

Quick run:

```bash
dotnet restore
dotnet run --project read-stat.csproj
```

Notes:
- To apply SukiUI theme, add the SukiUI Avalonia theme package (if available) and merge its ResourceDictionary in `App.axaml`.
- This project is a minimal skeleton—feel free to extend image handling, better styling, and month-level page-tracking.
# read-stat