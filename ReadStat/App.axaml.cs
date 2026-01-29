using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReadStat.Data;
using ReadStat.ViewModels;
using ReadStat.Views;
using System.IO;
using System;
using ReadStat.Models;

namespace ReadStat;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "readstat.db");
        Database.Initialize(dbPath);
        FileSystem.Initialize();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainVm = new MainWindowViewModel();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainVm
            };
            SetDataContext = (dc) => mainVm.CurrentPage = dc;
            
            SetDataContext.Invoke(new MainViewModel());
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static Action<object> SetDataContext = null!;
}
