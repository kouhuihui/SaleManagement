﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class OrderSetStoneInfo
    {
        public OrderSetStoneInfo()
        {
            Created = DateTime.Now;
        }

        public int Id { get; set; }

        public int MatchStoneId { get; set; }

        public string MathchStoneName { get; set; }

        public double Price { get; set; }

        public double WorkingCost { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int Number { get; set; }

        public double Weight { get; set; }

        public DateTime Created { get; set; }

        public string  CreatorId { get; set; }
    }
}
