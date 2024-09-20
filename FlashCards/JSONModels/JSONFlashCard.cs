using FlashCards.ViewModels;

namespace FlashCards.JSONModels;

public class JSONFlashCard
{
    public FlashCardSide Front { get; set; }
    public FlashCardSide Back { get; set; }

    public JSONFlashCard()
    {
        Front = new FlashCardSide();
        Back = new FlashCardSide();
    }
}

public class FlashCardSide
{
    public Layouts Layout { get; set; }
    public bool ShowBulletPointsIndividually { get; set; }
}
