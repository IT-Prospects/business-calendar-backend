namespace Model.DTO
{
    public class UserSignInDTO : IUserDTO
    {
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
