using System.Security.Cryptography;
using DAL.Common;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DAL;
using Model;
using Model.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BusinessCalendar.Helpers
{
    public static class AuthHelper
    {
        private const string _userDAONotInitializedError = "User DAO was not initialized.";
        public static string Issuer { get; private set; } = string.Empty;

        public static string Audience { get; private set; } = string.Empty;

        public static int Lifetime { get; private set; } = 0;

        private static string _key = string.Empty;

        private static UserDAO? _userDAO;

        public static void InitConfiguration()
        {
            Issuer = ConfigurationHelper.GetString("TokenIssuer");
            Audience = ConfigurationHelper.GetString("TokenAudience");
            Lifetime = ConfigurationHelper.GetInt("TokenLifetime");
            _key = ConfigurationHelper.GetString("TokenKey");
        }

        public static void InitDAO(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        }

        public static bool AuthenticateUser(UserSignInDTO userDTO, out string message, out User? user)
        {
            if (_userDAO == null) 
                throw new Exception(_userDAONotInitializedError);
            user = _userDAO.GetByEmail(userDTO.Email!);

            message = string.Empty;

            if (user == null)
            {
                message = "Bad user email";
                return false;
            }

            using var sha256 = SHA256.Create();
            var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password!)));
            if (hashedPassword == user.Password)
                return true;

            message = "Bad user password";
            return false;
        }

        public static (string, string) AuthorizeUser(User user)
        {
            var token = GetToken(user);
            user.RefreshToken = GetRefreshToken();

            return (token, user.RefreshToken);
        }

        public static (string, string) AuthorizeUser(string token, string refreshToken)
        {
            if (_userDAO == null)
                throw new Exception(_userDAONotInitializedError);

            var jwt = new JwtSecurityToken(token);
            var userId = long.Parse(jwt.Claims.First().Value);
            var user = _userDAO.GetById(userId);

            if (user.RefreshToken != refreshToken)
                throw new SecurityTokenNotYetValidException("Refresh token is not valid.");

            token = GetToken(user);

            user.RefreshToken = GetRefreshToken();
            return (token, user.RefreshToken);
        }

        private static string GetToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new(ClaimsIdentity.DefaultRoleClaimType, "user")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var jwt = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                notBefore: DateTime.UtcNow,
                claims: claimsIdentity.Claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static string GetRefreshToken()
        {
            var alphabet = Enumerable.Range(65, 26).Concat(Enumerable.Range(97, 26)).ToArray();
            var rand = new Random((int)DateTime.Now.Ticks);
            return string.Join(string.Empty, new char[128].Select(_ => (char)alphabet[rand.Next(alphabet.Length)]));
        }
    }
}
