using DAL.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class ImageDAO
    {
        private const string _errorReceiveObject = "Error when receiving an object {0}";

        private readonly UnitOfWork _unitOfWork;

        public ImageDAO(UnitOfWork uow)
        {
            _unitOfWork = uow;
            DbSet = _unitOfWork.DbSet<Image>();
        }

        protected DbSet<Image> DbSet;

        protected IQueryable<Image> DbSetView => DbSet;

        public Image GetById(long id)
        {
            try
            {
                return DbSet.Single(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public Image GetByName(string name)
        {
            try
            {
                return DbSet.Single(x => x.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public Image Create()
        {
            var item = new Image();
            DbSet.Add(item);
            return item;
        }

        public void Delete(long id)
        {
            DbSet.Remove(GetById(id));
        }
    }
}
