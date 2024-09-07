namespace FlashCards.DBModels;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation property for 1-to-many relationship with FlashCard
    public ICollection<FlashCard> FlashCards { get; set; } = null!;
}
