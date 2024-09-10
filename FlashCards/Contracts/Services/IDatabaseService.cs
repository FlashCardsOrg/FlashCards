using FlashCards.DBModels;

namespace FlashCards.Contracts.Services;

public interface IDatabaseService
{
    List<Box> GetBoxes();
    int AddBox(int number, DueAfterOptions dueAfter);
    void UpdateBox(int id, DueAfterOptions dueAfter);
    void DeleteBox(int id);

    List<Subject> GetSubjects();
    int AddSubject(string name);
    void DeleteSubject(int id);

    List<Tag> GetTags();
    int AddTag(string name);
    void DeleteTag(int id);
}
