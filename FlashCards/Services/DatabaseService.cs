using FlashCards.Contracts.Services;
using FlashCards.Data;
using FlashCards.DBModels;

namespace FlashCards.Services;

public class DatabaseService : IDatabaseService
{
    public List<Box> GetBoxes()
    {
        using FlashCardsContext context = new();
        return [.. context.Boxes.OrderBy(box => box.Number)];
    }

    public int GetBoxID(int number)
    {
        using FlashCardsContext context = new();
        return context.Boxes.FirstOrDefault(box => box.Number == number)!.Id;
    }

    public int AddBox(int number, DueAfterOptions dueAfter)
    {
        using FlashCardsContext context = new();
        Box newBox = new() { Number = number, DueAfter = dueAfter };
        context.Boxes.Add(newBox);
        context.SaveChanges();
        return newBox.Id;
    }

    public void EditBox(int id, DueAfterOptions dueAfter)
    {
        using FlashCardsContext context = new();
        var box = context.Boxes.Find(id);
        if (box is null)
        {
            return;
        }

        box.DueAfter = dueAfter;
        context.SaveChanges();
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

    public List<Subject> GetSubjects()
    {
        using FlashCardsContext context = new();
        return [.. context.Subjects];
    }

    public Subject? GetSubject(int id)
    {
        using FlashCardsContext context = new();
        return context.Subjects.Find(id);
    }

    public int AddSubject(string name)
    {
        using FlashCardsContext context = new();
        Subject newSubject = new() { Name = name };
        context.Subjects.Add(newSubject);
        context.SaveChanges();
        return newSubject.Id;
    }

    public void EditSubject(int id, string name)
    {
        using FlashCardsContext context = new();
        var subject = context.Subjects.Find(id);
        if (subject is null)
        {
            return;
        }

        subject.Name = name;
        context.SaveChanges();
    }

    public void DeleteSubject(int id)
    {
        using FlashCardsContext context = new();
        var subject = context.Subjects.Find(id);
        if (subject is null)
        {
            return;
        }

        context.Subjects.Remove(subject);
        context.SaveChanges();
    }

    public List<Tag> GetTags()
    {
        using FlashCardsContext context = new();
        return [.. context.Tags];
    }

    public int AddTag(string name)
    {
        using FlashCardsContext context = new();
        Tag newTag = new() { Name = name };
        context.Tags.Add(newTag);
        context.SaveChanges();
        return newTag.Id;
    }

    public void EditTag(int id, string name)
    {
        using FlashCardsContext context = new();
        var tag = context.Tags.Find(id);
        if (tag is null)
        {
            return;
        }

        tag.Name = name;
        context.SaveChanges();
    }

    public void DeleteTag(int id)
    {
        using FlashCardsContext context = new();
        var tag = context.Tags.Find(id);
        if (tag is null)
        {
            return;
        }

        context.Tags.Remove(tag);
        context.SaveChanges();
    }

    public List<int> GetDueFlashCardIDs()
    {
        using FlashCardsContext context = new();
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        return [.. context.FlashCards
            .AsEnumerable()
            .Where(flashCard => flashCard.LastReviewDate == null || flashCard.LastReviewDate.Value.AddDays(GetDaysFromDueAfter(flashCard.Box.DueAfter)) <= today)
            .Select(flashCard => flashCard.Id)
            .OrderBy(ID => Guid.NewGuid())];
    }

    private static int GetDaysFromDueAfter(DueAfterOptions dueAfter)
    {
        return dueAfter switch
        {
            DueAfterOptions.OneDay => 1,
            DueAfterOptions.TwoDays => 2,
            DueAfterOptions.ThreeDays => 3,
            DueAfterOptions.FourDays => 4,
            DueAfterOptions.FiveDays => 5,
            DueAfterOptions.SixDays => 6,
            DueAfterOptions.OneWeek => 7,
            DueAfterOptions.TwoWeeks => 14,
            DueAfterOptions.ThreeWeeks => 21,
            DueAfterOptions.FourWeeks => 28,
            DueAfterOptions.OneMonth => 30,
            DueAfterOptions.TwoMonths => 60,
            DueAfterOptions.ThreeMonths => 90,
            DueAfterOptions.SixMonths => 180,
            DueAfterOptions.OneYear => 365,
            DueAfterOptions.TwoYears => 730,
            DueAfterOptions.ThreeYears => 1095,
            DueAfterOptions.Never => int.MaxValue, // Never due
            _ => throw new ArgumentOutOfRangeException(nameof(dueAfter), dueAfter, null)
        };
    }

    public FlashCard? GetFlashCard(int id)
    {
        using FlashCardsContext context = new();
        return context.FlashCards.Find(id);
    }

    public int AddFlashCard(FlashCard flashCard)
    {
        using FlashCardsContext context = new();
        context.FlashCards.Add(flashCard);
        context.SaveChanges();
        return flashCard.Id;
    }
}
