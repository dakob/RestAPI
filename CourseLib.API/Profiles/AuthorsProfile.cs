using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLib.API.Helpers;
using CourseLibrary.API.Entities;

namespace CourseLib.API.Profiles
{
	public class AuthorsProfile: Profile
	{
		public AuthorsProfile()
		{
			CreateMap<Author, Models.AuthorDto>()
				.ForMember(dest => dest.Name,
					opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
				.ForMember(dest => dest.Age,
					opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

			CreateMap<Models.AuthorForCreationDto, Author>();
		}
	}
}
