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
        collection.AddTransient<MainViewModel>();
        collection.AddTransient<EditBookViewModel>();
        collection.AddTransient<AddBookBtnViewModel>();
        collection.AddTransient<BookViewModel>();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<CompletedBooksViewModel>();
        collection.AddSingleton<NavigationService>();
        collection.AddSingleton<WarmupService>();
        
        return collection; 
    }
}