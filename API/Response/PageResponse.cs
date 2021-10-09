using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class PageResponse
    {
        public int? PageNumber { get; set; }

        public int? TotalRecords {get; set;}

        public IEnumerable<Products> Products {get;set;}
    }
}