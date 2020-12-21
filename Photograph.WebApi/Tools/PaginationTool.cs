using Photograph.BLL.Services;
using Photograph.BLL.Services.AlbumService;
using Photograph.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photograph.WebApi.Tools
{
	public class PaginationTool
	{
		public static object SetPagination(PagingParameterModel paging, int count)
		{
			var previousPage = paging.pageNumber > 1 ? 1 : 0;
			var nextPage = count > (paging.pageSize*paging.pageNumber) ? 1 : 0;
			var lastPage = (int) Math.Ceiling(((double) count)/paging.pageSize);

			var paginationMetadata = new
			{
				pageSize = paging._pageSize,
				currentPage = paging.pageNumber,
				previousPage,
				nextPage,
				lastPage
			};

			return paginationMetadata;
		}
	}
}