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

        protected IQueryable<Image> DbSetView => DbSet.Include(x => x.Event);

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

        public IEnumerable<Image> GetSubImagesByEventId(long event_Id)
        {
            try
            {
                return (from img in DbSetView
                        where img.Event_Id == event_Id && img.Event.Image_Id != img.Id
                        select img).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(_errorReceiveObject, DbSet.GetType()), ex);
            }
        }

        public IEnumerable<string> GetAllPathsByEventId(long event_Id)
        {
            try
            {
                return DbSet.Where(x => x.Event_Id == event_Id).Select(x => x.Name).ToList();
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

        public Image GetItemForUpdate(long id)
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
