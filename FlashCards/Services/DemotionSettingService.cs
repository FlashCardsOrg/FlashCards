using FlashCards.Contracts.Services;

namespace FlashCards.Services;

public class DemotionSettingsService : IDemotionSettingsService
{
    private const string SettingsKey = "Demotion";
    private const string DefaultDemotionTag = "First";

    public string SelectedDemotionTag { get; set; } = DefaultDemotionTag;

    private readonly ILocalSettingsService _localSettingsService;

    public DemotionSettingsService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        SelectedDemotionTag = await LoadDemotionFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetDemotionAsync(string selectedDemotionTag)
    {
        SelectedDemotionTag = selectedDemotionTag;

        await SaveDemotionInSettingsAsync(SelectedDemotionTag);
    }

    private async Task<string> LoadDemotionFromSettingsAsync()
    {
        var selectedDemotionTag = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (selectedDemotionTag is null)
        {
            return DefaultDemotionTag;
        }

        return selectedDemotionTag;
    }

    private async Task SaveDemotionInSettingsAsync(string selectedDemotionTag)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, selectedDemotionTag);
    }
}
