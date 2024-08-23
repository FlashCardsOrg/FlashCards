namespace FlashCards.Contracts.Services;

public interface IDemotionSettingsService
{
    string SelectedDemotionTag
    {
        get;
    }

    Task InitializeAsync();

    Task SetDemotionAsync(string SelectedDemotionTag);
}
