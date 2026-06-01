using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.Data;
using ReadStat.ViewModels.Books;

namespace ReadStat.ViewModels;

public class CompletedBooksViewModel(
    NavigationBarViewModel navigationBar,
    CompletedBookViewModelFactory factory)
    : ObservableObject
{
    public NavigationBarViewModel NavigationBar { get; } = navigationBar;

    public Task<List<CompletedBookViewModel>> Books => Database
        .ListCompletedBooksAsync()
        .ContinueWith(t =>
        {
            var list = t.Result;
            return list.Select(factory.Create).ToList();
        });
}