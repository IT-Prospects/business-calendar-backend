using DAL.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class ImageDAO
    {
        private UnitOfWork _unitOfWork;

        public ImageDAO(UnitOfWork uow)
        {
            _unitOfWork = uow;
            DbSet = _unitOfWork.DbSet<Image>();
        }

        protected DbSet<Image> DbSet;

        protected IQueryable<Image> DbSetView => DbSet;

        public virtual Image GetById(long id)
        {
            try
            {
                return DbSet.Single(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении объекта {DbSet.GetType()}", ex);
            }
        }

        public virtual Image GetByName(string name)
        {
            try
            {
                return DbSet.Single(x => x.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении объекта {DbSet.GetType()}", ex);
            }
        }

        public virtual Image Create()
        {
            var item = new Image();
            DbSet.Add(item);
            return item;
        }

        public virtual void Delete(long id)
        {
            DbSet.Remove(GetById(id));
        }
    }
}
