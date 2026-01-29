using Microsoft.Extensions.DependencyInjection;
using ReadStat.ViewModels;

namespace ReadStat;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection collection)
    {
        collection.AddTransient<MainViewModel>();
    }
}