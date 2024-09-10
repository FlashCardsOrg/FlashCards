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
        if (box is null)
        {
            return;
        }
        MoveCardsOnDelete(box.Number);
        FixBoxNumbers(box.Number);
        context.Boxes.Remove(box);
        context.SaveChanges();
    }

    private static void MoveCardsOnDelete(int boxNumber)
    {
        int targetBoxNumber = boxNumber == 1 ? 2 : boxNumber - 1;
        MoveCards(boxNumber, targetBoxNumber);
    }

    private static void MoveCards(int fromBoxNumber, int toBoxNumber)
    {
        using FlashCardsContext context = new();
        var flashCards = context.FlashCards.Where(card => card.Box.Number == fromBoxNumber);
        foreach (var flashCard in flashCards)
        {
            flashCard.Box.Number = toBoxNumber;
        }
        context.SaveChanges();
    }

    private static void FixBoxNumbers(int number)
    {
        using FlashCardsContext context = new();
        var boxes = context.Boxes.Where(box => box.Number > number);
        foreach (var box in boxes)
        {
            box.Number--;
        }
        context.SaveChanges();
    }

    public List<Box> GetBoxes()
    {
        using FlashCardsContext context = new();
        return [.. context.Boxes.OrderBy(box => box.Number)];
    }
}
