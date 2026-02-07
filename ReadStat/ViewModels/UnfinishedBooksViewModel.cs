using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReadStat.ViewModels.Books;

namespace ReadStat.ViewModels;

public partial class UnfinishedBooksViewModel : ObservableObject
{
    public ObservableCollection<IBookListItem> Books { get; } = [];

    [ObservableProperty]
    private IBookListItem? _selected;

    private readonly AddBookBtnViewModel _addBookBtnViewModel;
    private readonly BookViewModelFactory _bookViewModelFactory;

    public NavigationBarViewModel Navigation { get; private set; }

    public UnfinishedBooksViewModel(
        AddBookBtnViewModel  addBookBtnViewModel,
        NavigationBarViewModel navigationBarViewModel,
        BookViewModelFactory bookViewModelFactory)
    {
        Navigation = navigationBarViewModel;
        _addBookBtnViewModel = addBookBtnViewModel;
        _bookViewModelFactory = bookViewModelFactory;
        _ = Refresh();
    }

    private async Task Refresh()
    {
        Books.Clear();
        var all = await Database.ListUnfinishedBooksAsync();
        
        Books.Add(_addBookBtnViewModel);
        foreach (var b in all)
        {
            Books.Add(_bookViewModelFactory.Create(b));
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (Selected is not BookViewModel bvm) return;
        Database.Delete(bvm.Id);
        await Refresh();
    }
}
