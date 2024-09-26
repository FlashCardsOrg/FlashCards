using FlashCards.DBModels;

namespace FlashCards.Contracts.Services;

public interface IDatabaseService
{
    List<Box> GetBoxes();
    int GetBoxID(int number);
    int AddBox(int number, DueAfterOptions dueAfter);
    void EditBox(int id, DueAfterOptions dueAfter);
    void DeleteBox(int id);

    List<Subject> GetSubjects();
    Subject? GetSubject(int id);
    int AddSubject(string name);
    void EditSubject(int id, string name);
    void DeleteSubject(int id);

    List<Tag> GetTags();
    int AddTag(string name);
    void EditTag(int id, string name);
    void DeleteTag(int id);

    List<int> GetDueFlashCardIDs();
    FlashCard? GetFlashCard(int id);
    int AddFlashCard(FlashCard flashCard);
}
