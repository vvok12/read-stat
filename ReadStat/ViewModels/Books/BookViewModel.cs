using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;
using ReadStat.Services;

namespace ReadStat.ViewModels.Books;

public partial class BookViewModel(NavigationService navigationService) : ObservableObject, IBookListItem
{
    private Book _model = null!;
    public BookViewModel WithModel(Book model)
    {
        _model =  model;
        return this;
    }

    [RelayCommand]
    private void EditBook()
    {
        navigationService.EditBook(_model);
    }
    
    public int Id => _model.BookId;
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
    
    public double Progress => PagesTotal > 0 ? (double)PagesRead / PagesTotal : 0.0;
}

public class BookViewModelFactory(NavigationService navigationService)
{
    public BookViewModel Create(Book model) => new BookViewModel(navigationService)
        .WithModel(model);
}
