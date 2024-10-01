using CommunityToolkit.Mvvm.ComponentModel;
using FlashCards.Contracts.Services;

namespace FlashCards.ViewModels;

public partial class VMFlashCard : ObservableRecipient
{
    private static readonly ICreateSettingsService _createSettingsService = App.GetService<ICreateSettingsService>();
    private readonly IDatabaseService _dbService = App.GetService<IDatabaseService>();

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _boxNumber = 1;

    [ObservableProperty]
    private int _semester = _createSettingsService.SelectedSemester;

    [ObservableProperty]
    private DateOnly? _lastReviewDate = null;

    [ObservableProperty]
    private int _subjectID = _createSettingsService.SelectedSubjectID;

    [ObservableProperty]
    private List<int> _tagIDs = _createSettingsService.SelectedTagIDs;

    [ObservableProperty]
    private bool _canBeSaved;

    [ObservableProperty]
    private bool _wasFlipped = false;

    [ObservableProperty]
    private FlashCardSides _currentSide = FlashCardSides.Front;

    [ObservableProperty]
    private VMFlashCardSide _front = new();

    [ObservableProperty]
    private VMFlashCardSide _back = new();

    public VMFlashCard()
    {
        // TODO: Fix CanBeSaved: _canBeSaved = GetCanBeSaved();
        _canBeSaved = true;
    }

    private bool GetCanBeSaved()
    {
        return (Semester > 0 && BoxNumber > 0 && _dbService.GetSubject(SubjectID) is not null && Front.IsComplete() && Back.IsComplete());
    }

    public partial class VMFlashCardSide : ObservableRecipient
    {
        [ObservableProperty]
        private Layouts _layout = Layouts.Text;

        [ObservableProperty]
        private bool _showBulletPointsIndividually = false;

        [ObservableProperty]
        private string? _content1;

        [ObservableProperty]
        private string? _content2;

        [ObservableProperty]
        private string? _content3;

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

        public void ResetContent()
        {
            Content1 = Content2 = Content3 = null;
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
