using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;
using ReadStat.ViewModels.Books;

namespace ReadStat.ViewModels;

public partial class EditBookViewModel: ObservableObject
{
    public EditBookViewModel(Book model)
    {
        _model = model;
    }

    private Book _model { get; set; }
    
    [RelayCommand]
    private void Save()
    {
        Database.AddOrUpdate(_model);
        App.SetDataContext?.Invoke(new MainViewModel());
    }

    [RelayCommand]
    private void Cancel()
    {
        App.SetDataContext?.Invoke(new MainViewModel());
    }
    
    public Bitmap? Cover => FileSystem.LoadBookCover(_model.CoverId);
    public string Title 
    { 
        get => _model.Title;
        set
        {
            if (_model.Title != value)
            {
                _model.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        } 
    }
    public int PagesTotal 
    { 
        get => _model.PagesTotal;
        set
        {
            _model.PagesTotal = value; 
            OnPropertyChanged(nameof(PagesTotal)); 
        } 
    }
    public int PagesRead
    {
        get => _model.PagesRead;
        set
        {
            _model.PagesRead = value;
            OnPropertyChanged(nameof(PagesRead));
        }
    }

    [RelayCommand]
    private async void ChangeCover(Button btn)
    {
        var topLevel = TopLevel.GetTopLevel(btn);
        
        if (topLevel?.StorageProvider is not { } sp)
        {
            return;
        }
        
        var files = await sp.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter = [FilePickerFileTypes.ImageAll]
        });

        if (files.Count < 1)
        {
            return;
        }
        
        var file = files[0];
        var generatedCoverId = Guid.NewGuid().ToString();
        using (var bmp = new Bitmap(await file.OpenReadAsync()))
        {
            bmp.Save(Path.Combine(AppContext.BaseDirectory, FileSystem.ImageFolder, $"{generatedCoverId}.bmp"));
        }

        if (btn.DataContext is BookViewModel model)
        {
            model.CoverId = generatedCoverId;
        }
    }
}