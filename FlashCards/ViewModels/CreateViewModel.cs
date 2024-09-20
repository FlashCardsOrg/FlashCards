using CommunityToolkit.Mvvm.ComponentModel;
using FlashCards.Contracts.Services;
using System.Collections.ObjectModel;

namespace FlashCards.ViewModels;

public partial class CreateViewModel : ObservableRecipient
{
    private readonly IDatabaseService _databaseService;
    private readonly ICreateSettingsService _createSettingsService;

    [ObservableProperty]
    private ObservableCollection<VMSubject> _subjects;

    [ObservableProperty]
    private int _selectedSubjectID;

    [ObservableProperty]
    private int _selectedSemester;

    [ObservableProperty]
    private ObservableCollection<VMTag> _tags;

    [ObservableProperty]
    private List<int?> _selectedTagIDs;

    [ObservableProperty]
    private Layouts _frontLayout;

    [ObservableProperty]
    private Layouts _backLayout;

    [ObservableProperty]
    private bool _frontShowBulletPointsIndividually;

    [ObservableProperty]
    private bool _backShowBulletPointsIndividually;

    [ObservableProperty]
    private bool _isFrontRichEditBoxEmpty;

    [ObservableProperty]
    private bool _isBackRichEditBoxEmpty;

    [ObservableProperty]
    private bool _canSaveFlashCard;

    public CreateViewModel(IDatabaseService databaseService, ICreateSettingsService createSettingsService)
    {
        _databaseService = databaseService;
        _createSettingsService = createSettingsService;
        _subjects = GetSubjects();
        _selectedSubjectID = _createSettingsService.SelectedSubjectID;
        _selectedSemester = _createSettingsService.SelectedSemester;
        _tags = GetTags();
        _selectedTagIDs = _createSettingsService.SelectedTagIDs;
        _frontLayout = Layouts.Text;
        _backLayout = Layouts.Text;
        _frontShowBulletPointsIndividually = false;
        _backShowBulletPointsIndividually = false;
        _isFrontRichEditBoxEmpty = false;
        _isBackRichEditBoxEmpty = false;
        _canSaveFlashCard = IsAllRequiredContentSet();
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

    private bool IsAllRequiredContentSet()
    {
        bool subjectExists = _databaseService.GetSubject(SelectedSubjectID) is not null;
        return subjectExists && SelectedSemester > 0 && !IsFrontRichEditBoxEmpty && !IsBackRichEditBoxEmpty;
    }

    public void UpdateCanSaveFlashCard()
    {
        CanSaveFlashCard = IsAllRequiredContentSet();
    }
}
