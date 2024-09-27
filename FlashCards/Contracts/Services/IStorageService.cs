using FlashCards.ViewModels;

namespace FlashCards.Contracts.Services;

public interface IStorageService
{
    Task AddFlashCardAsync(VMFlashCard vmFlashCard);
    Task<VMFlashCard> GetFlashCardAsync(int id);

    Task DeleteBox(int id);
}
