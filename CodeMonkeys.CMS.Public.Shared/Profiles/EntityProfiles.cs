using AutoMapper;

using CodeMonkeys.CMS.Public.Shared.DTOs;
using CodeMonkeys.CMS.Public.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Shared.Profiles
{
    public class EntityProfiles : Profile
    {
        public EntityProfiles() { }

        public EntityProfiles(string profileName) : base(profileName) { }

        public void CreateMappings()
        {
            CreateMap<Content, ContentDto>()
                // Map the nullable Author property of the Content entity to the nullable AuthorDto property of the ContentDto
                .ForMember(
                    dest => dest.Author,
                    opt => opt.MapFrom(src => src.Author == null ? null : new AuthorDto
                    {
                        Id = src.AuthorId ?? Guid.Empty,
                        Name = src.Author!.UserName ?? string.Empty,
                        Email = src.Author!.Email ?? string.Empty
                    }));

            CreateMap<WebPage, WebPageDto>()
                // Map the nullable Author property of the WebPageDto to the nullable Author property of the WebPage entity
                .ForMember(
                    dest => dest.Author,
                    opt => opt.MapFrom(src => src.Author == null ? null : new AuthorDto
                    {
                        Id = src.AuthorId ?? Guid.Empty,
                        Name = src.Author!.UserName ?? string.Empty,
                        Email = src.Author!.Email ?? string.Empty
                    }))
                .ForMember(
                    dest => dest.Contents,
                    opt => opt.MapFrom(src => src.Contents));

        }
    }
}