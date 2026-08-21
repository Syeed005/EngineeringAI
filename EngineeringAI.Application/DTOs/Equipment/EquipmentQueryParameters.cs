using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.DTOs.Equipment {
    public class EquipmentQueryParameters {
        //The MaxPageSize = 100 is intentional so a caller cannot request something excessive such as:
        //private const int MaxPageSize = 100;
        //private int _pageNumber = 1;
        //private int _pageSize = 25;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 25;

        //search & sort
        public string? Search { get; set; }

        public string? SortBy { get; set; }

        public string SortDirection { get; set; } = "asc";


        //public int PageNumber {
        //    get => _pageNumber;
        //    set => _pageNumber = value < 1 ? 1 : value;
        //}

        //public int PageSize {
        //    get => _pageSize;
        //    set {
        //        if (value < 1)
        //            _pageSize = 25;
        //        else if (value > MaxPageSize)
        //            _pageSize = MaxPageSize;
        //        else
        //            _pageSize = value;
        //    }
        //}

        public int? ProjectId { get; set; }

        public int? SupplierId { get; set; }

        public string? EquipmentType { get; set; }

        public string? Status { get; set; }
    }
}
