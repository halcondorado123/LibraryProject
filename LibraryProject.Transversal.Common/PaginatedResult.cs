using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Transversal.Common
{
    // Por revision
    public class PaginatedResult <T>
    {
            public IEnumerable<T> Items { get; set; } = [];
            public int TotalCount { get; set; }
            public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
}
