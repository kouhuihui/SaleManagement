using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Models
{
    public class OrderOperationLog
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime Created { get; set; }

        public string Content { get; set; }

        public OrderStatus Status { get; set; }

        public string OrderId { get; set; }
    }
}
