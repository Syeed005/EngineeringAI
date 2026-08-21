using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Common {
    public class PagedResult<T> {
        public IReadOnlyList<T> Items { get; set; } = [];

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
