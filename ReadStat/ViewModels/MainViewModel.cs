using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using System;
using System.Threading.Tasks;
using ReadStat.Services;
using ReadStat.ViewModels.Books;

namespace ReadStat.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ObservableCollection<IBookListItem> Books { get; } = [];

    [ObservableProperty]
    private IBookListItem? _selected;

    private readonly NavigationService _navigationService;

    public MainViewModel(NavigationService navigationService)
    {
        ArgumentNullException.ThrowIfNull(navigationService);
        
        _navigationService = navigationService;
        _ = Refresh();
    }

    private async Task Refresh()
    {
        Books.Clear();
        var all = await Database.GetUnfinishedBooksAsync();
        
        Books.Add(new AddBookBtnViewModel());
        foreach (var b in all)
        {
            Books.Add(new BookViewModel(b));
        }
        
        OnPropertyChanged(nameof(TotalBooksRead));
        OnPropertyChanged(nameof(TotalPagesRead));
        OnPropertyChanged(nameof(PagesReadThisMonth));
    }

    [RelayCommand]
    private void Edit(ContentControl? owner)
    {
        if (Selected == null) return;

        var model = Selected switch
        {
            BookViewModel bvm => bvm.ToModel(),
            AddBookBtnViewModel => new Book(),
            _ => throw new InvalidOperationException("could not cast selected content")
        };

        _navigationService.EditBook(model);
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (Selected is not BookViewModel bvm) return;
        Database.Delete(bvm.Id);
        await Refresh();
    }

    public Task<int> TotalBooksRead => Database.CountCompletedBooksAsync();
    public Task<int> TotalPagesRead => Database.CountPagesReadAsync();
    public int PagesReadThisMonth
    {
        get
        {
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            // We don't store per-day page increments; approximate by summing pages for books created this month
            return Database.GetAllBooks().Where(b => b.CreatedAt >= monthStart).Sum(b => b.PagesRead);
        }
    }
}
