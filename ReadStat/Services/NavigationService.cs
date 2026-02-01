using System;
using Microsoft.Extensions.DependencyInjection;
using ReadStat.Models;
using ReadStat.ViewModels;
using ReadStat.ViewModels.Books;

namespace ReadStat.Services;

public class NavigationService
{
    private readonly MainWindowViewModel _mainWindow;

    public NavigationService(MainWindowViewModel mainWindow)
    {
        _mainWindow = mainWindow;
    }
    
    public void MoveToUnfinishedBooks()
    {
        _mainWindow.CurrentPage = App.Services
            .GetRequiredService<UnfinishedBooksViewModel>();
    }

    public void MoveToStatistics()
    {
        _mainWindow.CurrentPage = App.Services
            .GetRequiredService<StatsViewModel>();
    }

    public void MoveToCompletedBooks()
    {
        _mainWindow.CurrentPage = App.Services
            .GetRequiredService<CompletedBooksViewModel>();
    }

    public void EditBook(Book book)
    {
        _mainWindow.CurrentPage = App.Services
            .GetRequiredService<EditBookViewModel>()
            .WithModel(book);
    }
}