using FlashCards.Contracts.Services;
using FlashCards.Data;
using FlashCards.DBModels;

namespace FlashCards.Services;

public class DatabaseService : IDatabaseService
{
    public void AddBox(int number, DueAfterOptions dueAfter)
    {
        using FlashCardsContext context = new();
        Box newBox = new() { Number = number, DueAfter = dueAfter };
        context.Boxes.Add(newBox);
        context.SaveChanges();
    }

    public List<Box> GetBoxes()
    {
        using FlashCardsContext context = new();
        return [.. context.Boxes.OrderBy(box => box.Number)];
    }
}
