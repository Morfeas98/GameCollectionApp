using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameCollection.Application.DTOs;
using GameCollection.Application.Services;
using GameCollection.Domain.Entities;

namespace GameCollection.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.FranchiseName,
                    opt => opt.MapFrom(src => src.Franchise != null ? src.Franchise.Name : null))
                .ForMember(dest => dest.Platforms,
                    opt => opt.MapFrom(src => src.GamePlatforms.Select(gp => gp.Platform.Name)))
                .ForMember(dest => dest.Genres,
                    opt => opt.MapFrom(src => src.GameGenres.Select(gg => gg.Genre.Name)));

            CreateMap<CreateGameDto, Game>()
                .ForMember(dest => dest.GamePlatforms, opt => opt.Ignore())
                .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
                .ForMember(dest => dest.Franchise, opt => opt.Ignore());

            CreateMap<Platform, string>().ConvertUsing(p => p.Name);
            CreateMap<Genre, string>().ConvertUsing(g => g.Name);

            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.CollectionCount,
                    opt => opt.MapFrom(src => src.Collections.Count));

            CreateMap<GameCollection.Domain.Entities.GameCollection, CollectionDto>()
                .ForMember(dest => dest.GameCount,
                    opt => opt.MapFrom(src => src.CollectionGames.Count(cg => !cg.IsDeleted)));

            CreateMap<CollectionGame, CollectionGameDto>()
                .ForMember(dest => dest.GameTitle,
                    opt => opt.MapFrom(src => src.Game.Title));
        }
    }
}
