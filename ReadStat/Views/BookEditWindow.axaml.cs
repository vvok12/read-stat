using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReadStat.Data;
using ReadStat.ViewModels.Books;

namespace ReadStat.Views;

public partial class BookEditWindow : Window
{
    public BookEditWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnSave(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is BookViewModel vm)
        {
            Database.AddOrUpdate(vm.ToModel());
        }
        Close();
    }

    private void OnCancel(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}
