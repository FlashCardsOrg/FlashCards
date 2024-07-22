using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FlashCards.Contracts.Services;
using FlashCards.Helpers;

using Microsoft.UI.Xaml;
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

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _versionDescription = GetVersionDescription();
        _selectedTheme = _themeSelectorService.Theme.ToString();
        _selectedLanguageTag = GetSelectedLanguageTag();
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
}
