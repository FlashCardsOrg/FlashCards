using FlashCards.Contracts.Services;
using FlashCards.DBModels;
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

    private void Demotion_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Tag is not string selectedDemotionTag)
        {
            return;
        }

        IDemotionSettingsService demotionSettingsService = App.GetService<IDemotionSettingsService>();
        demotionSettingsService.SetDemotionAsync(selectedDemotionTag);
    }

    private void AddBox_Button_Clicked(object sender, RoutedEventArgs e)
    {
        Settings_Box_Expander.IsExpanded = true;

        int number = ViewModel.Boxes.Count + 1;
        DueAfterOptions dueAfter = DueAfterOptions.OneDay;
        IDatabaseService databaseService = App.GetService<IDatabaseService>();
        int id = databaseService.AddBox(number, dueAfter);
        ViewModel.AddBox(id, number, dueAfter);
    }

    private void DeleteBox_Button_Clicked(object sender, RoutedEventArgs e)
    {
        if ((sender as MenuFlyoutItem)?.Tag is not int id)
        {
            return;
        }

        IDatabaseService databaseService = App.GetService<IDatabaseService>();
        databaseService.DeleteBox(id);
        ViewModel.DeleteBox(id);
        // TODO: Move Cards to another Box
    }

    private void Due_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedIndex is int selectedIndex && comboBox.Tag is int boxId)
        {
            var databaseService = App.GetService<IDatabaseService>();
            var dueAfter = (DueAfterOptions)selectedIndex;
            databaseService.UpdateBox(boxId, dueAfter);
        }
    }


}
