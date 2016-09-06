﻿using Dickson.Core.Common.Extensions;
using SaleManagement.Core;
using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Reconciliation
{
    public class ReconciliationItemViewModel
    {
        public ReconciliationItemViewModel()
        {

        }

        public ReconciliationItemViewModel(Core.Models.Reconciliation model)
        {
            Amount = model.Amount;
            CustomerId = model.CustomerId;
            CustomerName = model.CustomerName;
            CreatorId = model.CreatorId;
            CompanyId = model.CompanyId;
            Created = model.Created.ToString(SaleManagentConstants.UI.DateStringFormat);
            Type = model.Type;
            ReconciliationTypeName = model.Type.GetDisplayName();
        }

        [Display(Name = "金额")]
        [Required(ErrorMessage = "请输入{0}")]
        [Range(1, 999999, ErrorMessage = "请输入金额范围1-999999")]
        public double Amount { get; set; }

        [Display(Name = "客户")]
        [Required(ErrorMessage = "请选择{0}")]
        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CreatorId { get; set; }

        public int CompanyId { get; set; }

        public string Created { get; set; }

        public ReconciliationType Type { get; set; }

        public string ReconciliationTypeName { get; set; }
    }
}