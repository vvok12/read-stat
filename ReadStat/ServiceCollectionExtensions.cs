using Microsoft.Extensions.DependencyInjection;
using ReadStat.Services;
using ReadStat.ViewModels;
using ReadStat.ViewModels.Books;
using ReadStat.Views.Books;

namespace ReadStat;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection collection)
    {
        collection.AddTransient<UnfinishedBooksViewModel>();
        collection.AddTransient<EditBookViewModel>();
        collection.AddTransient<AddBookBtnViewModel>();
        collection.AddTransient<UnfinishedBookViewModel>();
        collection.AddTransient<CompletedBooksViewModel>();
        collection.AddTransient<NavigationBarViewModel>();
        collection.AddTransient<StatsViewModel>();
        collection.AddSingleton<AddBookBtnViewModel>();
        collection.AddSingleton<UnfinishedBookViewModelFactory>();
        collection.AddSingleton<CompletedBookViewModelFactory>();
        
        collection.AddSingleton<MainWindowViewModel>();
        
        collection.AddSingleton<NavigationService>();
        collection.AddSingleton<WarmupService>();
        
        return collection; 
    }
}