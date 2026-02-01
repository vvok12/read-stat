using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;
using System.Collections.ObjectModel;
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
    
    public NavigationBarViewModel Navigation { get; private set; }

    public MainViewModel(NavigationService navigationService, NavigationBarViewModel navigationBarViewModel)
    {
        Navigation = navigationBarViewModel;
        _navigationService = navigationService;
        _ = Refresh();
    }

    private async Task Refresh()
    {
        Books.Clear();
        var all = await Database.ListUnfinishedBooksAsync();
        
        Books.Add(new AddBookBtnViewModel());
        foreach (var b in all)
        {
            Books.Add(new BookViewModel(b));
        }
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
}
