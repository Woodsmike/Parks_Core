using AutoMapper;
using ParksAPI.Models;
using ParksAPI.Models.DTOs;

namespace ParksAPI.ParkMapper
{
    public class ParkMappings : Profile
    {
        public ParkMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
