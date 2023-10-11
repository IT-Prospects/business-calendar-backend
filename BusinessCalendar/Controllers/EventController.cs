using BusinessCalendar.Common;
using DAL;
using DAL.Common;
using DAL.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using System.Text;

namespace BusinessCalendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private readonly EventDAO _eventDAO;
        private readonly UnitOfWork _unitOfWork;

        public EventController(ILogger<EventController> logger, UnitOfWork uow)
        {
            _logger = logger;
            _eventDAO = new EventDAO(uow);
            _unitOfWork = uow;
        }

        [HttpGet]
        [Route("targetdate={targetDate}&offset={offset}")]
        public IActionResult Get(DateTime? targetDate, int? offset)
        {
            try
            {
                var param = new EventFilterParam(null, targetDate, offset);
                return Ok(new ResponseObject(_eventDAO.GetAllByFilter(param)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ex.Message));
            }
        }

        [HttpGet]
        [Route("currentdate={currentDate}")]
        public IActionResult Get(DateTime? currentDate)
        {
            try
            {
                var param = new EventFilterParam(currentDate, null, null);
                return Ok(new ResponseObject(_eventDAO.GetAllByFilter(param)));
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
                if (!IsValidate(itemDTO, out string message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = new Event(itemDTO);
                var newItem = _eventDAO.Create();
                SetValues(item, newItem);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(newItem));
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject (ex.Message));
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult Put(EventDTO itemDTO)
        {
            try
            {
                if (!IsValidate(itemDTO, out string message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = new Event(itemDTO);
                var oldItem = _eventDAO.GetItemForUpdate(item.Id);
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
                _eventDAO.Delete(id);
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

        private bool IsValidate(EventDTO item, out string message)
        {
            var stringBuilder = new StringBuilder(string.Empty);
            var requiredFieldErrorMessageTemplate = "Не заполнено обязательное поле {0}";

            if (string.IsNullOrWhiteSpace(item.Title))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, "Название"));
            if (string.IsNullOrWhiteSpace(item.Description))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, "Описание"));
            if (string.IsNullOrWhiteSpace(item.Address))
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, "Адрес проведения"));
            if (!item.EventDate.HasValue)
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, "Дата и время проведения"));
            if (!item.EventDuration.HasValue)
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, "Длительность"));
            if (!item.Image_Id.HasValue)
                stringBuilder.AppendLine(string.Format(requiredFieldErrorMessageTemplate, "Изображение"));

            message = stringBuilder.ToString();

            return message == string.Empty;
        }
    }
}