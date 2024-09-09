using FlashCards.DBModels;

namespace FlashCards.Contracts.Services;

public interface IDatabaseService
{
    void AddBox(int number, DueAfterOptions dueAfter);
    List<Box> GetBoxes();
}
