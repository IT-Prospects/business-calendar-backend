using BusinessCalendar.Common;
using DAL;
using DAL.Common;
using DAL.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using Model.Enum;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace BusinessCalendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private readonly EventDAO _eventDAO;
        private readonly ImageDAO _imageDAO;
        private readonly UnitOfWork _unitOfWork;

        public EventController(ILogger<EventController> logger, UnitOfWork uow)
        {
            _logger = logger;
            _eventDAO = new EventDAO(uow);
            _imageDAO = new ImageDAO(uow);
            _unitOfWork = uow;
        }

        [HttpGet]
        [Route("targetdate={targetDate}&offset={offset}")]
        public IActionResult Get(DateTime targetDate, int offset)
        {
            try
            {
                var param = new EventFilterParam(targetDate, offset);
                return Ok(new ResponseObject(_eventDAO.GetEventsByDate(param)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ex.Message));
            }
        }

        [HttpGet]
        [Route("currentdate={currentDate}")]
        public IActionResult Get(DateTime currentDate)
        {
            try
            {
                var param = new EventAnnouncementParam(currentDate);
                return Ok(new ResponseObject(_eventDAO.GetAnnounceEvents(param)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ex.Message));
            }
        }

        [HttpGet]
        [Route("id={id}")]
        public IActionResult Get(long id)
        {
            try
            {
                return Ok(new ResponseObject(_eventDAO.GetById(id)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ex.Message));
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post(EventDTO itemDTO)
        {
            try
            {
                if (!IsValidateDomainObject(itemDTO, ControllerAction.Create, out string message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = MappingToDomainObject(itemDTO);
                var newItem = _eventDAO.Create();
                SetValues(item, newItem);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(newItem));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ex.Message));
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult Put(EventDTO itemDTO)
        {
            try
            {
                if (!IsValidateDomainObject(itemDTO, ControllerAction.Update, out string message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = MappingToDomainObject(itemDTO);
                var oldItem = _eventDAO.GetItemForUpdate(itemDTO.Id!.Value);
                SetValues(item, oldItem);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(oldItem));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ex.Message));
            }
        }

        [HttpDelete]
        [Route("id={id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var delEvent = _eventDAO.GetById(id);
                _eventDAO.Delete(id);
                _imageDAO.Delete(delEvent.Image_Id);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ex.Message));
            }
        }

        private void SetValues(Event src, Event dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private bool IsValidateDomainObject(EventDTO item, ControllerAction controllerAction, out string message)
        {
            if (controllerAction == ControllerAction.Update && (!item.Id.HasValue || item.Id.Value == 0))
            {
                message = "Event ID is missing";
                return false;
            }

            var stringBuilder = new StringBuilder(string.Empty);
            var requiredFieldErrorMessageTemplate = "Required field \"{0}\" is not filled in";

            if (string.IsNullOrWhiteSpace(item.Title))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.Title)));

            if (string.IsNullOrWhiteSpace(item.Description))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.Description)));

            if (string.IsNullOrWhiteSpace(item.Address))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.Address)));

            if (!item.EventDate.HasValue)
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.EventDate)));

            if (!item.EventDuration.HasValue)
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.EventDuration)));

            if (!item.Image_Id.HasValue)
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, nameof(item.Image_Id)));

            message = stringBuilder.ToString();

            return message == string.Empty;
        }

        private Event MappingToDomainObject(EventDTO itemDTO)
        {
            var image = _imageDAO.GetById(itemDTO.Image_Id!.Value);
            return new Event
                    (
                        itemDTO.Title!,
                        itemDTO.Description!,
                        itemDTO.Address!,
                        itemDTO.EventDate!.Value,
                        itemDTO.EventDuration!.Value,
                        image
                    )
            {
                Id = itemDTO.Id ?? 0
            };
        }
    }
}