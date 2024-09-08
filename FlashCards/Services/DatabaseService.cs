using FlashCards.Contracts.Services;
using FlashCards.Data;
using FlashCards.DBModels;

namespace FlashCards.Services;

public class DatabaseService : IDatabaseService
{
    private readonly FlashCardsContext _context;

    public DatabaseService(FlashCardsContext context)
    {
        _context = context;
    }

    public void AddBox(int number, DueAfterOptions dueAfter)
    {
        Box newBox = new() { Number = number, DueAfter = dueAfter };
        _context.Boxes.Add(newBox);
        _context.SaveChanges();
    }

    public List<Box> GetBoxes()
    {
        return [.. _context.Boxes.OrderBy(box => box.Number)];
    }
}
