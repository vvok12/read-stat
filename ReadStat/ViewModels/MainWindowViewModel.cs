using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ReadStat.Messages;
using ReadStat.Services;

namespace ReadStat.ViewModels;

public partial class MainWindowViewModel: ObservableObject
{
    public object CurrentPage { get; private set; }
    
    public NavigationBarViewModel NavigationBar { get; }

    public MainWindowViewModel(NavigationBarViewModel navigationBar, NavigationService navigationService)
    {
        ArgumentNullException.ThrowIfNull(navigationBar);
        NavigationBar = navigationBar;
        CurrentPage = navigationService.CurrentPage;
        WeakReferenceMessenger.Default
            .Register<CurrentPageChanged>(this, (_, _) =>
            {
                CurrentPage = navigationService.CurrentPage;
                OnPropertyChanged(nameof(CurrentPage));
            });
    }
}