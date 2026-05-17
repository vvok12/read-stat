using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using ReadStat.Data;

namespace ReadStat.ViewModels;

public partial class StatsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservablePoint[] _pagesReadThisMonth = [];

    [ObservableProperty]
    private int _totalPagesRead = 0;
    
    [ObservableProperty]
    private int _totalBooksRead = 0;
    
    public object Sync { get; } = new();
    
    public StatsViewModel()
    {
        Task.Run(async () =>
        {
            TotalBooksRead = await Database.CountCompletedBooksAsync();
            TotalPagesRead = await Database.CountPagesReadAsync();
            
            var thisMonth = await QueryDailyReadSumAsync();
            lock (Sync)
            {
                PagesReadThisMonth = thisMonth;
            }
        });
    }
    
    private async Task<ObservablePoint[]> QueryDailyReadSumAsync()
    {
        var reads = await Database.GetLastMonthDailyReadsAsync();
        if (reads.Count == 0)
        {
            return [];
        }

        if (reads.Count > 30)
        {
            throw new InvalidOperationException("More than 30 reads are not supported");
        }

        var prevY = 0;
        var values = reads
            .Select((r) =>
            {
                var x = r.DayBefore;
                var y = r.PageSum + prevY;
                prevY = y; 
                return new  ObservablePoint(x, y); 
            })
            .ToArray();

        return values;
    }
}