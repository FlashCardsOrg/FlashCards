using FlashCards.ViewModels;

namespace FlashCards.Contracts.Services;

public interface IStorageService
{
    void AddFlashCard(VMFlashCard vmFlashCard);
    Task<VMFlashCard> GetFlashCardAsync(int id);
}
