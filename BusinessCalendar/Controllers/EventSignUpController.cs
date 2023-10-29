using BusinessCalendar.Common;
using BusinessCalendar.Helpers;
using DAL.Common;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Enum;
using Model;
using System.Text;

namespace BusinessCalendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventSignUpController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private readonly EventSignUpDAO _eventSignUpDAO;
        private readonly EventDAO _eventDAO;
        private readonly UnitOfWork _unitOfWork;

        public EventSignUpController(ILogger<EventController> logger, UnitOfWork uow)
        {
            _logger = logger;
            _unitOfWork = uow;
            _eventSignUpDAO = new EventSignUpDAO(uow);
            _eventDAO = new EventDAO(uow);
        }

        [HttpGet]
        [Route("event_id={event_Id}")]
        public IActionResult GetByEventId(long event_Id)
        {
            try
            {
                var result = _eventSignUpDAO.GetAllByEvent(event_Id).Select(MappingToDTO);
                return Ok(new ResponseObject(result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        [HttpGet]
        [Route("id={id}")]
        public IActionResult Get(long id)
        {
            try
            {
                return Ok(new ResponseObject(MappingToDTO(_eventSignUpDAO.GetById(id))));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post(EventSignUpDTO itemDTO)
        {
            try
            {
                if (!IsValidDTO(itemDTO, ControllerAction.Create, out var message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = MappingToDomainObject(itemDTO);
                
                var newItem = _eventSignUpDAO.Create();
                SetValues(item, newItem);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(MappingToDTO(newItem)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        [HttpDelete]
        [Route("id={id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                _eventSignUpDAO.Delete(id);
                _unitOfWork.SaveChanges();

                return Ok(new ResponseObject(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        private void SetValues(EventSignUp src, EventSignUp dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private bool IsValidDTO(EventSignUpDTO item, ControllerAction controllerAction, out string message)
        {
            var stringBuilder = new StringBuilder(string.Empty);
            const string requiredFieldErrorMessageTemplate = "Required field \"{0}\" is not filled in";

            if (string.IsNullOrWhiteSpace(item.FirstName))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.FirstName)));

            if (string.IsNullOrWhiteSpace(item.LastName))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.LastName)));

            if (string.IsNullOrWhiteSpace(item.Email))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.Email)));

            if (string.IsNullOrWhiteSpace(item.PhoneNumber))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.PhoneNumber)));

            if (!item.Event_Id.HasValue)
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.Event_Id)));

            message = stringBuilder.ToString();

            return message == string.Empty;
        }

        private EventSignUp MappingToDomainObject(EventSignUpDTO itemDTO)
        {
            return new EventSignUp
                    (
                        itemDTO.FirstName!,
                        itemDTO.LastName!,
                        itemDTO.Patronymic,
                        itemDTO.Email!,
                        itemDTO.PhoneNumber!,
                        _eventDAO.GetById(itemDTO.Event_Id!.Value)
                    );
        }

        private EventSignUpDTO MappingToDTO(EventSignUp item)
        {
            return new EventSignUpDTO
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Patronymic = item.Patronymic,
                Email = item.Email,
                PhoneNumber= item.PhoneNumber,
                Event_Id = item.Event_Id
            };
        }
    }
}
