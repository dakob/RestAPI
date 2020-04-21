using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLib.API.Helpers;
using CourseLib.API.Models;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLib.API.Controllers
{
	[ApiController]
	[Route("api/authorcollection")]
	public class AuthorCollectionController: ControllerBase
	{
		private readonly ICourseLibraryRepository _courseLibraryRepository;
		private readonly IMapper _mapper;

		public AuthorCollectionController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
		{
			_courseLibraryRepository = courseLibraryRepository ??
			                           throw new ArgumentNullException(nameof(ICourseLibraryRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));
		}

		[HttpGet("({ids})", Name = "GetAuthorCollection")]
		public  IActionResult GetAuthorColleciton(
			[FromRoute]
			[ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
		{
			if (ids == null)
			{
				return BadRequest();
			}

			var authorEntities = _courseLibraryRepository.GetAuthors(ids);

			if (ids.Count() != authorEntities.Count())
			{
				return NotFound();
			}

			var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
			return Ok(authorsToReturn);
		}

		[HttpPost]
		public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(
			IEnumerable<AuthorForCreationDto> authorCollection)
		{
			var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);
			foreach (var item in authorEntities)
			{
				_courseLibraryRepository.AddAuthor(item);
			}

			_courseLibraryRepository.Save();

			var authorsCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
			var idsStrings = string.Join(",", authorsCollectionToReturn.Select(a => a.Id));

			return CreatedAtRoute("GetAuthorCollection", new {ids = idsStrings}, authorsCollectionToReturn);
		}
	}
}
