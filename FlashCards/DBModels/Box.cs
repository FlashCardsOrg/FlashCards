namespace FlashCards.DBModels;

public class Box
{
    public int Id { get; set; }
    public string DueAfter { get; set; } = null!;

    // Navigation property for 1-to-many relationship with FlashCard
    public ICollection<FlashCard> FlashCards { get; set; } = null!;
}
