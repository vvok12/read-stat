using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReadStat.ViewModels;
using ReadStat.Views;
using ReadStat.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ReadStat;

public class App : Application
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
        Services = new ServiceCollection()
            .AddServices()
            .BuildServiceProvider();

        Services.GetRequiredService<WarmupService>().Run();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = App.Services.GetRequiredService<MainWindowViewModel>()
            };
        }

        Services.GetRequiredService<NavigationService>().MoveToMain();
        base.OnFrameworkInitializationCompleted();
    }
    
    public static ServiceProvider Services { get; private set; } = null!;
}
