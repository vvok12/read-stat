using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.Models;

namespace ReadStat.ViewModels.Books;

public class BookRatingCardViewModel(CompletedBook model): ObservableObject
{
    public string Rating { get; } = 
        model.Rating.HasValue
            ? $"{model.Rating}/5"
            : "Not Rated Yet";
}