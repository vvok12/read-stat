using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.Data;
using ReadStat.Models;

namespace ReadStat.ViewModels.Books;

public class BookCardViewModel(Book model) : ObservableObject
{
    public string Title
    {
        get => model.Title;
        set
        {
            if (model.Title == value) return;
            model.Title = value;
            OnPropertyChanged();
        }
    }

    public Bitmap? Cover => FileSystem.LoadBookCover(model.CoverId); 
}