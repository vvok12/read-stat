using System;
using Avalonia.Controls;
using Avalonia.Input;
using ReadStat.ViewModels;

namespace ReadStat.Views;

public partial class UnfinishedBooksView : UserControl
{
    public UnfinishedBooksView()
    {
        InitializeComponent();
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(DataContext);
        var viewModel = (UnfinishedBooksViewModel)DataContext;
        viewModel.EditCommand.Execute(this);
    }
}