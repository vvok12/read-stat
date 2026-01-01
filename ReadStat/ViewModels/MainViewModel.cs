using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;
using System.Collections.ObjectModel;
using System.Linq;
using ReadStat.Views;
using Avalonia.Controls;
using System;

namespace ReadStat.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ObservableCollection<BookViewModel> Books { get; } = new();

    private BookViewModel? _selected;
    public BookViewModel? Selected
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
        var all = Database.GetAllBooks();
        foreach (var b in all)
            Books.Add(new BookViewModel(b));
        OnPropertyChanged(nameof(TotalBooksRead));
        OnPropertyChanged(nameof(TotalPagesRead));
        OnPropertyChanged(nameof(PagesReadThisMonth));
    }

    [RelayCommand]
    private void Add(Window? owner)
    {
        var vm = new BookViewModel(new Book());
        var win = new BookEditWindow { DataContext = vm };
        win.Icon = owner?.Icon;
        win.ShowDialog(owner).ContinueWith(_ => Refresh());
    }

    [RelayCommand]
    private void Edit(Window? owner)
    {
        if (Selected == null) return;
        var model = Selected.ToModel();
        var vm = new BookViewModel(model);
        var win = new BookEditWindow { DataContext = vm };
        win.ShowDialog(owner).ContinueWith(_ => Refresh());
    }

    [RelayCommand]
    private void Delete()
    {
        if (Selected == null) return;
        Database.Delete(Selected.Id);
        Refresh();
    }

    [RelayCommand]
    private void IncrementPages(int pages)
    {
        if (Selected == null) return;
        Selected.PagesRead = Math.Min(Selected.PagesTotal, Selected.PagesRead + pages);
        var model = Selected.ToModel();
        if (Selected.PagesRead >= Selected.PagesTotal)
        {
            Selected.Completed = true;
        }
        Database.AddOrUpdate(model);
        Refresh();
    }

    [RelayCommand]
    private void Rate(int rating)
    {
        if (Selected == null) return;
        Selected.Rating = rating;
        Selected.Completed = true;
        Database.AddOrUpdate(Selected.ToModel());
        Refresh();
    }

    public int TotalBooksRead => Books.Count(b => b.Completed);
    public int TotalPagesRead => Books.Sum(b => b.PagesRead);
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
