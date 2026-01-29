using Avalonia.Controls;
using Avalonia.Input;
using ReadStat.ViewModels;

namespace ReadStat.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MainViewModel mainViewModel)
        {
            mainViewModel.EditCommand.Execute(this);
        }
    }
}