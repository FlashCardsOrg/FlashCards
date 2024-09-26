using FlashCards.Contracts.Services;
using FlashCards.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;

namespace FlashCards.Views;

public sealed partial class StudyPage : Page
{
    private readonly IDatabaseService _databaseService = App.GetService<IDatabaseService>();

    public StudyViewModel ViewModel
    {
        get;
    }

    public StudyPage()
    {
        ViewModel = App.GetService<StudyViewModel>();
        InitializeComponent();
        Loaded += StudyPage_Loaded;
    }

    private async void StudyPage_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.GetNextFlashCardAsync();
        UpdatePageContent();
    }

    private void UpdatePageContent()
    {
        Update_FlashCardSide_TextBlock();
        Update_Box_TextBlock();
        Update_Subject_TextBlock();
        Update_Semester_TextBlock();
        Update_Tags_TextBlock();
        Update_Content_RichTextBlock();
        Update_NextBulletPoint_Button();
    }

    private void Update_FlashCardSide_TextBlock()
    {
        Study_FlashCardSides_TextBlock.Text = WinUI3Localizer.Localizer.Get().GetLocalizedString(ViewModel.FlashCard.CurrentSide.ToString());
    }

    private void Update_Box_TextBlock()
    {
        string boxString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Box");
        Study_Box_TextBlock.Text = $"{boxString}: {ViewModel.FlashCard.BoxNumber}";
    }

    private void Update_Subject_TextBlock()
    {
        var selectedSubject = _databaseService.GetSubject(ViewModel.FlashCard.SubjectID);
        string subjectString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Subject");
        string noneString = WinUI3Localizer.Localizer.Get().GetLocalizedString("None");
        Study_Subject_TextBlock.Text = $"{subjectString}: {selectedSubject?.Name ?? noneString}";
    }

    private void Update_Semester_TextBlock()
    {
        string semesterString = WinUI3Localizer.Localizer.Get().GetLocalizedString("Semester");
        Study_Semester_TextBlock.Text = $"{semesterString}: {ViewModel.FlashCard.Semester}";
    }

    private void Update_Tags_TextBlock()
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

        Study_Tags_TextBlock.Text = $"{tagsString}: {selectedTagsString}";
    }

    private void Update_Content_RichTextBlock()
    {
        // TODO: Fix the RichTextBlock Content
        switch (ViewModel.FlashCard.CurrentSide)
        {
            case FlashCardSides.Front:
                Study_Front_RichTextBlock.Blocks.Clear();
                Study_Front_RichTextBlock.Blocks.Add(new Paragraph());
                break;
            case FlashCardSides.Back:
                Study_Back_RichTextBlock.Blocks.Clear();
                Study_Back_RichTextBlock.Blocks.Add(new Paragraph());
                break;
        }
    }

    private void Update_NextBulletPoint_Button()
    {
        // TODO: Check if there are more bullet points to show
        switch (ViewModel.FlashCard.CurrentSide)
        {
            case FlashCardSides.Front:
                Study_NextBulletPoint_Button.IsEnabled = ViewModel.FlashCard.Front.ShowBulletPointsIndividually;
                break;
            case FlashCardSides.Back:
                Study_NextBulletPoint_Button.IsEnabled = ViewModel.FlashCard.Back.ShowBulletPointsIndividually;
                break;
        }
    }

    private void NextBulletPoint_Button_Clicked(object sender, RoutedEventArgs e)
    {
        // TODO: Implement the NextBulletPoint_Button_Clicked method
        throw new NotImplementedException("NextBulletPoint_Button_Clicked is not implemented");
    }

    private void Flip_Button_Clicked(object sender, RoutedEventArgs e)
    {
        switch (ViewModel.FlashCard.CurrentSide)
        {
            case FlashCardSides.Front:
                ViewModel.FlashCard.CurrentSide = FlashCardSides.Back;
                break;
            case FlashCardSides.Back:
                ViewModel.FlashCard.CurrentSide = FlashCardSides.Front;
                break;
        }
        ViewModel.FlashCard.WasFlipped = true;
        UpdatePageContent();
    }

    private async void Skip_Button_Clicked(object sender, RoutedEventArgs e)
    {
        await ViewModel.GetNextFlashCardAsync();
        UpdatePageContent();
    }
}
