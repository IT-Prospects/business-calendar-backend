using Model.DTO;

namespace Model
{
    public class Event : BaseObject
    {
        #region props

        public string Title { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan EventDuration { get; set; } 

        public Image Image { get; set; }

        public long Image_Id { get; set; }

        #endregion

        #region ctors

        public Event()
        {
            Title = string.Empty;
            Description = string.Empty;
            Address = string.Empty;
            Image = null;
        }

        public Event(
            string title,
            string description,
            string address,
            DateTime eventDate,
            TimeSpan eventDuration,
            Image image
            )
        {
            Title = title;
            Description = description;
            Address = address;
            EventDate = eventDate;
            EventDuration = eventDuration;
            Image = image;
            Image_Id = image.Id;
        }

        public Event(EventDTO eventDTO)
        {
            Id = eventDTO.Id ?? 0;
            Title = eventDTO.Title;
            Description = eventDTO.Description;
            Address = eventDTO.Address;
            EventDate = eventDTO.EventDate.Value;
            EventDuration = eventDTO.EventDuration.Value;
            Image = eventDTO.Image;
            Image_Id = eventDTO.Image_Id.Value;
        }

        #endregion
    }
}