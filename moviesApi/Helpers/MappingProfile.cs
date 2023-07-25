﻿using AutoMapper;

namespace moviesApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<Movie,MovieDetailsDto>();
            CreateMap<MovieDto, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
            
        }
    }
}
