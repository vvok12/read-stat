using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Services;

namespace ReadStat.ViewModels;

public partial class NavigationBarViewModel : ObservableObject
{
    private readonly NavigationService _nav;

    public NavigationBarViewModel(NavigationService navigationService)
    {
        _nav =  navigationService;
    }

    [RelayCommand]
    private void ToUnfinished()
    {
        _nav.MoveToUnfinishedBooks();
    }

    [RelayCommand]
    private void ToStatistics()
    {
        _nav.MoveToStatistics();
    }

    [RelayCommand]
    private void ToCompleted()
    {
        _nav.MoveToCompletedBooks();
    }
}