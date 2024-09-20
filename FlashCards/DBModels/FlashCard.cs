namespace FlashCards.DBModels;

public class FlashCard
{
    public int Id { get; set; }
    public DateOnly? LastReviewDate { get; set; }
    public int Semester { get; set; }

    // Foreign Keys
    public int BoxId { get; set; }
    public Box Box { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    // Navigation property for many-to-many relationship with Tag
    public ICollection<Tag> Tags { get; set; } = null!;
}
