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

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }
        public string? RefreshToken { get; set; }

        #endregion

        #region ctors

        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Password = string.Empty;
        }

        public User(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
        }

        public User(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string password,
            string refreshToken)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            RefreshToken = refreshToken;
        }

        #endregion
    }
}
