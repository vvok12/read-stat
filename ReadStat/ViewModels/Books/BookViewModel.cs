using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;

namespace ReadStat.ViewModels.Books;

public partial class BookViewModel : ObservableObject, IBookListItem
{
    private readonly Book _model;

    public BookViewModel(Book model)
    {
        _model = model;
    }

    public string? CoverId
    {
        get => _model.CoverId;
        set
        {
            _model.CoverId = value; 
            OnPropertyChanged(nameof(CoverId)); 
            OnPropertyChanged(nameof(Cover));
        }
    }
    
    public int Id => _model.Id;
    public string Title
    {
        get => _model.Title;
        set
        {
            if (_model.Title != value)
            {
                _model.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    public Bitmap? Cover => FileSystem.LoadBookCover(CoverId); 

    public int PagesTotal
    {
        get => _model.PagesTotal;
        set { _model.PagesTotal = value; OnPropertyChanged(nameof(PagesTotal)); OnPropertyChanged(nameof(Progress)); }
    }

    public int PagesRead
    {
        get => _model.PagesRead;
        set { _model.PagesRead = value; OnPropertyChanged(nameof(PagesRead)); OnPropertyChanged(nameof(Progress)); }
    }

    public bool Completed
    {
        get => _model.Completed;
        set { _model.Completed = value; OnPropertyChanged(nameof(Completed)); }
    }

    public int Rating
    {
        get => _model.Rating;
        set { _model.Rating = value; OnPropertyChanged(nameof(Rating)); }
    }

    public double Progress => PagesTotal > 0 ? (double)PagesRead / PagesTotal : 0.0;

    public Book ToModel() => _model;

}
