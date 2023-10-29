using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EventSignUp : DomainObject
    {
        #region props

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Patronymic { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Event? Event { get; set; }

        public long Event_Id { get; set; }

        #endregion

        #region ctors

        public EventSignUp()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Patronymic = null;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Event = null;
            Event_Id = 0;
        }

        public EventSignUp(
            string firstName,
            string lastName,
            string? patronymic,
            string email,
            string phoneNumber,
            Event ev)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            Email = email;
            PhoneNumber = phoneNumber;
            Event = ev;
            Event_Id = ev.Id;
        }

        #endregion
    }
}
