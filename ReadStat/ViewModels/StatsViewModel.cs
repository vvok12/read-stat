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

public class StatsViewModel : ObservableObject
{
    private List<ObservablePoint> _pagesReadThisMonth = [];

    /*
     *
       <lvc:CartesianChart.Series>
         <lvc:XamlLineSeries
           SeriesName="Pages this month"
           Values="{Binding PagesReadThisMonth}"
           Fill="{x:Null}"
           GeometrySize="20"
           x:TypeArguments="x:Int32, draw:StarGeometry"/>
       </lvc:CartesianChart.Series>
     *
     * 
     */
    
    public StatsViewModel()
    {
        Series.Add(new LineSeries<ObservablePoint, StarGeometry>
        {
            Name = "Pages this month",
            GeometrySize = 20,
            Values = _pagesReadThisMonth,
            Fill = null
        });
        Task.Run(QueryPagesReadThisMonthAsync);
    }
    
    public Task<int> TotalBooksRead => Database.CountCompletedBooksAsync();
    public Task<int> TotalPagesRead => Database.CountPagesReadAsync();

    public object Sync { get; } = new();
    public List<ISeries> Series { get; set; } = new();
    
    private async Task QueryPagesReadThisMonthAsync()
    {
        var progress = GetLast30DaysProgress();
        var len = progress.Length;
        var values = progress.Select((el, idx) => new ObservablePoint(idx - len, el)).ToArray();

        lock (Sync)
        {
            _pagesReadThisMonth.Clear();
            _pagesReadThisMonth.AddRange(values);
        }
    }

    private int[] GetLast30DaysProgress()
    {
        var resp = new int[30] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        return resp;
    }
}