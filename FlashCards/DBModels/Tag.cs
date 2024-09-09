namespace FlashCards.DBModels;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation property for many-to-many relationship with FlashCard
    public ICollection<FlashCard> FlashCards { get; set; } = null!;
}
