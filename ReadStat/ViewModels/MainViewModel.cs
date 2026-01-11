using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;
using System.Collections.ObjectModel;
using System.Linq;
using ReadStat.Views;
using Avalonia.Controls;
using System;
using ReadStat.ViewModels.Books;

namespace ReadStat.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ObservableCollection<IBookListItem> Books { get; } = new();

    private IBookListItem? _selected;
    public IBookListItem? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public MainViewModel()
    {
        Refresh();
    }

    [RelayCommand]
    public void Refresh()
    {
        Books.Clear();
        Books.Add(new AddBookBtnViewModel());
        var all = Database.GetAllBooks();
        foreach (var b in all)
            Books.Add(new BookViewModel(b));
        OnPropertyChanged(nameof(TotalBooksRead));
        OnPropertyChanged(nameof(TotalPagesRead));
        OnPropertyChanged(nameof(PagesReadThisMonth));
    }

    [RelayCommand]
    private void Edit(Window? owner)
    {
        if (Selected == null) return;

        var model = Selected switch
        {
            BookViewModel bvm => bvm.ToModel(),
            AddBookBtnViewModel => new Book(),
            _ => throw new NotImplementedException()
        };
        
        var vm = new BookViewModel(model);
        var win = new BookEditWindow
        {
            DataContext = vm, 
            Icon = owner?.Icon
        };
        win.ShowDialog(owner).ContinueWith(_ => Refresh());
    }

    [RelayCommand]
    private void Delete()
    {
        if (Selected is not BookViewModel bvm) return;
        Database.Delete(bvm.Id);
        Refresh();
    }

    [RelayCommand]
    private void IncrementPages(int pages)
    {
        if (Selected is not BookViewModel bvm) return;
        bvm.PagesRead = Math.Min(bvm.PagesTotal, bvm.PagesRead + pages);
        var model = bvm.ToModel();
        if (bvm.PagesRead >= bvm.PagesTotal)
        {
            bvm.Completed = true;
        }
        Database.AddOrUpdate(model);
        Refresh();
    }

    [RelayCommand]
    private void Rate(int rating)
    {
        if (Selected is not BookViewModel bvm) return;
        bvm.Rating = rating;
        bvm.Completed = true;
        Database.AddOrUpdate(bvm.ToModel());
        Refresh();
    }

    public int TotalBooksRead => Books.OfType<BookViewModel>().Count(b => b.Completed);
    public int TotalPagesRead => Books.OfType<BookViewModel>().Sum(b => b.PagesRead);
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
