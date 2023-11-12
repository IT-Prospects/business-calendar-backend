namespace DAL.Params
{
    public class EventAnnouncementParam
    {
        public DateTime CurrentDateTime { get; set; }

        public EventAnnouncementParam(DateTime currentDateTime)
        {
            CurrentDateTime = currentDateTime;
        }
    }
}
