using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Models;
using ReadStat.Services;

namespace ReadStat.ViewModels.Books;

public partial class AddBookBtnViewModel : ObservableObject, IBookListItem
{
    private readonly NavigationService _nav;

    public AddBookBtnViewModel(NavigationService navigationService)
    {
        ArgumentNullException.ThrowIfNull(navigationService);
        _nav = navigationService;        
    }

    [RelayCommand]
    private void AddBook()
    {
        _nav.EditBook(new Book());
    }
}