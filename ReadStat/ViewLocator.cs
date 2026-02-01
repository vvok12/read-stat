using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.ViewModels;
using ReadStat.ViewModels.Books;
using ReadStat.Views;
using ReadStat.Views.Books;

namespace ReadStat
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object? data)
        {
            return data switch
            {
                UnfinishedBooksViewModel mainViewModel => new UnfinishedBooksView(),
                BookViewModel bookViewModel => new BookView(),
                AddBookBtnViewModel addBookBtnViewModel => new AddBookBtnView(),
                EditBookViewModel editBookViewModel => new EditBookView(),
                _ => new TextBlock { Text = "View is not Found: " + data?.GetType() }
            };
        }

        public bool Match(object? data)
        {
            return data is ObservableObject;
        }
    }
}
