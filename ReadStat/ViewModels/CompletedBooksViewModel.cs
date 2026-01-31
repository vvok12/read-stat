using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.Data;
using ReadStat.ViewModels.Books;

namespace ReadStat.ViewModels;

public class CompletedBooksViewModel : ObservableObject
{
    public CompletedBooksViewModel(NavigationBarViewModel navigationBar)
    {
        NavigationBar = navigationBar;
    }
    
    public NavigationBarViewModel NavigationBar { get; }
    public Task<List<BookViewModel>> Books => Database
        .ListCompletedBooksAsync()
        .ContinueWith(t =>
        {
            var list = t.Result;
            return list.Select(book => new BookViewModel(book)).ToList();
        });
}