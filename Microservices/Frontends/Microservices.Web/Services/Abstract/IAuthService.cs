using Microservices.Web.Models;
using Microservices.Web.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Abstract
{
    public interface IAuthService
    {
        Task<Response<UserDto>> Login(SignInInput model);
        Task<Response<TokenDto>> GetAccessByTokenByRefreshToken();
        Task<Response<NoContent>> LogOut();
    }
}
