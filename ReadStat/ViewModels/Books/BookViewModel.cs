using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.Data;
using ReadStat.Models;

namespace ReadStat.ViewModels.Books;

public class BookViewModel : ObservableObject, IBookListItem
{
    private readonly Book _model;

    public BookViewModel(Book model)
    {
        _model = model;
    }
    
    public int Id => _model.Id;
    public string Title
    {
        get => _model.Title;
        set
        {
            if (_model.Title == value) return;
            _model.Title = value;
            OnPropertyChanged();
        }
    }

    public Bitmap? Cover => FileSystem.LoadBookCover(_model.CoverId); 

    public int PagesTotal
    {
        get => _model.PagesTotal;
        set
        {
            _model.PagesTotal = value; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(Progress));
        }
    }

    public int PagesRead
    {
        get => _model.PagesRead;
        set
        {
            _model.PagesRead = value; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(Progress));
        }
    }

    public bool Completed
    {
        get => _model.Completed;
        set
        {
            _model.Completed = value; 
            OnPropertyChanged();
        }
    }

    public int Rating
    {
        get => _model.Rating;
        set
        {
            _model.Rating = value; 
            OnPropertyChanged();
        }
    }

    public double Progress => PagesTotal > 0 ? (double)PagesRead / PagesTotal : 0.0;

    public Book ToModel() => _model;

}
