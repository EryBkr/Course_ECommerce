using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Abstract
{
    //Abstract Services
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        //Shared tarafında oluşturmuş olduğumuz Response leri dönüyoruz
        Task<Response<TDto>> GetByIdAsync(int id);

        Task<Response<IEnumerable<TDto>>> GetAllAsync();

        //TEntity parametresi alan geriye bool return eden bir FuncDelege tanımlıyoruz
        //Sorguları finalde göndermek için IQueryable tanımladım
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);

        //DB de oluşturduğum datayı dto ya çevirip response ile paketleyip dönüyorum
        Task<Response<TDto>> AddAsync(TDto entity);

        //Asenkron metodu olmadığı için bu şekilde kullandık
        //Response dönebilmek için NoData adında boş bir class ekledim
        Response<NoContent> Remove(TDto entity);

        //Güncelleme işleminden sonra Response NoData dönüyorum
        //Asenkron metodu olmadığı için bu şekilde kullandık
        Response<NoContent> Update(TDto entity);


    }
}
