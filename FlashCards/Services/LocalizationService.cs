using FlashCards.Contracts.Services;

namespace FlashCards.Services;

public class LocalizationService : ILocalizationService
{
    private const string SettingsKey = "AppLanguage";
    private const string DefaultLanguageTag = "en-us";

    public string SelectedLanguageTag { get; set; } = DefaultLanguageTag;

    private readonly ILocalSettingsService _localSettingsService;

    public LocalizationService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        SelectedLanguageTag = await LoadLanguageFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetLanguageAsync(string selectedLanguageTag)
    {
        SelectedLanguageTag = selectedLanguageTag;

        await SetRequestedLanguageAsync(SelectedLanguageTag);
        await SaveLanguageInSettingsAsync(SelectedLanguageTag);
    }

    public async Task SetRequestedLanguageAsync(string selectedLanguageTag)
    {
        await WinUI3Localizer.Localizer.Get().SetLanguage(selectedLanguageTag);
        // TODO: Update TitleBar and ComboBoxes on SettingsPage

        await Task.CompletedTask;
    }

    private async Task<string> LoadLanguageFromSettingsAsync()
    {
        var selectedLanguageTag = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (selectedLanguageTag is null)
        {
            return DefaultLanguageTag;
        }

        return selectedLanguageTag;
    }

    private async Task SaveLanguageInSettingsAsync(string selectedLanguageTag)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, selectedLanguageTag);
    }
}
