using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.Data;

namespace ReadStat.ViewModels;

public class StatsViewModel : ObservableObject
{
    private readonly NavigationBarViewModel _nav;

    public StatsViewModel(NavigationBarViewModel navigationBar)
    {
        _nav =  navigationBar;
    }
    
    public Task<int> TotalBooksRead => Database.CountCompletedBooksAsync();
    public Task<int> TotalPagesRead => Database.CountPagesReadAsync();
    public int PagesReadThisMonth =>
        // not implement yet
        -1;
}