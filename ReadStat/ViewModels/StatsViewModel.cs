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
            
            var thisMonth = await QueryPagesReadThisMonthAsync();
            lock (Sync)
            {
                PagesReadThisMonth = thisMonth;
            }
        });
    }
    
    private async Task<ObservablePoint[]> QueryPagesReadThisMonthAsync()
    {
        var progress = GetLast30DaysProgress();
        var len = progress.Length;
        
        var shiftedValues = new ObservablePoint[len];
        for (var idx = 0; idx < len; idx++)
        {
            shiftedValues[idx] = new ObservablePoint(idx-len, progress[idx]);
        }
        return shiftedValues;
    }

    private int[] GetLast30DaysProgress()
    {
        var resp = new int[30] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        return resp;
    }
}