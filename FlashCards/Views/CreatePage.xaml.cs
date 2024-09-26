using FlashCards.Contracts.Services;
using FlashCards.ViewModels;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace FlashCards.Views;

public sealed partial class CreatePage : Page
{
    private readonly ICreateSettingsService _createSettingsService = App.GetService<ICreateSettingsService>();
    private readonly IDatabaseService _databaseService = App.GetService<IDatabaseService>();
    private readonly IStorageService _storageService = App.GetService<IStorageService>();
    public CreateViewModel ViewModel { get; }

    public CreatePage()
    {
        ViewModel = App.GetService<CreateViewModel>();
        InitializeComponent();
    }

    private void EditSubject_DropDownButton_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not DropDownButton)
        {
            return;
        }

        var selectedSubject = _databaseService.GetSubject(ViewModel.FlashCard.SubjectID);
        string subjectString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Subject");
        string noneString = WinUI3Localizer.Localizer.Get().GetLocalizedString("None");
        Create_EditSubject_DropDownButton.Content = $"{subjectString}: {selectedSubject?.Name ?? noneString}";
    }

    private void EditSubject_DropDownButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (sender is not DropDownButton)
        {
            return;
        }

        Create_EditSubject_MenuFlyout.Items.Clear();
        foreach (VMSubject subject in ViewModel.Subjects)
        {
            MenuFlyoutItem menuFlyoutItem = new()
            {
                Text = subject.SubjectName,
                Tag = subject.SubjectID
            };
            menuFlyoutItem.Click += EditSubject_MenuFlyoutItem_Clicked;
            Create_EditSubject_MenuFlyout.Items.Add(menuFlyoutItem);
        }
    }

    private void EditSubject_MenuFlyoutItem_Clicked(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuFlyoutItem menuFlyoutItem)
        {
            return;
        }

        _createSettingsService.SetSubjectAsync((int)menuFlyoutItem.Tag);
        ViewModel.FlashCard.SubjectID = (int)menuFlyoutItem.Tag;
        string subjectString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Subject");
        Create_EditSubject_DropDownButton.Content = $"{subjectString}: {menuFlyoutItem.Text}";
    }

    private void EditSemester_DropDownButton_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not DropDownButton)
        {
            return;
        }

        string semesterString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Semester");
        Create_EditSemester_DropDownButton.Content = $"{semesterString}: {ViewModel.FlashCard.Semester}";
    }

    private void EditSemester_DropDownButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (sender is not DropDownButton)
        {
            return;
        }

        Create_EditSemester_MenuFlyout.Items.Clear();
        string semesterString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Semester");
        for (int i = 1; i <= 13; i++)
        {
            MenuFlyoutItem menuFlyoutItem = new()
            {
                Text = $"{i}. {semesterString}",
                Tag = i
            };
            menuFlyoutItem.Click += EditSemester_MenuFlyoutItem_Clicked;
            Create_EditSemester_MenuFlyout.Items.Add(menuFlyoutItem);
        }
    }

    private void EditSemester_MenuFlyoutItem_Clicked(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuFlyoutItem menuFlyoutItem)
        {
            return;
        }

        _createSettingsService.SetSemesterAsync((int)menuFlyoutItem.Tag);
        ViewModel.FlashCard.Semester = (int)menuFlyoutItem.Tag;
        string semesterString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Semester");
        Create_EditSemester_DropDownButton.Content = $"{semesterString}: {menuFlyoutItem.Tag}";
    }

    private void EditTags_DropDownButton_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not DropDownButton)
        {
            return;
        }

        Update_EditTags_DropDownButton_Content();
    }

    private void EditTags_DropDownButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (sender is not DropDownButton)
        {
            return;
        }

        Create_EditTags_MenuFlyout.Items.Clear();
        foreach (VMTag tag in ViewModel.Tags)
        {
            ToggleMenuFlyoutItem toggleMenuFlyoutItem = new()
            {
                Text = tag.TagName,
                Tag = tag.TagID,
                IsChecked = ViewModel.FlashCard.TagIDs.Contains(tag.TagID)
            };
            toggleMenuFlyoutItem.Click += EditTags_MenuFlyoutItem_Clicked;
            Create_EditTags_MenuFlyout.Items.Add(toggleMenuFlyoutItem);
        }
    }

    private void EditTags_MenuFlyoutItem_Clicked(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleMenuFlyoutItem toggleMenuFlyoutItem)
        {
            return;
        }

        if (toggleMenuFlyoutItem.IsChecked)
        {
            ViewModel.FlashCard.TagIDs.Add((int)toggleMenuFlyoutItem.Tag);
        }
        else
        {
            ViewModel.FlashCard.TagIDs.Remove((int)toggleMenuFlyoutItem.Tag);
        }

        _createSettingsService.SetTagsAsync(ViewModel.FlashCard.TagIDs);
        Update_EditTags_DropDownButton_Content();
    }

    private void Update_EditTags_DropDownButton_Content()
    {
        string selectedTagsString;
        string tagsString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Tags");

        if (ViewModel.FlashCard.TagIDs.Count == 0)
        {
            selectedTagsString = WinUI3Localizer.Localizer.Get().GetLocalizedString("None");
        }
        else
        {
            List<string> selectedTags = _databaseService.GetTags()
                                                        .Where(tag => ViewModel.FlashCard.TagIDs.Contains(tag.Id))
                                                        .OrderBy(tag => tag.Name)
                                                        .Select(tag => tag.Name)
                                                        .Take(3)
                                                        .ToList();
            selectedTagsString = string.Join(", ", selectedTags);
            if (selectedTags.Count == 3 && _databaseService.GetTags().Count(tag => ViewModel.FlashCard.TagIDs.Contains(tag.Id)) > 3)
            {
                selectedTagsString += ", ...";
            }
        }

        Create_EditTags_DropDownButton.Content = $"{tagsString}: {selectedTagsString}";
    }

    private void EditLayout_DropDownButton_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not DropDownButton dropDownButton)
        {
            return;
        }

        string layoutString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Layout");
        string layout = "";

        switch (dropDownButton.Name.Split("_")[1])
        {
            case "Front":
                layout = WinUI3Localizer.Localizer.Get().GetLocalizedString($"Layout_{ViewModel.FlashCard.Front.Layout}");
                break;
            case "Back":
                layout = WinUI3Localizer.Localizer.Get().GetLocalizedString($"Layout_{ViewModel.FlashCard.Back.Layout}");
                break;
        }

        dropDownButton.Content = $"{layoutString}: {layout}";
    }

    private void EditLayout_MenuFlyoutItem_Clicked(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuFlyoutItem menuFlyoutItem)
        {
            return;
        }

        string layoutString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Layout");
        string layout = WinUI3Localizer.Localizer.Get().GetLocalizedString($"Layout_{menuFlyoutItem.Tag}");

        switch (menuFlyoutItem.Name.Split("_")[1])
        {
            case "Front":
                ViewModel.FlashCard.Front.Layout = (Layouts)Enum.Parse(typeof(Layouts), (string)menuFlyoutItem.Tag);
                Create_Front_Layout_DropDownButton.Content = $"{layoutString}: {layout}";
                break;
            case "Back":
                ViewModel.FlashCard.Back.Layout = (Layouts)Enum.Parse(typeof(Layouts), (string)menuFlyoutItem.Tag);
                Create_Back_Layout_DropDownButton.Content = $"{layoutString}: {layout}";
                break;
        }
    }

    private void RichEditBox_TextChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not RichEditBox richEditBox)
        {
            return;
        }

        richEditBox.Document.GetText(TextGetOptions.FormatRtf, out string text);

        switch (richEditBox.Name.Split("_")[1])
        {
            // TODO: Implement other layout types
            case "Front":
                ViewModel.FlashCard.Front.Content1 = text;
                break;
            case "Back":
                ViewModel.FlashCard.Back.Content1 = text;
                break;
        }
    }

    private async void SaveFlashCard_Button_Clicked(object sender, RoutedEventArgs e)
    {
        await _storageService.AddFlashCardAsync(ViewModel.FlashCard);

        // TODO: Implement other layout types
        Create_Front_RichEditBox.Document.SetText(TextSetOptions.None, null);
        Create_Back_RichEditBox.Document.SetText(TextSetOptions.None, null);
        Create_FrontBack_SelectorBar.SelectedItem = Create_SelectorBarItem_Front;
        ViewModel.FlashCard = new();
    }
}
