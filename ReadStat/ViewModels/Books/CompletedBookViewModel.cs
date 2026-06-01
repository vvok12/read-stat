using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Models;
using ReadStat.Services;

namespace ReadStat.ViewModels.Books;

public partial class CompletedBookViewModel(NavigationService navigationService): ObservableObject
{
    private Book _model = null!;
    public CompletedBookViewModel WithModel(CompletedBook model)
    {
        _model =  model;
        BookCard = new BookCardViewModel(model);
        BookRatingCard = new BookRatingCardViewModel(model);
        return this;
    }
    public BookCardViewModel BookCard { get; private set; } = null!;
    public BookRatingCardViewModel BookRatingCard { get; private set; } = null!;

    [RelayCommand]
    private void RateBook()
    {
        // TODO
    }
}

public class CompletedBookViewModelFactory(NavigationService navigationService)
{
    public CompletedBookViewModel Create(CompletedBook model) => 
        new CompletedBookViewModel(navigationService)
            .WithModel(model);
}