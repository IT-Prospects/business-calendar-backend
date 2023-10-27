namespace DAL.Params
{
    public class EventFilterParam
    {
        public DateTime TargetDate { get; set; }

        public int Offset { get; set; }

        public EventFilterParam(DateOnly targetDate, int offset)
        {
            TargetDate = targetDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            Offset = offset;
        }
    }
}
