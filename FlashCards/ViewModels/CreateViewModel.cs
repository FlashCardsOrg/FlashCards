using CommunityToolkit.Mvvm.ComponentModel;
using FlashCards.Contracts.Services;
using System.Collections.ObjectModel;

namespace FlashCards.ViewModels;

public partial class CreateViewModel : ObservableRecipient
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<VMSubject> _subjects;

    [ObservableProperty]
    private ObservableCollection<VMTag> _tags;

    [ObservableProperty]
    private VMFlashCard _flashCard = new();

    public CreateViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        _subjects = GetSubjects();
        _tags = GetTags();
    }

    private ObservableCollection<VMSubject> GetSubjects()
    {
        ObservableCollection<VMSubject> subjects = new(_databaseService.GetSubjects().Select(subject => new VMSubject(subject.Id, subject.Name)).OrderBy(subject => subject.SubjectName));
        return subjects;
    }

    private ObservableCollection<VMTag> GetTags()
    {
        ObservableCollection<VMTag> tags = new(_databaseService.GetTags().Select(tag => new VMTag(tag.Id, tag.Name)).OrderBy(tag => tag.TagName));
        return tags;
    }
}
