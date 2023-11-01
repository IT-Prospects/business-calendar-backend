using DAL.Common;
using Microsoft.EntityFrameworkCore;
using Model;

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
