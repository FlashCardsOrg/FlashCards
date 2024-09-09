using FlashCards.DBModels;

namespace FlashCards.Contracts.Services;

public interface IDatabaseService
{
    int AddBox(int number, DueAfterOptions dueAfter);
    void UpdateBox(int id, DueAfterOptions dueAfter);
    void DeleteBox(int id);
    List<Box> GetBoxes();
}
