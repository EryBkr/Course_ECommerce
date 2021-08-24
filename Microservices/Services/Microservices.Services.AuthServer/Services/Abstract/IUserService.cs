using Microservices.Services.AuthServer.Dtos;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Abstract
{
    //Kullanıcı oluşturma ve kullanıcıya ait bilgiyi bize getirme işlemlerinden sorumlu olacak interface
    public interface IUserService
    {
        Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserDto>> GetUserByNameAsync(string userName);
    }
}
