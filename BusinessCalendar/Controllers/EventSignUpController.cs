using BusinessCalendar.Common;
using BusinessCalendar.Helpers;
using DAL.Common;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model;
using System.Text;
using ClosedXML.Excel;
using ICSharpCode.SharpZipLib.Zip;
using Model.Consts;
using ICSharpCode.SharpZipLib.Core;

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

        [HttpPost]
        [Route("")]
        public IActionResult Post(EventSignUpDTO itemDTO)
        {
            try
            {
                if (!IsValidDTO(itemDTO, out var message))
                {
                    return BadRequest(new ResponseObject(message));
                }
                var item = MappingToDomainObject(itemDTO);

                if (item.Event!.EventDate < DateTime.UtcNow)
                {
                    throw new Exception("It is forbidden to register for an event that has already taken place");
                }
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

        [HttpGet]
        [Route("/Report/event_id={event_Id}")]
        public IActionResult GetEventSignUpReport(long event_Id)
        {
            try
            {
                var eventSignUps = _eventSignUpDAO.GetByEventId(event_Id).ToList();
                if (!eventSignUps.Any())
                {
                    //There are no registrations for this event
                    return NoContent();
                }

                var bytes = GetReportArchive(eventSignUps);

                return File(bytes,
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    $"Report_{eventSignUps[0].Event!.Title}_{DateTime.Now:dd.MM.yyyy_HH:mm:SSzz}.zip");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject(ExceptionHelper.GetFullMessage(ex)));
            }
        }

        private void GetReportExcel(IReadOnlyList<EventSignUp> eventSignUps, Stream outputStream)
        {
            try
            {
                using var workbook = ExcelHelper.LoadTemplate(ExcelTemplateConsts.EventSignUpsTemplate);
                var sheet = ExcelHelper.GetSheet(workbook, 1);

                var cells = new object[eventSignUps.Count, 3];
                for (var i = 0; i < eventSignUps.Count; i++)
                {
                    cells[i, 0] = $"{eventSignUps[i].LastName} {eventSignUps[i].FirstName} {eventSignUps[i].Patronymic}";
                    cells[i, 1] = eventSignUps[i].PhoneNumber;
                    cells[i, 2] = eventSignUps[i].Email;
                }

                ExcelHelper.SetCellValues(sheet, 2, 1, cells);
                ExcelHelper.SetBorderAtRange(sheet, 1, 1, eventSignUps.Count + 1, 3, XLBorderStyleValues.Thin);
                ExcelHelper.AutoSizeCells(sheet);

                ExcelHelper.Save(workbook, outputStream);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred when forming an Excel document.", ex);
            }
        }

        private byte[] GetReportArchive(IReadOnlyList<EventSignUp> eventSignUps)
        {
            var tempArchivePath = $"temp_{Guid.NewGuid()}.zip";
            try
            {
                using var excelStream = new MemoryStream();
                GetReportExcel(eventSignUps, excelStream);

                using var fileStream = System.IO.File.Create(tempArchivePath);
                using var zipFileStream = new ZipOutputStream(fileStream);

                var ev = eventSignUps[0].Event!;
                zipFileStream.SetLevel(9);
                zipFileStream.Password = ev.ArchivePassword;

                var entry = new ZipEntry($"{ev.Title}.xlsx");
                entry.DateTime = DateTime.Now;
                zipFileStream.PutNextEntry(entry);

                excelStream.Seek(0, SeekOrigin.Begin);
                StreamUtils.Copy(excelStream, zipFileStream, new byte[4096]);

                zipFileStream.IsStreamOwner = true;
                zipFileStream.Finish();

                fileStream.Seek(0, SeekOrigin.Begin);
                var bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, (int)fileStream.Length);

                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred when forming the ZIP file.", ex);
            }
            finally
            {
                System.IO.File.Delete(tempArchivePath);
            }
        }

        private void SetValues(EventSignUp src, EventSignUp dst)
        {
            _unitOfWork.Context().Entry(dst).CurrentValues.SetValues(src);
        }

        private bool IsValidDTO(EventSignUpDTO item, out string message)
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
                PhoneNumber = item.PhoneNumber,
                Event_Id = item.Event_Id
            };
        }
    }
}
