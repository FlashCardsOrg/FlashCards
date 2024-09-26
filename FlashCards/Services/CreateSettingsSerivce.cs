using FlashCards.Contracts.Services;

namespace FlashCards.Services;

public class CreateSettingsService : ICreateSettingsService
{
    private const string SubjectID_SettingsKey = "Subject_ID";
    private const string Semester_SettingsKey = "Semester";
    private const string TagIDs_SettingsKey = "Tag_IDs";

    private const int DefaultSemester = 1;

    public int SelectedSubjectID { get; set; } = 0;
    public int SelectedSemester { get; set; } = DefaultSemester;
    public List<int> SelectedTagIDs { get; set; } = [];

    private readonly ILocalSettingsService _localSettingsService;

    public CreateSettingsService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        SelectedSubjectID = await LoadSubjectFromSettingsAsync();
        SelectedSemester = await LoadSemesterFromSettingsAsync();
        SelectedTagIDs = await LoadTagsFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetSubjectAsync(int selectedSubjectID)
    {
        SelectedSubjectID = selectedSubjectID;
        await SaveSubjectInSettingsAsync(selectedSubjectID);
    }

    private async Task<int> LoadSubjectFromSettingsAsync()
    {
        var selectedSubjectID = await _localSettingsService.ReadSettingAsync<int>(SubjectID_SettingsKey);
        return selectedSubjectID;
    }

    private async Task SaveSubjectInSettingsAsync(int selectedSubjectID)
    {
        await _localSettingsService.SaveSettingAsync(SubjectID_SettingsKey, selectedSubjectID);
    }

    public async Task SetSemesterAsync(int selectedSemester)
    {
        SelectedSemester = selectedSemester;
        await SaveSemesterInSettingsAsync(SelectedSemester);
    }

    private async Task<int> LoadSemesterFromSettingsAsync()
    {
        var selectedSemester = await _localSettingsService.ReadSettingAsync<int>(Semester_SettingsKey);
        if (selectedSemester == 0)
        {
            selectedSemester = DefaultSemester;
        }
        return selectedSemester;
    }

    private async Task SaveSemesterInSettingsAsync(int selectedSemester)
    {
        await _localSettingsService.SaveSettingAsync(Semester_SettingsKey, selectedSemester);
    }

    public async Task SetTagsAsync(List<int> selectedTagIDs)
    {
        SelectedTagIDs = selectedTagIDs;
        await SaveTagsInSettingsAsync(SelectedTagIDs);
    }

    private async Task<List<int>> LoadTagsFromSettingsAsync()
    {
        var selectedTagIDs = await _localSettingsService.ReadSettingAsync<List<int>>(TagIDs_SettingsKey) ?? [];
        return selectedTagIDs;
    }

    private async Task SaveTagsInSettingsAsync(List<int> selectedTagIDs)
    {
        await _localSettingsService.SaveSettingAsync(TagIDs_SettingsKey, selectedTagIDs);
    }
}
