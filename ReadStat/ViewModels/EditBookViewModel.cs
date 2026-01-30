using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadStat.Data;
using ReadStat.Models;
using ReadStat.Services;

namespace ReadStat.ViewModels;

public partial class EditBookViewModel: ObservableObject
{
    private readonly NavigationService _nav;

    public EditBookViewModel(NavigationService navigationService)
    {
        ArgumentNullException.ThrowIfNull(navigationService);
        _nav =  navigationService;
    }

    public EditBookViewModel WithModel(Book model)
    {
        ArgumentNullException.ThrowIfNull(model);
        _model = model;
        return this;
    }

    private Book? _model;
    
    [RelayCommand]
    private void Save()
    {
        if (_model is not null)
        {
            Database.AddOrUpdate(_model);
        }
        _nav.MoveToMain();
    }

    [RelayCommand]
    private void Cancel()
    {
        _nav.MoveToMain();
    }
    
    public Bitmap? Cover => FileSystem.LoadBookCover(_model?.CoverId);
    public string Title 
    { 
        get => _model?.Title ?? string.Empty;
        set
        {
            if (_model is null) return;
            if (_model.Title == value) return;
            _model.Title = value;
            OnPropertyChanged();
        } 
    }
    public int PagesTotal 
    { 
        get => _model?.PagesTotal ?? 0;
        set
        {
            if (_model is null) return;
            _model.PagesTotal = value; 
            OnPropertyChanged(); 
        } 
    }
    public int PagesRead
    {
        get => _model?.PagesRead ?? 0;
        set
        {
            if (_model is null) return;
            _model.PagesRead = value;
            OnPropertyChanged();
        }
    }
    
    public string? CoverId
    {
        get => _model?.CoverId;
        set
        {
            if (_model is null) return;
            _model.CoverId = value; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(Cover));
        }
    }


    [RelayCommand]
    private async Task ChangeCover(Button btn)
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

        if (btn.DataContext is EditBookViewModel model)
        {
            model.CoverId = generatedCoverId;
        }
    }
}