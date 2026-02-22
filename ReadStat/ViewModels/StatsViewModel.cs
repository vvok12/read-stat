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