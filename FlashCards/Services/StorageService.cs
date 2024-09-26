using FlashCards.Contracts.Services;
using FlashCards.DBModels;
using FlashCards.JSONModels;
using FlashCards.ViewModels;

namespace FlashCards.Services;

public class StorageService(IDatabaseService databaseService, IJSONService JSONService) : IStorageService
{
    private readonly IDatabaseService _databaseService = databaseService;
    private readonly IJSONService _JSONService = JSONService;

    public void AddFlashCard(VMFlashCard vmFlashCard)
    {
        FlashCard flashCard = new()
        {
            Semester = vmFlashCard.Semester,
            BoxId = _databaseService.GetBoxID(vmFlashCard.BoxNumber),
            SubjectId = vmFlashCard.SubjectID,
            Tags = _databaseService.GetTags().Where(tag => vmFlashCard.TagIDs.Contains(tag.Id)).ToList()
        };
        int flashCardID = _databaseService.AddFlashCard(flashCard);

        JSONFlashCard jsonFlashCard = new(
            vmFlashCard.Front.Layout, vmFlashCard.Front.ShowBulletPointsIndividually,
            vmFlashCard.Back.Layout, vmFlashCard.Back.ShowBulletPointsIndividually
        );
        string flashCardFolder = _JSONService.AddFlashCard(flashCardID, jsonFlashCard);

        string frontFolder = Path.Combine(flashCardFolder, "Front");
        string backFolder = Path.Combine(flashCardFolder, "Back");
        Directory.CreateDirectory(frontFolder);
        Directory.CreateDirectory(backFolder);

        SaveFlashCardSideContent(vmFlashCard.Front, frontFolder);
        SaveFlashCardSideContent(vmFlashCard.Back, backFolder);
    }

    private static async void SaveFlashCardSideContent(VMFlashCard.VMFlashCardSide flashCardSide, string folder)
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

    public Task<VMFlashCard> GetFlashCardAsync(int id)
    {
        throw new NotImplementedException();
    }
}
