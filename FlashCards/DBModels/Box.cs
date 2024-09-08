namespace FlashCards.DBModels;

public class Box
{
    public int Id { get; set; }
    public int Number { get; set; }
    public DueAfterOptions DueAfter { get; set; } = DueAfterOptions.OneDay;

    // Navigation property for 1-to-many relationship with FlashCard
    public ICollection<FlashCard> FlashCards { get; set; } = null!;
}

public enum DueAfterOptions
{
    OneDay,     // 1d
    TwoDays,    // 2d
    ThreeDays,  // 3d
    FourDays,   // 4d
    FiveDays,   // 5d
    SixDays,    // 6d
    OneWeek,    // 1w
    TwoWeeks,   // 2w
    ThreeWeeks, // 3w
    FourWeeks,  // 4w
    OneMonth,   // 1m
    TwoMonths,  // 2m
    ThreeMonths,// 3m
    SixMonths,  // 6m
    OneYear,    // 1y
    TwoYears,   // 2y
    ThreeYears, // 3y
    Never       // never
}
