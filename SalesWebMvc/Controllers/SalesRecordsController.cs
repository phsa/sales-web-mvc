using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
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
            List<SalesRecord> sales = await _salesRecordService.FindByDateAsync(minDate, maxDate);

            if (minDate.HasValue)
            {
                ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            }

            if (maxDate.HasValue)
            {
                ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            }

            return View(sales);
        }

        public IActionResult GroupingSearch()
        {
            return View();
        }
    }
}
