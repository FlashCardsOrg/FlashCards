using FlashCards.Contracts.Services;
using FlashCards.DBModels;
using FlashCards.JSONModels;
using FlashCards.ViewModels;

namespace FlashCards.Services;

public class StorageService(IDatabaseService databaseService, IJSONService JSONService) : IStorageService
{
    private const string _defaultBoxesFolder = "FlashCards/ApplicationData/Boxes";
    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    private readonly IDatabaseService _databaseService = databaseService;
    private readonly IJSONService _JSONService = JSONService;

    public async Task AddFlashCardAsync(VMFlashCard vmFlashCard)
    {
        FlashCard flashCard = new()
        {
            Semester = vmFlashCard.Semester,
            BoxId = _databaseService.GetBoxID(vmFlashCard.BoxNumber),
            SubjectId = vmFlashCard.SubjectID,
            Tags = _databaseService.GetTags().Where(tag => vmFlashCard.TagIDs.Contains(tag.Id)).ToList()
        };
        vmFlashCard.Id = _databaseService.AddFlashCard(flashCard);

        JSONFlashCard jsonFlashCard = new(
            vmFlashCard.Front.Layout, vmFlashCard.Front.ShowBulletPointsIndividually,
            vmFlashCard.Back.Layout, vmFlashCard.Back.ShowBulletPointsIndividually
        );
        string flashCardFolder = _JSONService.AddFlashCard(vmFlashCard.Id, jsonFlashCard);

        string frontFolder = Path.Combine(flashCardFolder, "Front");
        string backFolder = Path.Combine(flashCardFolder, "Back");
        Directory.CreateDirectory(frontFolder);
        Directory.CreateDirectory(backFolder);

        await SaveFlashCardSideContentAsync(vmFlashCard.Front, frontFolder);
        await SaveFlashCardSideContentAsync(vmFlashCard.Back, backFolder);
    }

    private static async Task SaveFlashCardSideContentAsync(VMFlashCard.VMFlashCardSide flashCardSide, string folder)
    {
        var fileExtension1 = Path.GetExtension(flashCardSide.Content1);
        var fileExtension2 = Path.GetExtension(flashCardSide.Content2);
        var fileExtension3 = Path.GetExtension(flashCardSide.Content3);

        switch (flashCardSide.Layout)
        {
            case Layouts.Text:
                await File.WriteAllTextAsync(Path.Combine(folder, "Content-1.rtf"), flashCardSide.Content1);
                break;
            case Layouts.File:
                File.Copy(flashCardSide.Content1!, Path.Combine(folder, $"Content-1{fileExtension1}"));
                break;
            case Layouts.Text_Text:
                await File.WriteAllTextAsync(Path.Combine(folder, "Content-1.rtf"), flashCardSide.Content1);
                await File.WriteAllTextAsync(Path.Combine(folder, "Content-2.rtf"), flashCardSide.Content2);
                break;
            case Layouts.File_File:
                File.Copy(flashCardSide.Content1!, Path.Combine(folder, $"Content-1{fileExtension1}"));
                File.Copy(flashCardSide.Content2!, Path.Combine(folder, $"Content-2{fileExtension2}"));
                break;
            case Layouts.Text_File:
                await File.WriteAllTextAsync(Path.Combine(folder, "Content-1.rtf"), flashCardSide.Content1);
                File.Copy(flashCardSide.Content2!, Path.Combine(folder, $"Content-2{fileExtension2}"));
                break;
            case Layouts.File_Text:
                File.Copy(flashCardSide.Content1!, Path.Combine(folder, $"Content-1{fileExtension1}"));
                await File.WriteAllTextAsync(Path.Combine(folder, "Content-2.rtf"), flashCardSide.Content2);
                break;
            case Layouts.Text_File_File:
                await File.WriteAllTextAsync(Path.Combine(folder, "Content-1.rtf"), flashCardSide.Content1);
                File.Copy(flashCardSide.Content2!, Path.Combine(folder, $"Content-2{fileExtension2}"));
                File.Copy(flashCardSide.Content3!, Path.Combine(folder, $"Content-3{fileExtension3}"));
                break;
        }
    }

    public async Task<VMFlashCard> GetFlashCardAsync(int id)
    {
        FlashCard flashCard = _databaseService.GetFlashCard(id)! ?? throw new ArgumentException("Flash card not found", nameof(id));
        JSONFlashCard jSONFlashCard = await _JSONService.GetFlashCardAsync(flashCard.Box.Number, id);
        VMFlashCard vmFlashCard = new()
        {
            Id = flashCard.Id,
            BoxNumber = flashCard.Box.Number,
            LastReviewDate = flashCard.LastReviewDate,
            SubjectID = flashCard.Subject.Id,
            TagIDs = (flashCard.Tags ?? []).Select(tag => tag.Id).ToList(),
            Front = await GetFlashCardSideAsync(flashCard, jSONFlashCard, "Front"),
            Back = await GetFlashCardSideAsync(flashCard, jSONFlashCard, "Back")
        };
        return vmFlashCard;
    }

    private async Task<VMFlashCard.VMFlashCardSide> GetFlashCardSideAsync(FlashCard flashCard, JSONFlashCard jsonFlashCard, string flashCardSide)
    {
        return flashCardSide switch
        {
            "Front" => new VMFlashCard.VMFlashCardSide
            {
                Layout = jsonFlashCard.Front.Layout,
                ShowBulletPointsIndividually = jsonFlashCard.Front.ShowBulletPointsIndividually,
                Content1 = await GetFlashCardSideContentAsync(flashCard, FlashCardSides.Front.ToString(), "Content-1"),
                Content2 = await GetFlashCardSideContentAsync(flashCard, FlashCardSides.Front.ToString(), "Content-2"),
                Content3 = await GetFlashCardSideContentAsync(flashCard, FlashCardSides.Front.ToString(), "Content-3")
            },
            "Back" => new VMFlashCard.VMFlashCardSide
            {
                Layout = jsonFlashCard.Back.Layout,
                ShowBulletPointsIndividually = jsonFlashCard.Back.ShowBulletPointsIndividually,
                Content1 = await GetFlashCardSideContentAsync(flashCard, FlashCardSides.Back.ToString(), "Content-1"),
                Content2 = await GetFlashCardSideContentAsync(flashCard, FlashCardSides.Back.ToString(), "Content-2"),
                Content3 = await GetFlashCardSideContentAsync(flashCard, FlashCardSides.Back.ToString(), "Content-3")
            },
            _ => throw new ArgumentException("Invalid flash card side", nameof(flashCardSide)),
        };
    }

    private async Task<string> GetFlashCardSideContentAsync(FlashCard flashCard, string flashCardSide, string fileName)
    {
        string boxesFolder = Path.Combine(_localApplicationData, _defaultBoxesFolder);
        string boxFolder = Path.Combine(boxesFolder, $"Box-{flashCard.Box.Number}");
        string flashCardSideFolder = Path.Combine(boxFolder, $"FlashCard-{flashCard.Id}/{flashCardSide}");


        string filePath = Path.Combine(flashCardSideFolder, $"{fileName}.rtf");
        if (File.Exists(filePath))
        {
            return await File.ReadAllTextAsync(filePath);
        }
        else
        {
            return filePath;
        }
    }
}
