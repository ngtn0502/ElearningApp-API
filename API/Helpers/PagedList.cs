using System.Collections.Generic;
using System.Linq;
using API.DTOs;
using API.Entities;

namespace API.Helpers
{
    public class PagedList
    {
        public static PageResponse CreatePagedResponse(List<Product> products, int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentpageSize = pageSize ?? 6;

            var productsPage = products
            .Skip((currentPageNumber - 1) * currentpageSize)
            .Take(currentpageSize).AsQueryable();

            var response = new PageResponse
            {
                PageNumber = currentPageNumber,
                TotalRecords = products.Count,
                Products = productsPage
            };

            return response;
        }

    }
}