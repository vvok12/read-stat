using CommunityToolkit.Mvvm.ComponentModel;

namespace ReadStat.ViewModels;

public class MainWindowViewModel: ObservableObject
{
    public object? CurrentPage
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
}