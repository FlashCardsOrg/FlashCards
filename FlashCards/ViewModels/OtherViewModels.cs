using FlashCards.Contracts.Services;
using System.ComponentModel;

namespace FlashCards.ViewModels;

public class VMFlashCard : INotifyPropertyChanged
{
    private static readonly ICreateSettingsService _createSettingsService = App.GetService<ICreateSettingsService>();
    private readonly IDatabaseService _dbService = App.GetService<IDatabaseService>();

    private int _semester = _createSettingsService.SelectedSemester;
    private int _boxNumber = 1;
    private int _subjectID = _createSettingsService.SelectedSubjectID;
    private VMFlashCardSide _front = new();
    private VMFlashCardSide _back = new();

    public DateOnly? LastReviewDate { get; set; } = null;
    public List<int> TagIDs { get; set; } = _createSettingsService.SelectedTagIDs;

    public int Semester
    {
        get => _semester;
        set
        {
            _semester = value;
            OnPropertyChanged(nameof(Semester));
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }
    public int BoxNumber
    {
        get => _boxNumber;
        set
        {
            _boxNumber = value;
            OnPropertyChanged(nameof(BoxNumber));
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }
    public int SubjectID
    {
        get => _subjectID;
        set
        {
            _subjectID = value;
            OnPropertyChanged(nameof(SubjectID));
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }
    public VMFlashCardSide Front
    {
        get => _front;
        set
        {
            _front = value;
            OnPropertyChanged(nameof(Front));
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }
    public VMFlashCardSide Back
    {
        get => _back;
        set
        {
            _back = value;
            OnPropertyChanged(nameof(Back));
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }

    public bool CanBeSaved => IsComplete() && Front.IsComplete() && Back.IsComplete();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool IsComplete()
    {
        return Semester > 0 && BoxNumber > 0 && _dbService.GetSubject(SubjectID) is not null;
    }

    public class VMFlashCardSide : INotifyPropertyChanged
    {
        private Layouts _layout = Layouts.Text;
        private string? _content1;
        private string? _content2;
        private string? _content3;

        public bool ShowBulletPointsIndividually { get; set; } = false;

        public Layouts Layout
        {
            get => _layout;
            set
            {
                _layout = value;
                OnPropertyChanged(nameof(Layout));
                OnPropertyChanged(nameof(IsComplete));
            }
        }
        public string? Content1
        {
            get => _content1;
            set
            {
                _content1 = value;
                OnPropertyChanged(nameof(Content1));
                OnPropertyChanged(nameof(IsComplete));
            }
        }
        public string? Content2
        {
            get => _content2;
            set
            {
                _content2 = value;
                OnPropertyChanged(nameof(Content2));
                OnPropertyChanged(nameof(IsComplete));
            }
        }
        public string? Content3
        {
            get => _content3;
            set
            {
                _content3 = value;
                OnPropertyChanged(nameof(Content3));
                OnPropertyChanged(nameof(IsComplete));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
