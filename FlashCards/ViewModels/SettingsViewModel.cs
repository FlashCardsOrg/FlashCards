﻿using System.Collections.ObjectModel;
using System.Reflection;

using CommunityToolkit.Mvvm.ComponentModel;

using FlashCards.Contracts.Services;
using FlashCards.Helpers;

using Windows.ApplicationModel;

namespace FlashCards.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private string _selectedTheme;

    [ObservableProperty]
    private string _selectedLanguageTag;

    [ObservableProperty]
    private ObservableCollection<Box> _boxes;

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _versionDescription = GetVersionDescription();
        _selectedTheme = _themeSelectorService.Theme.ToString();
        _selectedLanguageTag = GetSelectedLanguageTag();
        _boxes = GetBoxes();
    }

    private static string GetSelectedLanguageTag()
    {
        return WinUI3Localizer.Localizer.Get().GetCurrentLanguage();
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        var appDisplayName = WinUI3Localizer.Localizer.Get().GetLocalizedString("AppDisplayName");
        return $"{appDisplayName} - {version.Major}.{version.Minor}.{version.Build}";
    }

    private static ObservableCollection<Box> GetBoxes()
    {
        ObservableCollection<Box> boxes = new ObservableCollection<Box>();
        boxes.Add(new Box("MyBox 1", 0));
        boxes.Add(new Box("MyBox 2", 1));
        return boxes;

    }
}

public class Box(string boxName, int selectedIndex)
{
    public string BoxName { get; set; } = boxName;
    public int SelectedIndex { get; set; } = selectedIndex;
}

