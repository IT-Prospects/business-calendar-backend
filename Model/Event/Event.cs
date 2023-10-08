namespace Model
{
    public class Event
    {
        public Event()
        {
            Title = string.Empty;
            ShortDescription = string.Empty;
            Description = string.Empty;
            FullDescription = string.Empty;
            Topics = new HashSet<string>();
            Address = string.Empty;
            EventDates = new HashSet<DateTime>();
            Image = null;
            Company = null;
            City = null;
        }

        public Event(
            string title,
            string shortDescription,
            string description,
            string fullDescription,
            ICollection<string> topics,
            string address,
            ICollection<DateTime> eventDates,
            Image image,
            Company company,
            City city
            )
        {
            Title = title;
            ShortDescription = shortDescription;
            Description = description;
            FullDescription = fullDescription;
            Topics = topics;
            Address = address;
            EventDates = eventDates;
            Image = image;
            Image_Id = image.Id;
            Company = company;
            Company_Id = company.Id;
            City = city;
            City_Id = city.Id;
        }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string FullDescription { get; set; }

        public ICollection<string> Topics { get; set; }

        public string Address { get; set; }

        public ICollection<DateTime> EventDates { get; set; }

        public Image Image { get; set; }

        public long Image_Id { get; set; }

        public Company Company { get; set; }

        public long Company_Id { get; set; }

        public City City { get; set; }

        public long City_Id { get; set; }
    }
}