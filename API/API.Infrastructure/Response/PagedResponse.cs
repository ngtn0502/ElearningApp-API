using System.Collections.Generic;

namespace API.DTOs
{
    public class PagedResponse
    {
        public int? PageNumber { get; set; }

        public int? TotalRecords { get; set; }

        public IEnumerable<ProductDTOs> Products { get; set; }
    }
}