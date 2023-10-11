using DAL.Common;
using DAL.Params;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Configuration;

namespace BusinessCalendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        private readonly ImageDAO _ImageDAO;
        private readonly UnitOfWork _unitOfWork;

        public ImageController(ILogger<ImageController> logger, UnitOfWork uow)
        {
            _logger = logger;
            _ImageDAO = new ImageDAO(uow);
            _unitOfWork = uow;
        }

        [HttpPost]
        public IActionResult Post(IFormFile formFile)
        {
            var fileExtension = GetFileExtension(formFile.FileName);

            var newItem = _ImageDAO.Create();
            var item = new Image
            {
                Name = $"{Guid.NewGuid()}{fileExtension}"
            };

            var filePath = Path.Combine(".", ConfigurationHelper.GetString("imagesPath"), item.Name);
            var fileInfo = new FileInfo(filePath);
            if(!fileInfo.Exists)
            {
                using (var fs = fileInfo.Create())
                {
                    formFile.CopyTo(fs);
                }
            }
            
            SetValues(item, newItem);
            _unitOfWork.SaveChanges();
            return Ok(newItem);
        }

        //[HttpPut]
        //[Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        //public long Put([FromForm] IFormCollection formCollection)
        //{
        //    var oldItem = _ImageDAO.GetById(item.Id);
        //    SetValues(item, oldItem);
        //    _unitOfWork.SaveChanges();
        //    return oldItem.Id;
        //    return 0;
        //}

        //[HttpDelete]
        //[Route("/{id}")]
        //public long Delete(long id)
        //{
        //    _ImageDAO.Delete(id);
        //    _unitOfWork.SaveChanges();
        //    return id;
        //}

        private void SetValues(Image src, Image dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private void GetFormParameters(IFormCollection formCollection, out IFormFile file, out string extension)
        {
            if (formCollection?.ContainsKey("files") ?? false)
            {
                file = formCollection.Files[0];
                extension = GetFileExtension(file.FileName);
            }
            else
            {
                throw new ArgumentNullException("Файл не передан!");
            }
        }

        private string GetFileExtension(string fileName)
        {
            return fileName[fileName.LastIndexOf('.')..];
        }
    }
}
