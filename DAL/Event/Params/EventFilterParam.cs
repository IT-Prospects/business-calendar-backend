using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Params
{
    public class EventFilterParam
    {
        public DateTime? CurrentDate { get; set; }

        public DateTime? TargetDate { get; set; }

        public int? Offset { get; set; }

        public EventFilterParam() { }

        public EventFilterParam(DateTime? currentDate, DateTime? targetDate, int? offset)
        {
            CurrentDate = currentDate;
            TargetDate = targetDate;
            Offset = offset;
        }
    }
}
