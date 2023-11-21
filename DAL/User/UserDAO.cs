using DAL.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class UserDAO
    {
        private const string _errorReceiveObject = "Error when receiving an object {0}";

        private readonly UnitOfWork _unitOfWork;

        public UserDAO(UnitOfWork uow)
        {
            _unitOfWork = uow;
            DbSet = _unitOfWork.DbSet<User>();
        }

        protected DbSet<User> DbSet;

        protected IQueryable<User> DbSetView => DbSet;

        public User GetById(long id)
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

        public bool HasDuplicateByEmailOrPhoneNumber(string email, string phoneNumber)
        {
            try
            {
                return DbSetView.Any(x => x.Email == email || x.PhoneNumber == phoneNumber);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public User? GetByEmail(string email)
        {
            try
            {
                return DbSetView.SingleOrDefault(x => x.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public User Create()
        {
            var item = new User();
            DbSet.Add(item);
            return item;
        }

        public User GetItemForUpdate(long id)
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
