using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Params
{
    public class EventAnnouncementParam
    {
        public DateTime CurrentDate { get; set; }

        public EventAnnouncementParam(DateTime currentDate)
        {
            CurrentDate = currentDate;
        }
    }
}
