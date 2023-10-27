namespace Model
{
    public class Event : DomainObject
    {
        #region props

        public string Title { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan EventDuration { get; set; }

        public ICollection<Image> Images { get; set; }

        #endregion

        #region ctors

        public Event()
        {
            Title = string.Empty;
            Description = string.Empty;
            Address = string.Empty;
            EventDate = DateTime.MinValue;
            EventDuration = TimeSpan.Zero;
            Images = new HashSet<Image>();
        }

        public Event(
            string title,
            string description,
            string address,
            DateTime eventDate,
            TimeSpan eventDuration,
            ICollection<Image> images
            )
        {
            Title = title;
            Description = description;
            Address = address;
            EventDate = eventDate;
            EventDuration = eventDuration;
            Images = images;
        }

        #endregion
    }
}