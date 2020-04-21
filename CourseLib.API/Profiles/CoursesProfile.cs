using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLib.API.Models;
using CourseLibrary.API.Entities;

namespace CourseLib.API.Profiles
{
	public class CoursesProfile: Profile
	{
		public CoursesProfile()
		{
			CreateMap<Course, CourseDto>();
			CreateMap<CourseForCreationDto, Course>();
		}
	}
}
