using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User : DomainObject
    {
        #region props

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Patronymic { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        #endregion

        #region ctors

        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Patronymic = null;
            Email = string.Empty;
            PhoneNumber = string.Empty;
        }

        public User(
            string firstName,
            string lastName,
            string? patronymic,
            string email,
            string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        #endregion
    }
}
