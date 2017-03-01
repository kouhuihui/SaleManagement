using SaleManagement.Core;
using SaleManagement.Core.Models;
using SaleManagement.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.SpotGoods
{
    public class SpotGoodsViewModel: SpotGoodsBase
    {
        public SpotGoodsViewModel()
        {
            Attachments = new List<AttachmentItem>();
        }

        public IEnumerable<AttachmentItem> Attachments { get; set; }
    }
}