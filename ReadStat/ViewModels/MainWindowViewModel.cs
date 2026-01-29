using CommunityToolkit.Mvvm.ComponentModel;

namespace ReadStat.ViewModels;

public partial class MainWindowViewModel: ObservableObject
{
    [ObservableProperty]
    private object? _currentPage;
}