namespace FlashCards.Contracts.Services;

public interface ICreateSettingsService
{
    int SelectedSubjectID { get; }
    int SelectedSemester { get; }
    List<int> SelectedTagIDs { get; }

    Task InitializeAsync();

    Task SetSubjectAsync(int selectedSubjectID);
    Task SetSemesterAsync(int selectedSemester);
    Task SetTagsAsync(List<int> selectedTagIDs);
}
