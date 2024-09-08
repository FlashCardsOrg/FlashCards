﻿using System.Collections.ObjectModel;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

using FlashCards.Contracts.Services;
using FlashCards.Data;
using FlashCards.DBModels;
using FlashCards.Helpers;

using Windows.ApplicationModel;
using Windows.Security.Cryptography.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlashCards.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    private readonly IDemotionSettingsService _demotionSettingsService;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private string _selectedTheme;

    [ObservableProperty]
    private string _selectedLanguageTag;

    [ObservableProperty]
    private string _selectedDemotionTag;

    [ObservableProperty]
    private ObservableCollection<Box> _boxes;

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IDemotionSettingsService demotionSettingsService)
    {
        _themeSelectorService = themeSelectorService;
        _demotionSettingsService = demotionSettingsService;
        _versionDescription = GetVersionDescription();
        _selectedTheme = _themeSelectorService.Theme.ToString();
        _selectedLanguageTag = GetSelectedLanguageTag();
        _selectedDemotionTag = _demotionSettingsService.SelectedDemotionTag;
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
        IDatabaseService databaseService = App.GetService<IDatabaseService>();
        ObservableCollection<Box> boxes = new(databaseService.GetBoxes().Select(box => new Box(box.Number, (int)box.DueAfter)));
        return boxes;
    }

    internal void AddBox(int number, DueAfterOptions dueAfter)
    {
        Boxes.Add(new Box(number, (int)dueAfter ));
    }
}

public class Box(int number, int selectedIndex)
{
    public string BoxName { get; set; } = $"{WinUI3Localizer.Localizer.Get().GetLocalizedString("Box")} {number}";
    public int SelectedIndex { get; set; } = selectedIndex;
}

