using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using ReadStat.Messages;
using ReadStat.Models;
using ReadStat.ViewModels;

namespace ReadStat.Services;

public class NavigationService
{
    private object? _currentPage;
    public object CurrentPage
    {
        get 
        {
            if (_currentPage == null)
            {
                MoveToUnfinishedBooks();
            }
            return _currentPage!;
        }
        private set
        {
            if (_currentPage == value) return;
            
            _currentPage = value;
            WeakReferenceMessenger.Default.Send(new CurrentPageChanged(value));
        }
    }

    public void MoveToUnfinishedBooks()
    {
        CurrentPage = App.Services
            .GetRequiredService<UnfinishedBooksViewModel>();
    }

    public void MoveToStatistics()
    {
        CurrentPage = App.Services
            .GetRequiredService<StatsViewModel>();
    }

    public void MoveToCompletedBooks()
    {
        CurrentPage = App.Services
            .GetRequiredService<CompletedBooksViewModel>();
    }

    public void EditBook(Book book)
    {
        CurrentPage = App.Services
            .GetRequiredService<EditBookViewModel>()
            .WithModel(book);
    }
}