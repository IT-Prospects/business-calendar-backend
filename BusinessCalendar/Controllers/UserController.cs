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
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using _2DAL.Migrations;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BusinessCalendar.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn(UserSignInDTO itemDTO)
        {
            AuthHelper.InitDAO(_userDAO);
            if (!IsValidDTO(itemDTO, out var message) || !AuthHelper.AuthenticateUser(itemDTO, out message, out var user))
            {
                return BadRequest(new ResponseObject(message));
            }
            var (token, refreshToken) = AuthHelper.AuthorizeUser(user!);

            _unitOfWork.SaveChanges();
            return Ok(new ResponseObject(new { token, refreshToken }));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            AuthHelper.InitDAO(_userDAO);
            var token = HttpContext.Request.Headers.Authorization.ToString()[7..];
            (token, refreshToken) = AuthHelper.AuthorizeUser(token, refreshToken);

            _unitOfWork.SaveChanges();
            return Ok(new ResponseObject(new {token, refreshToken}));
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
}
