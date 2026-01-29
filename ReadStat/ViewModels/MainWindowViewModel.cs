using CommunityToolkit.Mvvm.ComponentModel;

namespace ReadStat.ViewModels;

public class MainWindowViewModel: ObservableObject
{
    private object? _currentPage;
    public object? CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
}