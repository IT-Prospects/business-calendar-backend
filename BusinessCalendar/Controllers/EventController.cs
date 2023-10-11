using DAL;
using DAL.Common;
using DAL.Params;
using Microsoft.AspNetCore.Mvc;
using Model;

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
        [Route("")]
        public IActionResult Get(DateTime? currentDate, DateTime? targetDate, int? offset)
        {
            var param = new EventFilterParam(currentDate, targetDate, offset);
            return Ok(_eventDAO.GetAll(param));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            return Ok(_eventDAO.GetById(id));
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post(Event item)
        {
            var newItem = _eventDAO.Create();
            SetValues(item, newItem);
            _unitOfWork.SaveChanges();
            return Ok(newItem);
        }

        [HttpPut]
        [Route("")]
        public IActionResult Put(Event item)
        {
            var oldItem = _eventDAO.GetById(item.Id);
            SetValues(item, oldItem);
            _unitOfWork.SaveChanges();
            return Ok(oldItem);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(long id)
        {
            _eventDAO.Delete(id);
            _unitOfWork.SaveChanges();
            return Ok(id);
        }

        private void SetValues(Event src, Event dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }
    }
}