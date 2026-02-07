using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.ViewModels.Books;

namespace ReadStat.ViewModels;

public class CompletedBooksViewModel(
    NavigationBarViewModel navigationBar,
    BookViewModelFactory factory)
    : ObservableObject
{
    public NavigationBarViewModel NavigationBar { get; } = navigationBar;

    public Task<List<BookViewModel>> Books => Database
        .ListCompletedBooksAsync()
        .ContinueWith(t =>
        {
            var list = t.Result;
            return list.Select(factory.Create).ToList();
        });
}