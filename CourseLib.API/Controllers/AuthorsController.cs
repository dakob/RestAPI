using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
	[Route("api/authors")]
	public class AuthorsController : ControllerBase
	{
		private readonly ICourseLibraryRepository _courseLibraryRepository;
		private readonly IMapper _mapper;

		public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
		{
			_courseLibraryRepository = courseLibraryRepository ??
			                           throw new ArgumentNullException(nameof(courseLibraryRepository));
			_mapper = mapper ?? throw new ArgumentException(nameof(mapper));
		}

		[HttpGet]
		[HttpHead]
		public ActionResult<IEnumerable<AuthorDto>> GetAuthors(string mainCathegory, string searchQuery)
		{
			var authorsFromRepo = _courseLibraryRepository.GetAuthors(mainCathegory, searchQuery);

			return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
		}

		[HttpGet("{authorId}", Name = "GetAuthor")]
		public IActionResult GetAuthor(Guid authorId)
		{
			var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

			if (authorFromRepo == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
		}

		[HttpPost]
		public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto authorForCreationDto)
		{
			var author = _mapper.Map<Author>(authorForCreationDto);
			_courseLibraryRepository.AddAuthor(author);
			_courseLibraryRepository.Save();
			var authorToReturn = _mapper.Map<AuthorDto>(author);

			return CreatedAtRoute("GetAuthor", new {authorId = authorToReturn.Id}, authorToReturn);
		}


	}
}
