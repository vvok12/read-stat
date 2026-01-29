using Microsoft.Extensions.DependencyInjection;
using ReadStat.Models;
using ReadStat.ViewModels;

namespace ReadStat.Services;

public class NavigationService
{
    private readonly MainWindowViewModel _mainWindow;

    public NavigationService(MainWindowViewModel mainWindow)
    {
        _mainWindow = mainWindow;
    }
    
    public void MoveToMain()
    {
        _mainWindow.CurrentPage = App.Services.GetRequiredService<MainViewModel>();
    }

    public void EditBook(Book book)
    {
        _mainWindow.CurrentPage = App.Services
            .GetRequiredService<EditBookViewModel>()
            .WithModel(book);
    }
}