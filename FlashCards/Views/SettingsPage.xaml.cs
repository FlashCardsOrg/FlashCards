using FlashCards.Contracts.Services;
using FlashCards.DBModels;
using FlashCards.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FlashCards.Views;

public sealed partial class SettingsPage : Page
{
    private readonly ILocalizationService _localizationService = App.GetService<ILocalizationService>();
    private readonly IThemeSelectorService _themeSelectorService = App.GetService<IThemeSelectorService>();
    private readonly IDemotionSettingsService _demotionSettingsService = App.GetService<IDemotionSettingsService>();
    private readonly IDatabaseService _databaseService = App.GetService<IDatabaseService>();
    private readonly IStorageService _storageService = App.GetService<IStorageService>();

    private readonly Dictionary<int, TextBox> _editSubject_TextBoxes = [];
    private readonly Dictionary<int, TextBox> _editTag_TextBoxes = [];

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

        _localizationService.SetLanguageAsync(selectedLanguageTag);
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
        _themeSelectorService.SetThemeAsync(selectedTheme);
    }

    private void Demotion_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Tag is not string selectedDemotionTag)
        {
            return;
        }

        _demotionSettingsService.SetDemotionAsync(selectedDemotionTag);
    }

    private void AddBox_Button_Clicked(object sender, RoutedEventArgs e)
    {
        Settings_Box_Expander.IsExpanded = true;

        int number = ViewModel.Boxes.Count + 1;
        DueAfterOptions dueAfter = DueAfterOptions.OneDay;
        int id = _databaseService.AddBox(number, dueAfter);
        ViewModel.AddBox(id, number, dueAfter);
    }

    private void DeleteBox_Button_Clicked(object sender, RoutedEventArgs e)
    {
        if ((sender as MenuFlyoutItem)?.Tag is not int id)
        {
            return;
        }
        // TODO: Disbale Delete Button if last box
        _storageService.DeleteBox(id);
        ViewModel.DeleteBox(id);
    }

    private void Due_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedIndex is int selectedIndex && comboBox.Tag is int boxId)
        {
            DueAfterOptions dueAfter = (DueAfterOptions)selectedIndex;
            _databaseService.EditBox(boxId, dueAfter);
        }
    }

    private void AddSubject_Button_Clicked(object sender, RoutedEventArgs e)
    {
        Settings_Subject_Expander.IsExpanded = true;

        string name = "New Subject";
        int id = _databaseService.AddSubject(name);
        ViewModel.AddSubject(id, name);
    }

    private void EditSubject_Button_Clicked(object sender, RoutedEventArgs e)
    {

        if ((sender as Button)?.Tag is not int id)
        {
            return;
        }

        ViewModel.SetSubjectEditingState(id, true);
    }

    private void EditSubjectCancel_Button_Clicked(object sender, RoutedEventArgs e)
    {

        if ((sender as Button)?.Tag is not int id)
        {
            return;
        }

        ViewModel.SetSubjectEditingState(id, false);
    }

    private void EditSubjectSave_Button_Clicked(object sender, RoutedEventArgs e)
    {

        if ((sender as Button)?.Tag is not int id)
        {
            return;
        }

        string name = _editSubject_TextBoxes[id].Text;
        _databaseService.EditSubject(id, name);
        ViewModel.EditSubject(id, name);
        ViewModel.SetSubjectEditingState(id, false);
    }

    private void DeleteSubject_Button_Clicked(object sender, RoutedEventArgs e)
    {
        if ((sender as MenuFlyoutItem)?.Tag is not int id)
        {
            return;
        }

        // TODO: Dont allow delete if subject has flashcards
        _databaseService.DeleteSubject(id);
        ViewModel.DeleteSubject(id);
    }

    private void EditSubject_TextBox_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox || textBox.Tag is not int id)
        {
            return;
        }

        if (!_editSubject_TextBoxes.ContainsKey(id))
        {
            _editSubject_TextBoxes[id] = textBox;
        }
        textBox.Focus(FocusState.Programmatic);
        textBox.SelectionLength = textBox.Text.Length;
    }

    private void AddTag_Button_Clicked(object sender, RoutedEventArgs e)
    {
        Settings_Tag_Expander.IsExpanded = true;

        string name = "New Tag";
        int id = _databaseService.AddTag(name);
        ViewModel.AddTag(id, name);
    }

    private void EditTag_Button_Clicked(object sender, RoutedEventArgs e)
    {

        if ((sender as Button)?.Tag is not int id)
        {
            return;
        }

        ViewModel.SetTagEditingState(id, true);
    }

    private void EditTagCancel_Button_Clicked(object sender, RoutedEventArgs e)
    {

        if ((sender as Button)?.Tag is not int id)
        {
            return;
        }

        ViewModel.SetTagEditingState(id, false);
    }

    private void EditTagSave_Button_Clicked(object sender, RoutedEventArgs e)
    {

        if ((sender as Button)?.Tag is not int id)
        {
            return;
        }

        string name = _editTag_TextBoxes[id].Text;
        _databaseService.EditTag(id, name);
        ViewModel.EditTag(id, name);
        ViewModel.SetTagEditingState(id, false);
    }

    private void DeleteTag_Button_Clicked(object sender, RoutedEventArgs e)
    {
        if ((sender as MenuFlyoutItem)?.Tag is not int id)
        {
            return;
        }

        _databaseService.DeleteTag(id);
        ViewModel.DeleteTag(id);
    }
    private void EditTag_TextBox_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox || textBox.Tag is not int id)
        {
            return;
        }

        if (!_editTag_TextBoxes.ContainsKey(id))
        {
            _editTag_TextBoxes[id] = textBox;
        }
        textBox.Focus(FocusState.Programmatic);
        textBox.SelectionLength = textBox.Text.Length;
    }
}
