﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.ViewModel
{
    public class ReportQueryBaseDto
    {
        public string CustomerId { get; set; }

        public DateTime? StatisticStartDate { get; set; }

        public DateTime? StatisticEndDate { get; set; }
    }
}