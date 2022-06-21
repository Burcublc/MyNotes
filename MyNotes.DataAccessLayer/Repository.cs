using MyNote.CoreLayer;
using MyNotes.Common;
using MyNotes.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.DataAccessLayer
{
    public class Repository<T> :BaseRepository, IRepository<T> where T:class//somutlarda class
    {
        private readonly MyNotesContext _db;
        private DbSet<T> objSet;

        public Repository()
        {
            _db = BaseRepository.CreateContext();
            objSet = _db.Set<T>();
        }

        public int Delete(T entity)
        {
            objSet.Remove(entity);
            return Save(); //savechanges işlemi yapmak için save 'e gönderdik ve onu return ettik
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return objSet.FirstOrDefault(predicate);
        }

        public int Insert(T entity)
        {
            objSet.Add(entity);
            if(entity is BaseEntity o) //entity baseentity ise
            {
                DateTime now=DateTime.Now;//süre farkını önlemek için
                /*BaseEntity o=entity as BaseEntity;*/ //burda BaseEntity 'i new 'leyemeyiz çünkü verileri kaybederiz onun yerine yukarıdan gelen entity değerinin içerisinde var nasıl olsa ordan aldık
                o.ModifiedOn = now;
                o.ModifiedUserName = App.Common.GetCurrentUserName();
                o.CreatedOn = now;
            }
            return Save();
        }

        public List<T> List() //birçok programda list yerine GetAll kullanılır
        {
            return objSet.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> predicate)
        {
            return objSet.Where(predicate).ToList();
        }

        public IQueryable<T> QList()
        {
            return objSet.AsQueryable();
        }

        public int Save()
        {
            return _db.SaveChanges();
        }

        public int Update(T entity)
        {
            if(entity is BaseEntity o)
            {
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = App.Common.GetCurrentUserName();
            }
            return Save();
        }
    }
}
