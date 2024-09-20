using CommunityToolkit.Mvvm.ComponentModel;
using FlashCards.Contracts.Services;
using System.Collections.ObjectModel;

namespace FlashCards.ViewModels;

public partial class CreateViewModel : ObservableRecipient
{
    private readonly IDatabaseService _databaseService;
    private readonly ICreateSettingsService _createSettingsService;

    [ObservableProperty]
    private ObservableCollection<Subject> _subjects;

    [ObservableProperty]
    private int _selectedSubjectID;

    [ObservableProperty]
    private int _selectedSemester;

    [ObservableProperty]
    private ObservableCollection<Tag> _tags;

    [ObservableProperty]
    private List<int?> _selectedTagIDs;

    public CreateViewModel(IDatabaseService databaseService, ICreateSettingsService createSettingsService)
    {
        _databaseService = databaseService;
        _createSettingsService = createSettingsService;
        _subjects = GetSubjects();
        _selectedSubjectID = _createSettingsService.SelectedSubjectID;
        _selectedSemester = _createSettingsService.SelectedSemester;
        _tags = GetTags();
        _selectedTagIDs = _createSettingsService.SelectedTagIDs;
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
