using AutoMapper;
using Microservices.Services.AuthServer.Dtos;
using Microservices.Services.AuthServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.MapProfiles
{
    class DtoMapper : Profile
    {
        public DtoMapper()
        {
            //Her iki taraflı dönüşümü gerçekleştirdim
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
