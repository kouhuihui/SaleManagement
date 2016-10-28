using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManagement.Protal.Models.Reconciliation
{
    public enum ArrearageType
    {
        [Display(Name = "新货")]
        New = 0,

        [Display(Name = "维修货")]
        Repair = 1
    }
}