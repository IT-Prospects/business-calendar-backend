using System.Security.Cryptography;
using BusinessCalendar.Common;
using BusinessCalendar.Helpers;
using DAL.Common;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model;
using System.Text;

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

                item.PasswordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(item.PasswordHash)));

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

        private void SetValues(User src, User dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private const string _requiredFieldErrorMessageTemplate = "Required field \"{0}\" is not filled in";

        private bool IsValidDTO(UserDTO item, out string message)
        {
            var stringBuilder = new StringBuilder(string.Empty);

            IsValidDTO(new UserSignInDTO { Email = item.Email, Password = item.Password }, out var innerMessage);

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

        private bool IsValidDTO(UserSignInDTO item, out string message)
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
