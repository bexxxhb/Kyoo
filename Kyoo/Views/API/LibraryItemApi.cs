using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kyoo.CommonApi;
using Kyoo.Controllers;
using Kyoo.Models;
using Kyoo.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Kyoo.Api
{
	[Route("api/item")]
	[Route("api/items")]
	[ApiController]
	public class LibraryItemApi : ControllerBase
	{
		private readonly ILibraryItemRepository _libraryItems;
		private readonly string _baseURL;


		public LibraryItemApi(ILibraryItemRepository libraryItems, IConfiguration configuration)
		{
			_libraryItems = libraryItems;
			_baseURL = configuration.GetValue<string>("public_url").TrimEnd('/');
		}

		[HttpGet]
		[Authorize(Policy = "Read")]
		public async Task<ActionResult<Page<LibraryItem>>> GetAll([FromQuery] string sortBy,
			[FromQuery] int afterID,
			[FromQuery] Dictionary<string, string> where,
			[FromQuery] int limit = 50)
		{
			where.Remove("sortBy");
			where.Remove("limit");
			where.Remove("afterID");

			try
			{
				ICollection<LibraryItem> ressources = await _libraryItems.GetAll(
					ApiHelper.ParseWhere<LibraryItem>(where),
					new Sort<LibraryItem>(sortBy),
					new Pagination(limit, afterID));

				return new Page<LibraryItem>(ressources, 
					_baseURL + Request.Path,
					Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString(), StringComparer.InvariantCultureIgnoreCase),
					limit);
			}
			catch (ItemNotFound)
			{
				return NotFound();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new {Error = ex.Message});
			}
		}
	}
}