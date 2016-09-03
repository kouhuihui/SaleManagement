using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Protal.Models.DailyGoldPrice
{
    public class DailyGoldPriceViewModel
    {
        public DailyGoldPriceViewModel()
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public DailyGoldPriceViewModel(Core.Models.DailyGoldPrice dailyGoldPrice)
        {
            Id = dailyGoldPrice.Id;
            ColorFormId = dailyGoldPrice.ColorForm.Id;
            ColorFormName = dailyGoldPrice.ColorForm.Name;
            Date = dailyGoldPrice.Date.ToString("yyyy-MM-dd"); ;
            Price = dailyGoldPrice.Price;
        }

        public int Id { get; set; }

        [Display(Name = "成色")]
        [Required(ErrorMessage = "请选择{0}")]
        public int ColorFormId { get; set; }

        public string ColorFormName { get; set; }

        [Display(Name ="日期")]
        [Required(ErrorMessage ="请填写{0}")]
        public string Date { get; set; }

        public double Price { get; set; }

        public IEnumerable<ColorForm> ColorForms { get; set; }
    }
}