using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReadStat.ViewModels;

namespace ReadStat.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
