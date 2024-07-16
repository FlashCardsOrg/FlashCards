using FlashCards.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace FlashCards.Views;

public sealed partial class StudyPage : Page
{
    public StudyViewModel ViewModel
    {
        get;
    }

    public StudyPage()
    {
        ViewModel = App.GetService<StudyViewModel>();
        InitializeComponent();
    }
}
