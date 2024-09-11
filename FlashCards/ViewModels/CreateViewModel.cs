using CommunityToolkit.Mvvm.ComponentModel;
using FlashCards.Contracts.Services;
using System.Collections.ObjectModel;

namespace FlashCards.ViewModels;

public partial class CreateViewModel : ObservableRecipient
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<Subject> _subjects;

    [ObservableProperty]
    private ObservableCollection<Tag> _tags;

    public CreateViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        _subjects = GetSubjects();
        _tags = GetTags();
    }

    private ObservableCollection<Subject> GetSubjects()
    {
        ObservableCollection<Subject> subjects = new(_databaseService.GetSubjects().Select(subject => new Subject(subject.Id, subject.Name)));
        return subjects;
    }

    private ObservableCollection<Tag> GetTags()
    {
        ObservableCollection<Tag> tags = new(_databaseService.GetTags().Select(tag => new Tag(tag.Id, tag.Name)));
        return tags;
    }
}
