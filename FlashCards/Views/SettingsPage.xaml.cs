using FlashCards.ViewModels;

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
        if (((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Tag is not string selectedLanguage)
        {
            return;
        }

        // TODO: Save selected language in settings
        WinUI3Localizer.Localizer.Get().SetLanguage(selectedLanguage);
        // TODO: Update TitleBar
    }
}
