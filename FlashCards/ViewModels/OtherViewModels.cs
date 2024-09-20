namespace FlashCards.ViewModels;

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
