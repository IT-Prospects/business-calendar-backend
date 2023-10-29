using DAL.Common;
using DAL.Params;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EventSignUpDAO
    {
        private const string _errorReceiveObject = "Error when receiving an object {0}";

        private readonly UnitOfWork _unitOfWork;

        public EventSignUpDAO(UnitOfWork uow)
        {
            _unitOfWork = uow;
            DbSet = _unitOfWork.DbSet<EventSignUp>();
        }

        protected DbSet<EventSignUp> DbSet;

        protected IQueryable<EventSignUp> DbSetView =>
            DbSet.Include(x => x.Event);

        public EventSignUp GetById(long id)
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

        public IEnumerable<EventSignUp> GetAll()
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

        public IEnumerable<EventSignUp> GetAllByEvent(long event_Id)
        {
            try
            {
                return DbSetView.Where(x => x.Event_Id == event_Id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public EventSignUp Create()
        {
            var item = new EventSignUp();
            DbSet.Add(item);
            return item;
        }

        public EventSignUp GetItemForUpdate(long id)
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
