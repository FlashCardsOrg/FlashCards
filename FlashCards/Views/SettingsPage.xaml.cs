using FlashCards.Contracts.Services;
using FlashCards.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FlashCards.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
        
    }

    private void Language_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Tag is not string selectedLanguageTag)
        {
            return;
        }

        ILocalizationService localizationService = App.GetService<ILocalizationService>();
        localizationService.SetLanguageAsync(selectedLanguageTag);
    }

    private void Theme_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Split in 2 if statments and return early
        if ((sender as ComboBox)?.SelectedItem is not ComboBoxItem selectedItem)
        {
            return;
        }
        if (Enum.TryParse(selectedItem.Tag.ToString(), out ElementTheme selectedTheme) is false)
        {
            return;
        }
        IThemeSelectorService themeSelectorService = App.GetService<IThemeSelectorService>();
        themeSelectorService.SetThemeAsync(selectedTheme);
    }
}
