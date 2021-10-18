using System.Linq;
using API.Entities;

namespace API.DTOs
{
    public class PageResponse
    {
        public int? PageNumber { get; set; }

        public int? TotalRecords { get; set; }

        public IQueryable<Product> Products { get; set; }
    }
}