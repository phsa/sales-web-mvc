using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {

        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (minDate.HasValue)
            {
                ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            }

            if (maxDate.HasValue)
            {
                ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            }

            List<SalesRecord> sales = await _salesRecordService.FindByDateAsync(minDate, maxDate);

            return View(sales);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (minDate.HasValue)
            {
                ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            }

            if (maxDate.HasValue)
            {
                ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            }

            List<IGrouping<Department, SalesRecord>> groupedSales = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);

            //groupedSales.OrderBy(group => group.Key.Name);
            groupedSales.Sort((g1, g2) => g1.Key.Name.CompareTo(g2.Key.Name));

            return View(groupedSales);
        }
    }
}
