using FlashCards.Contracts.Services;
using FlashCards.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FlashCards.Views;

public sealed partial class CreatePage : Page
{
    public CreateViewModel ViewModel
    {
        get;
    }

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

        ICreateSettingsService createSettingsService = App.GetService<ICreateSettingsService>();
        int selectedSubjectID = createSettingsService.SelectedSubjectID;
        IDatabaseService databaseService = App.GetService<IDatabaseService>();
        var selectedSubject = databaseService.GetSubject(selectedSubjectID);
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
        foreach (Subject subject in ViewModel.Subjects)
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

        ICreateSettingsService createSettingsService = App.GetService<ICreateSettingsService>();
        createSettingsService.SetSubjectAsync((int)menuFlyoutItem.Tag);
        string subjectString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Subject");
        Create_EditSubject_DropDownButton.Content = $"{subjectString}: {menuFlyoutItem.Text}";
    }

    private void EditSemester_DropDownButton_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not DropDownButton)
        {
            return;
        }

        ICreateSettingsService createSettingsService = App.GetService<ICreateSettingsService>();
        int selectedSemester = createSettingsService.SelectedSemester;
        string semesterString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Semester");
        Create_EditSemester_DropDownButton.Content = $"{semesterString}: {selectedSemester}";
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

        ICreateSettingsService createSettingsService = App.GetService<ICreateSettingsService>();
        createSettingsService.SetSemesterAsync((int)menuFlyoutItem.Tag);
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
        List<int?> selectedTagIDs = ViewModel.SelectedTagIDs;

        foreach (Tag tag in ViewModel.Tags)
        {
            ToggleMenuFlyoutItem toggleMenuFlyoutItem = new()
            {
                Text = tag.TagName,
                Tag = tag.TagID,
                IsChecked = selectedTagIDs.Contains(tag.TagID)
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
            ViewModel.SelectedTagIDs.Add((int)toggleMenuFlyoutItem.Tag);
        }
        else
        {
            ViewModel.SelectedTagIDs.Remove((int)toggleMenuFlyoutItem.Tag);
        }

        ICreateSettingsService createSettingsService = App.GetService<ICreateSettingsService>();
        createSettingsService.SetTagsAsync(ViewModel.SelectedTagIDs);

        Update_EditTags_DropDownButton_Content();
    }

    private void Update_EditTags_DropDownButton_Content()
    {
        List<int?> selectedTagIds = ViewModel.SelectedTagIDs;
        string selectedTagsString;
        string tagsString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Tags");

        if (selectedTagIds.Count == 0)
        {
            selectedTagsString = WinUI3Localizer.Localizer.Get().GetLocalizedString("None");
        }
        else
        {
            IDatabaseService databaseService = App.GetService<IDatabaseService>();
            List<string> selectedTags = databaseService.GetTags()
                                                       .Where(tag => selectedTagIds.Contains(tag.Id))
                                                       .Select(tag => tag.Name)
                                                       .Take(3)
                                                       .ToList();
            selectedTagsString = string.Join(", ", selectedTags);
            if (selectedTags.Count == 3 && databaseService.GetTags().Count(tag => selectedTagIds.Contains(tag.Id)) > 3)
            {
                selectedTagsString += ", ...";
            }
        }

        Create_EditTags_DropDownButton.Content = $"{tagsString}: {selectedTagsString}";
    }
}
