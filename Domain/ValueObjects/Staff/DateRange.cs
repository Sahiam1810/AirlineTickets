namespace Domain.ValueObjects.Staff;

public sealed record DateRange
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    private DateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static DateRange Create(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be greater than start date", nameof(endDate));

        return new DateRange(startDate, endDate);
    }
}
