using FlashCards.Contracts.Services;

namespace FlashCards.ViewModels;

public class VMFlashCard
{
    private static readonly ICreateSettingsService _createSettingsService = App.GetService<ICreateSettingsService>();
    private readonly IDatabaseService _dbService = App.GetService<IDatabaseService>();

    public int Id { get; set; }
    public int BoxNumber { get; set; } = 1; public int Semester { get; set; } = _createSettingsService.SelectedSemester;
    public DateOnly? LastReviewDate { get; set; } = null;
    public int SubjectID = _createSettingsService.SelectedSubjectID;
    public List<int> TagIDs { get; set; } = _createSettingsService.SelectedTagIDs;

    public bool CanBeSaved = true;

    // TODO: Fix WasFlipped
    public bool WasFlipped = false;
    // TODO: Fix CurrentSide
    public FlashCardSides CurrentSide = FlashCardSides.Front;

    public VMFlashCardSide Front = new();
    public VMFlashCardSide Back = new();

    // TODO: Fix CanBeSaved
    public void UpdateCanBeSaved()
    {
        CanBeSaved = Semester > 0 && BoxNumber > 0 && _dbService.GetSubject(SubjectID) is not null;
    }

    public class VMFlashCardSide
    {
        // TODO: Fix Layout
        public Layouts Layout { get; set; } = Layouts.Text;
        public bool ShowBulletPointsIndividually { get; set; } = false;
        public string? Content1 { get; set; } = "Test";
        public string? Content2 { get; set; } = "Test";
        public string? Content3 { get; set; }

        public bool IsComplete()
        {
            return Layout switch
            {
                Layouts.Text or Layouts.File => !string.IsNullOrWhiteSpace(Content1),
                Layouts.Text_Text or Layouts.File_File or Layouts.Text_File or Layouts.File_Text => !string.IsNullOrWhiteSpace(Content1) && !string.IsNullOrWhiteSpace(Content2),
                Layouts.Text_File_File => !string.IsNullOrWhiteSpace(Content1) && !string.IsNullOrWhiteSpace(Content2) && !string.IsNullOrWhiteSpace(Content3),
                _ => false,
            };
        }
    }
}

public class VMBox(int boxID, int number, int selectedIndex)
{
    public int BoxID { get; set; } = boxID;
    public string BoxName { get; set; } = $"{WinUI3Localizer.Localizer.Get().GetLocalizedString("Box")} {number}";
    public int SelectedIndex { get; set; } = selectedIndex;
}

public class VMSubject(int subjectID, string subjectName)
{
    public int SubjectID { get; set; } = subjectID;
    public string SubjectName { get; set; } = subjectName;
    public bool EditingState { get; set; } = false;
}

public class VMTag(int tagID, string tagName)
{
    public int TagID { get; set; } = tagID;
    public string TagName { get; set; } = tagName;
    public bool EditingState { get; set; } = false;
}

public enum FlashCardSides
{
    Front,
    Back
}

public enum Layouts
{
    Text,
    File,
    Text_Text,
    File_File,
    Text_File,
    File_Text,
    Text_File_File
}
