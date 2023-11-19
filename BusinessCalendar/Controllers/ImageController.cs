using DAL.Common;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model;
using BusinessCalendar.Common;
using Microsoft.EntityFrameworkCore;
using BusinessCalendar.Helpers;
using Model.DTO;

namespace BusinessCalendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        private readonly ImageDAO _imageDAO;
        private readonly EventDAO _eventDAO;
        private readonly UnitOfWork _unitOfWork;

        public ImageController(ILogger<ImageController> logger, UnitOfWork uow)
        {
            _logger = logger;
            _imageDAO = new ImageDAO(uow);
            _eventDAO = new EventDAO(uow);
            _unitOfWork = uow;
        }

        [HttpGet]
        [Route("event_id={event_Id}")]
        public IActionResult GetAdditionalImageByEventId(long event_Id)
        {
            try
            {
                var result = _imageDAO.GetSubImagesByEventId(event_Id);
                return Ok(new ResponseObject(result.Select(MappingToDTO)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        [HttpPost]
        public IActionResult Post(IFormFile formFile)
        {
            var name = string.Empty;
            try
            {
                (var newItem, name) = AddImage(formFile);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(MappingToDTO(newItem)));
            }
            catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
            {
                if(!string.IsNullOrEmpty(name))
                    ImageFileHelper.DeleteImage(name);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        [HttpPost]
        [Route("Event")]
        public IActionResult AddImageForEvent(IFormFile formFile, long event_Id)
        {
            var name = string.Empty;
            try
            {
                (var newItem, name) = AddImage(formFile, event_Id);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(MappingToDTO(newItem)));
            }
            catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
            {
                if (!string.IsNullOrEmpty(name))
                    ImageFileHelper.DeleteImage(name);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        private (Image, string) AddImage(IFormFile formFile, long? event_Id = null)
        {
            var yandexStorageURL = "https://storage.yandexcloud.net";
            string name;
            var fileExtension = GetFileExtension(formFile.FileName);
            var bucketName = ConfigurationHelper.GetString("YCBucketName");
            var item = new Image(
                $"{yandexStorageURL}/{bucketName}/{Guid.NewGuid()}{fileExtension}",
                event_Id.HasValue ? _eventDAO.GetById(event_Id.Value) : null);
            ImageFileHelper.SaveImage(formFile, name = item.URL.Split('/').Last());

            var newItem = _imageDAO.Create();
            SetValues(item, newItem);
            return (newItem, name);
        }

        private void SetValues(Image src, Image dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private string GetFileExtension(string fileName)
        {
            return fileName[fileName.LastIndexOf('.')..];
        }

        private ImageDTO MappingToDTO(Image image)
        {
            return new ImageDTO()
            {
                Id = image.Id,
                URL = image.URL,
                Event_Id = image.Event_Id,
            };
        }
    }
}
