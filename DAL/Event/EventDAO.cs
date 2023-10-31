using DAL.Common;
using DAL.Params;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class EventDAO
    {
        private const string _errorReceiveObject = "Error when receiving an object {0}";

        private readonly UnitOfWork _unitOfWork;

        public EventDAO(UnitOfWork uow)
        {
            _unitOfWork = uow;
            DbSet = _unitOfWork.DbSet<Event>();
        }

        protected DbSet<Event> DbSet;

        protected IQueryable<Event> DbSetView =>
            from ev in _unitOfWork.DbSet<Event>()
            join mainImg in _unitOfWork.DbSet<Image>() on ev.Image_Id equals mainImg.Id
            join img in _unitOfWork.DbSet<Image>() on ev.Id equals img.Event_Id into subImg
            select new Event
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Address = ev.Address,
                EventDate = ev.EventDate,
                EventDuration = ev.EventDuration,
                Image = mainImg,
                Image_Id = mainImg.Id,
                SubImages = subImg.Where(x => x.Id != ev.Image_Id)
                    .Select(x => new Image { Id = x.Id, Name = x.Name, Event = ev, Event_Id = ev.Id }).ToList(),
            };

        public Event GetById(long id)
        {
            try
            {
                var item = DbSetView.Single(x => x.Id == id);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public Event GetFlatItemById(long id)
        {
            try
            {
                var item = DbSet.Single(x => x.Id == id);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public IEnumerable<Event> GetAll()
        {
            try
            {
                return DbSetView.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public IEnumerable<Event> GetAnnounceEvents(EventAnnouncementParam param)
        {
            try
            {
                return DbSetView.Where(x => x.EventDate > param.CurrentDateTime).OrderBy(x => x.EventDate).Take(3).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public IEnumerable<Event> GetEventsByDate(EventFilterParam param)
        {
            try
            {
                return DbSetView.Where(x => x.EventDate.Date == param.TargetDate.Date).OrderBy(x => x.EventDate).Skip(param.Offset).Take(6).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public Event Create()
        {
            var item = new Event();
            DbSet.Add(item);

            return item;
        }

        public Event GetItemForUpdate(long id)
        {
            var item = DbSet.Local.SingleOrDefault(x => x.Id == id);
            return (item = DbSet.Single(x => x.Id == id));
        }

        public void Delete(long id)
        {
            DbSet.Remove(GetFlatItemById(id));
        }


    }
}
