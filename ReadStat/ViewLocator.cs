using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using ReadStat.ViewModels;

namespace ReadStat
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object? data)
        {
            var name = data!.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "View is not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ObservableObject;
        }
    }
}
