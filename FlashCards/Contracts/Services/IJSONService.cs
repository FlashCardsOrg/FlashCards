using FlashCards.JSONModels;

namespace FlashCards.Contracts.Services;

public interface IJSONService
{
    string AddFlashCard(int flashCardID, JSONFlashCard flashCard, int? boxNumber = null);
    Task<JSONFlashCard> GetFlashCardAsync(int boxNumber, int flashCardID);
}
