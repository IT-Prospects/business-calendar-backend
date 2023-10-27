﻿using DAL.Common;
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
            DbSet.Include(x => x.Images);

        public Event GetById(long id)
        {
            try
            {
                return DbSetView.Single(x => x.Id == id);
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
                return DbSetView;
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
            DbSet.Remove(GetById(id));
        }
    }
}
