namespace FlashCards.Contracts.Services;

public interface ILocalizationService
{
    string SelectedLanguageTag
    {
        get;
    }

    Task InitializeAsync();

    Task SetLanguageAsync(string SelectedLanguageTag);

    Task SetRequestedLanguageAsync(string SelectedLanguageTag);
}
