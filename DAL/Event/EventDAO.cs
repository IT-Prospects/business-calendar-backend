using DAL.Common;
using DAL.Params;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Model;
using System;
using System.Diagnostics.CodeAnalysis;
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

        public virtual IEnumerable<Event> GetAnnounceEvents(EventAnnouncementParam param)
        {
            try
            {
                return DbSet.Where(x => x.EventDate >= param.CurrentDate).Take(3).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении объекта {DbSet.GetType()}", ex);
            }
        }

        public virtual IEnumerable<Event> GetEventsByDate(EventFilterParam param)
        {
            try
            {
                return DbSet.Where(x => x.EventDate.Date == param.TargetDate.Date).Skip(param.Offset).Take(6).ToList();
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

        public virtual Event GetItemForUpdate(long id)
        {
            var item = DbSet.Local.SingleOrDefault(x => x.Id == id);
            return (item = DbSet.Single(x => x.Id == id));
        }

        public virtual void Delete(long id)
        {
            DbSet.Remove(GetById(id));
        }
    }
}
