using FlashCards.Contracts.Services;
using FlashCards.Data;
using FlashCards.DBModels;

namespace FlashCards.Services;

public class DatabaseService : IDatabaseService
{
    public int AddBox(int number, DueAfterOptions dueAfter)
    {
        using FlashCardsContext context = new();
        Box newBox = new() { Number = number, DueAfter = dueAfter };
        context.Boxes.Add(newBox);
        context.SaveChanges();
        return newBox.Id;
    }

    public void UpdateBox(int id, DueAfterOptions dueAfter)
    {
        using FlashCardsContext context = new();
        var box = context.Boxes.Find(id);
        if (box is not null)
        {
            box.DueAfter = dueAfter;
            context.SaveChanges();
        }
    }

    public void DeleteBox(int id)
    {
        using FlashCardsContext context = new();
        var box = context.Boxes.Find(id);
        if (box is not null)
        {
            context.Boxes.Remove(box);
            context.SaveChanges();
        }
    }

    public List<Box> GetBoxes()
    {
        using FlashCardsContext context = new();
        return [.. context.Boxes.OrderBy(box => box.Number)];
    }
}
