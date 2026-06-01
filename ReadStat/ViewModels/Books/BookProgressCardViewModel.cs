using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.Models;

namespace ReadStat.ViewModels.Books;

public class BookProgressCardViewModel(Book model): ObservableObject
{
    public int PagesTotal
    {
        get => model.PagesTotal;
        set
        {
            model.PagesTotal = value; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(Progress));
        }
    }

    public int PagesRead
    {
        get => model.PagesRead;
        set
        {
            model.PagesRead = value;
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(Progress));
        }
    }
    
    public double Progress => PagesTotal > 0 ? (double)PagesRead / PagesTotal : 0.0;
}