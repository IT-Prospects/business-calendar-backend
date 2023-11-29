using System.Security.Cryptography;
using BusinessCalendar.Common;
using BusinessCalendar.Helpers;
using DAL.Common;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BusinessCalendar.Controllers
{
    [AllowAnonymous]
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

                item.PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(item.PasswordHash)));

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
            AuthHelper.InitDAO(_userDAO);
            if (!IsValidUserSignInDTO(itemDTO, out var message) || !AuthHelper.AuthenticateUser(itemDTO, out message, out var user))
            {
                return BadRequest(new ResponseObject(message));
            }
            var (token, refreshToken) = AuthHelper.AuthorizeUser(user!);

            _unitOfWork.SaveChanges();
            return Ok(new ResponseObject(new { token, refreshToken }));
        }

        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            AuthHelper.InitDAO(_userDAO);
            var authHeaderValue = HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authHeaderValue))
                return BadRequest("Required Authorization header is missing.");
            var token = authHeaderValue[7..];
            (token, refreshToken) = AuthHelper.AuthorizeUser(token, refreshToken);

            _unitOfWork.SaveChanges();
            return Ok(new ResponseObject(new { token, refreshToken }));
        }

        private void SetValues(User src, User dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private const string _requiredFieldErrorMessageTemplate = "Required field \"{0}\" is not filled in";

        private bool IsValidDTO(UserDTO item, out string message)
        {
            var stringBuilder = new StringBuilder(string.Empty);

            IsValidUserSignInDTO(new UserSignInDTO { Email = item.Email, Password = item.Password }, out var innerMessage);

            stringBuilder.Append(innerMessage);

            if (string.IsNullOrWhiteSpace(item.FirstName))
                stringBuilder.AppendLine(string.Format(_requiredFieldErrorMessageTemplate, nameof(item.FirstName)));

            if (string.IsNullOrWhiteSpace(item.LastName))
                stringBuilder.AppendLine(string.Format(_requiredFieldErrorMessageTemplate, nameof(item.LastName)));

            if (string.IsNullOrWhiteSpace(item.PhoneNumber))
                stringBuilder.AppendLine(string.Format(_requiredFieldErrorMessageTemplate, nameof(item.PhoneNumber)));

            message = stringBuilder.ToString();

            return message == string.Empty;
        }

        private bool IsValidUserSignInDTO(UserSignInDTO item, out string message)
        {
            var stringBuilder = new StringBuilder(string.Empty);

            if (string.IsNullOrWhiteSpace(item.Email))
                stringBuilder.AppendLine(string.Format(_requiredFieldErrorMessageTemplate, nameof(item.Email)));

            if (string.IsNullOrWhiteSpace(item.Password))
                stringBuilder.AppendLine(string.Format(_requiredFieldErrorMessageTemplate, nameof(item.Password)));

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
                PhoneNumber = item.PhoneNumber
            };
        }
    }
}
