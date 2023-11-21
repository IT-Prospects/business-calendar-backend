using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using BusinessCalendar.Common;
using BusinessCalendar.Helpers;
using DAL.Common;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BusinessCalendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserDAO _userDAO;
        private readonly UnitOfWork _unitOfWork;

        public UserController(ILogger<UserController> logger, UnitOfWork uow)
        {
            _logger = logger;
            _unitOfWork = uow;
            _userDAO = new UserDAO(uow);
        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(UserDTO itemDTO)
        {
            try
            {
                if (!IsValidDTO(itemDTO, out var message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = MappingToDomainObject(itemDTO);

                if (_userDAO.HasDuplicateByEmailOrPhoneNumber(item.Email, item.PhoneNumber))
                {
                    return BadRequest("This email or phone number already registered");
                }
                using var sha256 = SHA256.Create();

                item.Password = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(item.Password)));

                var newItem = _userDAO.Create();
                SetValues(item, newItem);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(MappingToDTO(newItem)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }


        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn(UserSignInDTO itemDTO)
        {
            if (!IsValidDTO(itemDTO, out var message) || !AuthenticateUser(itemDTO, out message))
            {
                return BadRequest(new ResponseObject(message));
            }

            var user = _userDAO.GetByEmail(itemDTO.Email!)!;

            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new(ClaimsIdentity.DefaultRoleClaimType, "user")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow.AddMinutes(-500);
            var now2 = DateTime.UtcNow.AddMinutes(1000);
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: "BusinessCalendarBackend",
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now2,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            using var sha256 = SHA256.Create();
            user.RefreshToken = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(token)));
            _unitOfWork.SaveChanges();
            return Ok(new {user.Id, token, user.RefreshToken});
        }

        private bool AuthenticateUser(UserSignInDTO userDTO, out string message)
        {
            message = string.Empty;
            var user = _userDAO.GetByEmail(userDTO.Email!);
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

        [HttpDelete]
        [Route("id={id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                _userDAO.Delete(id);
                _unitOfWork.SaveChanges();

                return Ok(new ResponseObject(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        private void SetValues(User src, User dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private bool IsValidDTO(IUserDTO item, out string message)
        {
            var stringBuilder = new StringBuilder(string.Empty);
            const string requiredFieldErrorMessageTemplate = "Required field \"{0}\" is not filled in";


            if (item is UserDTO userDTO)
            {
                if (string.IsNullOrWhiteSpace(userDTO.FirstName))
                    stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(userDTO.FirstName)));

                if (string.IsNullOrWhiteSpace(userDTO.LastName))
                    stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(userDTO.LastName)));

                if (string.IsNullOrWhiteSpace(userDTO.PhoneNumber))
                    stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(userDTO.PhoneNumber)));
            }

            if (string.IsNullOrWhiteSpace(item.Email))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.Email)));

            if (string.IsNullOrWhiteSpace(item.Password))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.Password)));

            message = stringBuilder.ToString();

            return message == string.Empty;
        }

        private User MappingToDomainObject(UserDTO itemDTO)
        {
            return new User
                    (
                        itemDTO.FirstName!,
                        itemDTO.LastName!,
                        itemDTO.Email!,
                        itemDTO.PhoneNumber!,
                        itemDTO.Password!
                    );
        }

        private UserDTO MappingToDTO(User item)
        {
            return new UserDTO
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                PhoneNumber = item.PhoneNumber,
                Password = item.Password
            };
        }
    }

    public class AuthOptions
    {
       
    }
}
