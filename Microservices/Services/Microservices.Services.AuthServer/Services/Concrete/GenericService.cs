using AutoMapper;
using Microservices.Services.AuthServer.Services.Abstract;
using Microservices.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Concrete
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _uOw;
        private readonly IGenericRepository<TEntity> _genericRepo;
        private readonly IMapper _mapper;

        public GenericService(IUnitOfWork uOw, IGenericRepository<TEntity> genericRepo, IMapper mapper)
        {
            _uOw = uOw;
            _genericRepo = genericRepo;
            _mapper = mapper;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = _mapper.Map<TEntity>(entity);
            await _genericRepo.AddAsync(newEntity);
            await _uOw.CommitAsync();
            var newDto = _mapper.Map<TDto>(newEntity);
            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = _mapper.Map<List<TDto>>(await _genericRepo.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(entities, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity = _mapper.Map<TDto>(await _genericRepo.GetByIdAsync(id));

            if (entity == null)
                return Response<TDto>.Fail("Id Not Found", 404);

            var newDto = _mapper.Map<TDto>(entity);
            return Response<TDto>.Success(newDto, 200);
        }

        public Response<NoContent> Remove(TDto entity)
        {
            var newEntity = _mapper.Map<TEntity>(entity);
            _genericRepo.Remove(newEntity);
            _uOw.Commit();

            return Response<NoContent>.Success(200);
        }

        public Response<NoContent> Update(TDto entity)
        {
            var newEntity = _mapper.Map<TEntity>(entity);
            _genericRepo.Update(newEntity);
            _uOw.Commit();

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepo.Where(predicate);

            return Response<IEnumerable<TDto>>.Success(_mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
        }
    }
}
