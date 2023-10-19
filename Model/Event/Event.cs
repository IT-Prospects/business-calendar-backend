using Model.DTO;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

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

        public Image? Image { get; set; }

        public long Image_Id { get; set; }

        #endregion

        #region ctors

        public Event()
        {
            Title = string.Empty;
            Description = string.Empty;
            Address = string.Empty;
            EventDate = DateTime.MinValue;
            EventDuration = TimeSpan.Zero;
            Image = null;
            Image_Id = 0;
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

        #endregion
    }
}