using AutoMapper;
using Microservices.Services.AuthServer.Dtos;
using Microservices.Services.AuthServer.Entities;
using Microservices.Services.AuthServer.Services.Abstract;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        //Kullanıcı Kaydı
        public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User { Email = createUserDto.Email, UserName = createUserDto.UserName };

            //Kullanıcıyı oluşturuyorum
            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                //Identity den aldığımız hataları dönüyoruz
                var errors = result.Errors.Select(i => i.Description).ToList();
                return Response<UserDto>.Fail(errors, 400);
            }

            //User nesnemi UserDto a map ledim
            var mappedUser = _mapper.Map<UserDto>(user);

            //Hata yoksa UserDto yu dönüyoruz
            return Response<UserDto>.Success(mappedUser, 200);
        }

        public async Task<Response<UserDto>> GetUserByNameAsync(string userName)
        {
            //Kullanıcı adından user a ulaşıyoruz
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return Response<UserDto>.Fail("Username not found", 404);

            //User nesnemi UserDto a map ledim
            var mappedUser = _mapper.Map<UserDto>(user);

            //Hata yoksa UserDto yu dönüyoruz
            return Response<UserDto>.Success(mappedUser, 200);
        }

    }
}
