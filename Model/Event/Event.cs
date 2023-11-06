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

        public string ArchivePassword { get; set; }

        public Image? Image { get; set; }

        public long Image_Id { get; set; }

        public List<Image> SubImages { get; set; }

        #endregion

        #region ctors

        public Event()
        {
            Title = string.Empty;
            Description = string.Empty;
            Address = string.Empty;
            EventDate = DateTime.MinValue;
            EventDuration = TimeSpan.Zero;
            ArchivePassword = string.Empty;
            Image = null;
            Image_Id = 0;
            SubImages = new List<Image>();
        }

        public Event(
            string title,
            string description,
            string address,
            DateTime eventDate,
            TimeSpan eventDuration,
            string archivePassword,
            Image image,
            List<Image> subImages
            )
        {
            Title = title;
            Description = description;
            Address = address;
            EventDate = eventDate;
            EventDuration = eventDuration;
            ArchivePassword = archivePassword;
            Image = image;
            Image_Id = image.Id;
            SubImages = subImages;
        }

        #endregion
    }
}