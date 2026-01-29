using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReadStat.Data;
using ReadStat.ViewModels;
using ReadStat.Views;
using System.IO;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace ReadStat;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // If you use CommunityToolkit, line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        // BindingPlugins.DataValidators.RemoveAt(0);

        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        collection.AddServices();

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();
        
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
            
            SetDataContext.Invoke(services.GetRequiredService<MainViewModel>());
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static Action<object> SetDataContext = null!;
}
