using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
