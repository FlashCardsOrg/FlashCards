using FlashCards.DBModels;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Data;
public class FlashCardsContext : DbContext
{
    public DbSet<FlashCard> FlashCards { get; set; } = null!;
    public DbSet<Box> Boxes { get; set; } = null!;
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Flashcards.db");
    }
}
