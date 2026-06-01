using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;
using ReadStat.Services;

namespace ReadStat.ViewModels.Books;

public partial class UnfinishedBookViewModel(NavigationService navigationService) : ObservableObject, IBookListItem
{
    private Book _model = null!;

    public UnfinishedBookViewModel WithModel(Book model)
    {
        _model =  model;
        BookCardViewModel = new BookCardViewModel(_model);
        BookProgressCard = new BookProgressCardViewModel(_model);
        return this;
    }
    
    public BookCardViewModel BookCardViewModel { get; private set; } = null!;
    public BookProgressCardViewModel BookProgressCard { get; private set; } = null!;

    [RelayCommand]
    private void EditBook()
    {
        navigationService.EditBook(_model);
    }
    
    public int Id => _model.BookId;
    
}

public class UnfinishedBookViewModelFactory(NavigationService navigationService)
{
    public UnfinishedBookViewModel Create(Book model) => 
        new UnfinishedBookViewModel(navigationService)
            .WithModel(model);
}
