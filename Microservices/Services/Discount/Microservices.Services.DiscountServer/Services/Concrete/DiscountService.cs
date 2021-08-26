using Dapper;
using Microservices.Services.DiscountServer.Models;
using Microservices.Services.DiscountServer.Services.Abstract;
using Microservices.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.DiscountServer.Services.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration; //Connection String için aldık
        private readonly IDbConnection _dbConnection; //Sql Bağlantısı için aldık.Postgreye özel bir arayüz değil daha genel bir interface

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql")); //Postgre bağlantısını burada oluşturduk
        }

        public async Task<Response<NoContent>> Add(Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("insert into discount (userid,rate,code) values (@UserId,@Rate,@Code)", new { UserId = discount.UserId, Rate = discount.Rate, Code = discount.Code });

            if (saveStatus > 0)
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("An Error Accured while adding", 500);
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("delete from discount where id=@id", new { id = id });

            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<List<Discount>>> GetAll()
        {
            //Dapper aracılığıyla get all işlemini gerçekleştirdik
            var discounts = await _dbConnection.QueryAsync<Discount>("Select * from discount");
            return Response<List<Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Discount>> GetByCodeAndUserId(string code, string userId)
        {
            //Kullanıcıya ait kupon kodu var mı?
            var discount = await _dbConnection.QueryFirstOrDefaultAsync<Discount>("select * from discount where userid=@userId and code=@code", new { userid = userId, code = code });

            if (discount == null)
                return Response<Discount>.Fail("Discount not found", 404);

            return Response<Discount>.Success(discount, 200);
        }

        public async Task<Response<Discount>> GetById(int id)
        {
            //Dapper aracılığıyla get by id işlemini gerçekleştirdik
            var discount = await _dbConnection.QueryFirstOrDefaultAsync<Discount>("select * from discount where id=@Id", new { Id = id });

            if (discount == null)
                return Response<Discount>.Fail("Discount not found", 404);

            return Response<Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Update(Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("update discount set userid=@UserId,rate=@Rate,code=@Code where id=@Id", new { UserId = discount.UserId, Rate = discount.Rate, Code = discount.Code, Id = discount.Id });

            if (saveStatus > 0)
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("An Error Accured while updating", 500);
        }
    }
}
