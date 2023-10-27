using DAL.Common;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model;
using BusinessCalendar.Common;
using Microsoft.EntityFrameworkCore;
using BusinessCalendar.Helpers;

namespace BusinessCalendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        private readonly ImageDAO _imageDAO;
        private readonly UnitOfWork _unitOfWork;

        public ImageController(ILogger<ImageController> logger, UnitOfWork uow)
        {
            _logger = logger;
            _imageDAO = new ImageDAO(uow);
            _unitOfWork = uow;
        }

        [HttpPost]
        public IActionResult Post(IFormFile formFile)
        {
            string path = string.Empty;
            try
            {
                var fileExtension = GetFileExtension(formFile.FileName);
                var item = new Image($"{Guid.NewGuid()}{fileExtension}");
                ImageFileHelper.CreateImageFile(formFile, path = item.Name);

                var newItem = _imageDAO.Create();
                SetValues(item, newItem);
                _unitOfWork.SaveChanges();
                return Ok(new ResponseObject(newItem));
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
            {
                if(!string.IsNullOrEmpty(path))
                    ImageFileHelper.DeleteImageFile(path);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        //[HttpPut]
        //[Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        //public long Put([FromForm] IFormCollection formCollection)
        //{
        //    var oldItem = _imageDAO.GetById(item.Id);
        //    SetValues(item, oldItem);
        //    _unitOfWork.SaveChanges();
        //    return oldItem.Id;
        //    return 0;
        //}

        //[HttpDelete]
        //[Route("/{id}")]
        //public long Delete(long id)
        //{
        //    _imageDAO.Delete(id);
        //    _unitOfWork.SaveChanges();
        //    return id;
        //}

        private void SetValues(Image src, Image dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private string GetFileExtension(string fileName)
        {
            return fileName[fileName.LastIndexOf('.')..];
        }
    }
}
