using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class EventDTO
    {
        public long? Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public DateTime? EventDate { get; set; }

        public TimeSpan? EventDuration { get; set; }

        public long? Image_Id { get; set; }

        public string? ImageName { get; set; }
    }
}
