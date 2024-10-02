using FlashCards.ViewModels;

namespace FlashCards.Contracts.Services;

public interface IStorageService
{
    Task AddFlashCardAsync(VMFlashCard vmFlashCard);
    Task<VMFlashCard> GetFlashCardAsync(int id);

    Task DeleteBoxAsync(int id);

    Task FlashCardCorrectAsync(int id);
    Task FlashCardWrongAsync(int id);
}
