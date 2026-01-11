using CommunityToolkit.Mvvm.ComponentModel;
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
            if (_model.Title != value)
            {
                _model.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

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

    public string? ImagePath
    {
        get => _model.ImagePath;
        set { _model.ImagePath = value; OnPropertyChanged(nameof(ImagePath)); }
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
