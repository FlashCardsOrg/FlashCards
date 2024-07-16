using FlashCards.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace FlashCards.Views;

public sealed partial class ProgressPage : Page
{
    public ProgressViewModel ViewModel
    {
        get;
    }

    public ProgressPage()
    {
        ViewModel = App.GetService<ProgressViewModel>();
        InitializeComponent();
    }
}
