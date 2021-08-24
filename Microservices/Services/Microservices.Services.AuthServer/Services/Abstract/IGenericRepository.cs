using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Abstract
{
    //Abstract CRUDs
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        //Asenkron metodu olmadığı için bu şekilde kullandık
        void Remove(TEntity entity);
        //Güncellenmiş datayı geri dönüyoruz
        //Asenkron metodu olmadığı için bu şekilde kullandık
        TEntity Update(TEntity entity);
        //TEntity parametresi alan geriye bool return eden bir FuncDelege tanımlıyoruz
        //Sorguları finalde göndermek için IQueryable tanımladım
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    }
}
