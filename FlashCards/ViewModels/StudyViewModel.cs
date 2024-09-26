using CommunityToolkit.Mvvm.ComponentModel;
using FlashCards.Contracts.Services;

namespace FlashCards.ViewModels;

public partial class StudyViewModel : ObservableRecipient
{
    private readonly IDatabaseService _databaseService;
    private readonly IStorageService _storageService;

    [ObservableProperty]
    private bool _studyingState = true;

    [ObservableProperty]
    private List<int> _dueFlashCardIDs;

    [ObservableProperty]
    private VMFlashCard _flashCard = new();
    public StudyViewModel(IDatabaseService databaseService, IStorageService storageService)
    {
        _databaseService = databaseService;
        _storageService = storageService;
        _dueFlashCardIDs = _databaseService.GetDueFlashCardIDs();
    }

    public async Task GetNextFlashCardAsync()
    {
        if (DueFlashCardIDs.Count == 0)
        {
            StudyingState = false;
            return;
        }

        int flashCardID = DueFlashCardIDs[0];
        FlashCard = await _storageService.GetFlashCardAsync(flashCardID);
        DueFlashCardIDs.RemoveAt(0);
    }
}
