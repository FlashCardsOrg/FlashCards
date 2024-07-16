using FlashCards.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace FlashCards.Views;

public sealed partial class CreatePage : Page
{
    public CreateViewModel ViewModel
    {
        get;
    }

    public CreatePage()
    {
        ViewModel = App.GetService<CreateViewModel>();
        InitializeComponent();
    }
}
