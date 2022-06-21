using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.CoreLayer
{
    public interface IRepository<T>//soyutlarda interface
    {
        //BÜTÜN CLASSLARIMIN ORTAK OLARAK  KULLANDIĞI YER
        //TEMEL CRUD ISLEMLERI
        List<T> List();
        List<T> List(Expression <Func<T, bool>> predicate);//select * from table where id==1 veya dbcontext.Set<T>().where(x=>x.id==1).FirstOrDefault();
        IQueryable<T> QList();
        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        int Save();
        T Find(Expression<Func<T, bool>> predicate);
    }
}
