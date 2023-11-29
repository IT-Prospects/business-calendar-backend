using BusinessCalendar.Common;
using BusinessCalendar.Helpers;
using DAL;
using DAL.Common;
using DAL.Params;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using Model.Enum;
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
        public IActionResult Get(DateOnly targetDate, int offset)
        {
            try
            {
                var param = new EventFilterParam(targetDate, offset);
                var result = _eventDAO.GetEventsByDate(param).Select(MappingToDTO);
                return Ok(new ResponseObject(result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        [HttpGet]
        [Route("currentdate={currentDate}")]
        public IActionResult Get(DateTime currentDate)
        {
            try
            {
                var param = new EventAnnouncementParam(currentDate);
                var result = _eventDAO.GetAnnounceEvents(param).Select(MappingToDTO);
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
                return Ok(new ResponseObject(MappingToDTO(_eventDAO.GetById(id))));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post(EventDTO itemDTO)
        {
            try
            {
                if (!IsValidDTO(itemDTO, ControllerAction.Create, out var message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = MappingToDomainObject(itemDTO);

                var rand = new Random((int)DateTime.Now.Ticks);
                item.ArchivePassword = string.Join(string.Empty, new char[12].Select(_ => (char)rand.Next(0x0021, 0x007E)));

                var newItem = _eventDAO.Create();
                SetValues(item, newItem);
                _unitOfWork.SaveChanges();

                newItem.Image!.Event = newItem;
                _unitOfWork.SaveChanges();

                return Ok(new ResponseObject(MappingToDTO(newItem)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult Put(EventDTO itemDTO)
        {
            try
            {
                if (!IsValidDTO(itemDTO, ControllerAction.Update, out var message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = MappingToDomainObject(itemDTO);
                var oldItem = _eventDAO.GetItemForUpdate(itemDTO.Id!.Value);
                item.ArchivePassword = oldItem.ArchivePassword;
                if (item.Image_Id != oldItem.Image_Id)
                {
                    return BadRequest(new ResponseObject("It is forbidden to change the main image"));
                }

                SetValues(item, oldItem);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(MappingToDTO(oldItem)));
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
                var delImageURLs = _imageDAO.GetAllImageURLsByEventId(id);
                _eventDAO.Delete(id);
                _unitOfWork.SaveChanges();

                foreach (var name in delImageURLs)
                {
                    ImageFileHelper.DeleteImage(name);
                }

                return Ok(new ResponseObject(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        private void SetValues(Event src, Event dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private bool IsValidDTO(EventDTO item, ControllerAction controllerAction, out string message)
        {
            if (controllerAction == ControllerAction.Update && item.Id is null or 0)
            {
                message = "Event ID is missing";
                return false;
            }

            var stringBuilder = new StringBuilder(string.Empty);
            const string requiredFieldErrorMessageTemplate = "Required field \"{0}\" is not filled in";

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
            
            return new Event
                    (
                        itemDTO.Title!,
                        itemDTO.Description!,
                        itemDTO.Address!,
                        itemDTO.EventDate!.Value,
                        itemDTO.EventDuration!.Value,
                        itemDTO.ArchivePassword ?? string.Empty,
                        _imageDAO.GetById(itemDTO.Image_Id!.Value),
                        itemDTO.Id.HasValue ? _imageDAO.GetSubImagesByEventId(itemDTO.Id.Value).ToList() : new List<Image>()
                    )
            {
                Id = itemDTO.Id ?? 0
            };
        }

        private EventDTO MappingToDTO(Event item)
        {
            return new EventDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Address = item.Address,
                EventDate = item.EventDate,
                EventDuration = item.EventDuration,
                ArchivePassword = item.ArchivePassword,
                Image_Id = item.Image!.Id,
                ImageURL = item.Image!.URL
            };
        }
    }
}