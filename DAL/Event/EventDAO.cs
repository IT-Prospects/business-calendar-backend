using DAL.Common;
using DAL.Params;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Model;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DAL
{
    public class EventDAO
    {
        private UnitOfWork _unitOfWork;

        public EventDAO(UnitOfWork uow)
        {
            _unitOfWork = uow;
            DbSet = _unitOfWork.DbSet<Event>();
        }

        protected DbSet<Event> DbSet;

        protected IQueryable<Event> DbSetView => 
            from ev in DbSet
            join img in _unitOfWork.DbSet<Image>() on ev.Image_Id equals img.Id
            select new Event 
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Address = ev.Address,
                EventDate = ev.EventDate,
                EventDuration = ev.EventDuration,
                Image = img,
                Image_Id = img.Id,
            };

        public virtual Event GetById(long id)
        {
            try
            {
                return DbSetView.Single(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении объекта {DbSet.GetType()}", ex);
            }
        }

        public virtual IEnumerable<Event> GetAll()
        {
            try
            {
                return DbSet.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении объекта {DbSet.GetType()}", ex);
            }
        }

        public virtual IEnumerable<Event> GetAll(EventFilterParam param)
        {
            try
            {
                Func<Event, bool> func = x => true;

                if (param.CurrentDate.HasValue)
                {               
                    func = x => x.EventDate >= param.CurrentDate.Value && x.EventDate <= param.CurrentDate.Value.AddDays(3);
                }
                else if (param.TargetDate.HasValue)
                {
                    func = x => x.EventDate.Date == param.TargetDate.Value.Date;
                }

                return DbSet.Where(func).Skip(param.Offset ?? 0).Take(6).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении объекта {DbSet.GetType()}", ex);
            }
        }

        public virtual Event Create()
        {
            var item = new Event();
            DbSet.Add(item);
            return item;
        }

        public virtual void Delete(long id)
        {
            DbSet.Remove(GetById(id));
        }
    }
}
