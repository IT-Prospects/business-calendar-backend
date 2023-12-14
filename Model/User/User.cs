namespace Model
{
    public class User : DomainObject
    {
        #region props

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string? RefreshToken { get; set; }

        #endregion

        #region ctors

        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
        }

        public User(
            string firstName,
            string lastName,
            string email,
            string passwordHash,
            string? refreshToken = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            RefreshToken = refreshToken;
        }

        #endregion
    }
}
