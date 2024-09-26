using FlashCards.Contracts.Services;
using FlashCards.JSONModels;
using System.Text.Json;

namespace FlashCards.Services;

public class JSONService : IJSONService
{
    private const string _defaultBoxesFolder = "FlashCards/ApplicationData/Boxes";
    private const int _defaultBoxNumber = 1;

    private readonly JsonSerializerOptions _options;
    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _boxesFolder;

    public JSONService()
    {
        _options = new JsonSerializerOptions { WriteIndented = true };
        _boxesFolder = Path.Combine(_localApplicationData, _defaultBoxesFolder);
    }

    public string AddFlashCard(int flashCardID, JSONFlashCard flashCard, int? _boxNumber = null)
    {
        int boxNumber = _boxNumber ?? _defaultBoxNumber;
        string boxFolder = Path.Combine(_boxesFolder, $"Box-{boxNumber}");
        string flashCardFolder = Path.Combine(boxFolder, $"FlashCard-{flashCardID}");
        string flashCardFile = Path.Combine(flashCardFolder, $"FlashCard-{flashCardID}.json");

        Directory.CreateDirectory(flashCardFolder);

        string jsonString = JsonSerializer.Serialize(flashCard, _options);
        File.WriteAllText(flashCardFile, jsonString);

        return flashCardFolder;
    }

    public async Task<JSONFlashCard> GetFlashCardAsync(int boxNumber, int flashCardID)
    {
        string boxFolder = Path.Combine(_boxesFolder, $"Box-{boxNumber}");
        string flashCardFolder = Path.Combine(boxFolder, $"FlashCard-{flashCardID}");
        string flashCardFile = Path.Combine(flashCardFolder, $"FlashCard-{flashCardID}.json");

        using FileStream openStream = File.OpenRead(flashCardFile);

        JSONFlashCard flashCard = (await JsonSerializer.DeserializeAsync<JSONFlashCard>(openStream))!;
        return flashCard;
    }
}
